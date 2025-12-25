using System.Security.Claims;

namespace FlipShop.WebApi.Security.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContext;

        public UserService(IHttpContextAccessor httpContext)
        {
            this._httpContext = httpContext;
        }

        public string GetUserId()
        {
            return _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public bool isAuthenticated()
        {
            return _httpContext.HttpContext.User.Identity.IsAuthenticated; 
        }
    }
}
