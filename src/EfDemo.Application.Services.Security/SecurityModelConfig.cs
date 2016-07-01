namespace EfDemo.Application.Services.Security
{
    public class SecurityModelConfig
    {
        public string ApplicationUserTableName { get; set; } = "ApplicationUsers";
        public string ApplicationRoleTableName { get; set; } = "ApplicationRoles";
        public string ApplicationUserClaimTableName { get; set; } = "ApplicationUserClaims";
        public string ApplicationUserLoginTableName { get; set; } = "ApplicationUserLogins";
        public string ApplicationUserRoleTableName { get; set; } = "ApplicationUserRoles";
        public string UserIdFieldName { get; set; } = "CreatedById";
        public string UserNameIndex { get; set; } = "UserNameIndex";
        public string RoleNameIndex { get; set; } = "RoleNameIndex";
    }
}