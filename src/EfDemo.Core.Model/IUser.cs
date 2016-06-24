namespace EfDemo.Core.Model
{
    public interface IUser
    {
        int UserId { get; set; }
        string LoginName { get; set; }
        string UserPassword { get; set; }
        string UserNames { get; set; }
        string UserLastName { get; set; }
    }
}