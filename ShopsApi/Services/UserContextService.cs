using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ShopsApi.Services
{

    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccesor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccesor = httpContextAccessor;
        }
        public ClaimsPrincipal User => _httpContextAccesor.HttpContext?.User;
        public int? GetUserId => User is null ? null : (int?)int.Parse(User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);
    }
}
