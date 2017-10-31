using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace NetCoreTemplate.Infrastructure.Authentication.Contracts
{
    public interface IIdentityProvider
    {
        ClaimsPrincipal GetUserIdentity(HttpContext context);
        string AuthenticateUser(string userLogin, string password);
    }
}