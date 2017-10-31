using NetCoreTemplate.Infrastructure.Authentication.Contracts;
using System.Security.Claims;
using Jose;
using System;
using NetCoreTemplate.Config;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace NetCoreTemplate.Infrastructure.Authentication
{
  internal sealed class IdentityProvider : IIdentityProvider
  {
    private readonly AuthSettings _authSettings;
    private const string _bearerDeclaration = "Bearer ";

    public IdentityProvider(IAppConfiguration appConfig)
    {
      _authSettings = appConfig.AuthSettings;
    }

    public ClaimsPrincipal GetUserIdentity(HttpContext context)
    {
      try
      {
        var authorizationHeader = context.Request.Headers["Authorization"][0];
        var jwt = authorizationHeader.Substring(_bearerDeclaration.Length);

        var authToken = Jose.JWT.Decode<AuthToken>(jwt, _authSettings.SecretKeyBytes, JwsAlgorithm.HS256);

        if (authToken.ExpirationDateTime < DateTime.UtcNow)
          return null;

        return new AuthPrincipal(authToken.UserId, authToken.UserName, authToken.UserLogin, authToken.Roles);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return null;
      }
    }

    public string AuthenticateUser(string userLogin, string password)
    {
      //TODO: Do a real authentication
      var token = new AuthToken
      {
        UserId = Guid.NewGuid(),
        UserName = userLogin,
        UserLogin = userLogin,
        Roles = new List<string> { "Customer" },
        ExpirationDateTime = DateTime.UtcNow.AddDays(5)
      };

      return Jose.JWT.Encode(token, _authSettings.SecretKeyBytes, JwsAlgorithm.HS256);
    }
  }
}