namespace EfDemo.Application.Services.Security
{
    public class SecurityModelConfig
    {
        public string ApplicationUserTableName { get; set; } = "ApplicationUsers";
        public string IdentityRoleTableName { get; set; } = "ApplicationRoles";
        public string IdentityUserClaimTableName { get; set; } = "ApplicationUserClaims";
        public string IdentityUserLoginTableName { get; set; } = "ApplicationUserLogins";
        public string IdentityUserRoleTableName { get; set; } = "ApplicationUserRoles";
        public string IdentityUserTableName { get; set; } = "ApplicationUsers";
        public string UserIdFieldName { get; set; } = "UserId";
    }
}