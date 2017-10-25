using System;
using System.Threading.Tasks;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using NetCoreTemplate.Config;
using NetCoreTemplate.Helpers;
using NetCoreTemplate.Infrastructure.Authentication.Contracts;

namespace NetCoreTemplate.Api
{
  public class HomeModule : NancyModule
  {

    IIdentityProvider _identityProvider;
    public HomeModule(IAppConfiguration appConfig, IIdentityProvider identityProvider)
    {
      Get("/", args => "Hello from Nancy runnin on .Net Core");
      Get("/conneg/{name}", args => Response.AsJson(new { Name = args.name }));
      Get("/smtp", args => Response.AsJson(appConfig.Smtp));

      Post("/add", args =>
      {

        dynamic dto = Context.ToDynamic();
        string name = dto.Name;
        decimal price = dto.Price;

        return Response.AsJson(new { IsValid = true, Message = "Product added sucessfully", Data = new { name, price } });
      });

      // Post("/login", args =>
      // {
      //     dynamic credentials = Context.ToDto<LogingRequest>();
      //     return Response.AsJson(new { IsValid = true, Token = identityProvider.AuthenticateUser(credentials.UserLogin, credentials.Password) });
      // });

      this.Post<LogingRequest,LoginResult>("/login", Login);
      _identityProvider = identityProvider;
    }

    
    private LoginResult Login(LogingRequest req)
    {
      return new LoginResult
      {
        IsValid = true,
        Token = _identityProvider.AuthenticateUser(req.UserLogin, req.Password)
      };
    }
  }

  public class LoginResult
  {
    public bool IsValid { get; set; }
    public string Token { get; set; }
  }

  public class LogingRequest
  {
    public string UserLogin { get; set; }
    public string Password { get; set; }
  }
}