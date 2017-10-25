using System.Security.Claims;
using Nancy;

namespace NetCoreTemplate.Infrastructure.Authentication.Contracts
{
    public interface IIdentityProvider
    {
        ClaimsPrincipal GetUserIdentity(NancyContext context);
        string AuthenticateUser(string userLogin, string password);
    }
}