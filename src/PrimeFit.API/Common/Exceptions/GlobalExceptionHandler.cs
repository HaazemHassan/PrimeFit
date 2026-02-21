using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Application.Common.Exceptions;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment environment)
    : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;
    private readonly IHostEnvironment _environment = environment;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var requestPath = httpContext.Request.Path;
        var userId = httpContext.User?.Identity?.Name ?? "Anonymous";
        var isDevelopment = _environment.IsDevelopment();
        var traceId = httpContext.TraceIdentifier;

        switch (exception)
        {
            case ValidationException validationException:
            return await HandleValidationException(
                httpContext,
                validationException,
                requestPath,
                userId,
                traceId,
                cancellationToken);

            case UnauthorizedException e:
            _logger.LogWarning(e, "Unauthorized - Path: {Path}, User: {User}, TraceId: {TraceId}",
                requestPath, userId, traceId);

            return await WriteProblemDetails(
                httpContext,
                StatusCodes.Status401Unauthorized,
                "Unauthorized Access",
                new
                {
                    Code = "Auth.Unauthorized",
                    Description = e.Message
                },
                traceId,
                cancellationToken);

            case ForbiddenException e:
            _logger.LogWarning(e, "Forbidden - Path: {Path}, User: {User}, TraceId: {TraceId}",
                requestPath, userId, traceId);

            return await WriteProblemDetails(
                httpContext,
                StatusCodes.Status403Forbidden,
                "Forbidden Action",
                new
                {
                    Code = "Auth.Forbidden",
                    Description = "You do not have permission to perform this action."
                },
                traceId,
                cancellationToken);

            case KeyNotFoundException e:
            _logger.LogWarning(e,
                "Not Found - Path: {Path}, User: {User}, TraceId: {TraceId}",
                requestPath, userId, traceId);

            return await WriteProblemDetails(
                httpContext,
                StatusCodes.Status404NotFound,
                "Resource Not Found",
                new
                {
                    Code = "Resource.NotFound",
                    Description = e.Message
                },
                traceId,
                cancellationToken);

            case SqlException:
            case DbUpdateException:
            _logger.LogError(exception,
                "Database error - Path: {Path}, User: {User}, TraceId: {TraceId}",
                requestPath, userId, traceId);

            return await WriteProblemDetails(
                httpContext,
                StatusCodes.Status500InternalServerError,
                "Database Error",
                new
                {
                    Code = "Database.Error",
                    Description = isDevelopment
                        ? exception.Message
                        : "A database error occurred."
                },
                traceId,
                cancellationToken);

            default:
            _logger.LogError(exception,
                "Unhandled exception - Path: {Path}, User: {User}, TraceId: {TraceId}",
                requestPath, userId, traceId);

            return await WriteProblemDetails(
                httpContext,
                StatusCodes.Status500InternalServerError,
                "Unexpected Error",
                new
                {
                    Code = "Server.Error",
                    Description = isDevelopment
                        ? exception.Message
                        : "An unexpected error occurred."
                },
                traceId,
                cancellationToken);
        }
    }

    private async Task<bool> HandleValidationException(
        HttpContext httpContext,
        ValidationException exception,
        string path,
        string userId,
        string traceId,
        CancellationToken cancellationToken)
    {
        _logger.LogWarning(exception,
            "Validation error - Path: {Path}, User: {User}, TraceId: {TraceId}",
            path, userId, traceId);

        var errors = exception.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray()
            );

        var problemDetails = new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Failed",
            Instance = path
        };

        problemDetails.Extensions["traceId"] = traceId;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static async Task<bool> WriteProblemDetails(
        HttpContext httpContext,
        int statusCode,
        string title,
        object error,
        string traceId,
        CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {

            Status = statusCode,
            Title = title,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["errors"] = new[] { error };
        problemDetails.Extensions["traceId"] = traceId;

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}