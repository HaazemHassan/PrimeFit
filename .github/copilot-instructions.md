# Copilot Instructions

## Project Guidelines
- Use options pattern as: `services.Configure<TOptions>(configuration.GetSection(...))`; if setup-time values are needed, read a separate copy from configuration, and inject `IOptions<TOptions>` in runtime services instead of injecting options class directly.
- In this project, class/type naming should use `*Options` instead of `*Settings` for configuration models, while configuration section keys may remain unchanged for compatibility.