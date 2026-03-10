namespace PrimeFit.Domain.Common.Enums
{
    public enum Permission
    {
        UsersRead = 1,
        UsersWrite = 2,
        UsersDelete = 3,

        MembersView = 100,
        MembersWrite = 101,
        MembersDelete = 103,

        SubscriptionsView = 200,
        SubscriptionsWrite = 201,
        SubscriptionsCancel = 203,

        PackagesView = 300,
        PackagesWrite = 301,
        PackagesDelete = 303,

        EmployeesView = 400,
        EmployeesWrite = 401,
        EmployeesDelete = 403,

        CheckInWrite = 500
    }
}
