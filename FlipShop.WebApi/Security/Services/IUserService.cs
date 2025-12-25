namespace FlipShop.WebApi.Security.Services
{
    public interface IUserService
    {
        string GetUserId();
        bool isAuthenticated();
    }
}