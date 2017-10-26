using System;
using System.Threading.Tasks;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using NetCoreTemplate.Config;
using NetCoreTemplate.Helpers;
using NetCoreTemplate.Infrastructure.Authentication.Contracts;
using NetCoreTemplate.Infrastructure.ModuleRouting;

namespace NetCoreTemplate.Api
{
  public class HomeModule : ApiModule
  {

    IIdentityProvider _identityProvider;
    public HomeModule(IAppConfiguration appConfig, IIdentityProvider identityProvider) : base("home")
    {
      Get("/", args => "Hello from Nancy runnin on .Net Core");
      Get("/conneg/{name}", args => Response.AsJson(new { Name = args.name }));
      Get("/smtp", args => Response.AsJson(appConfig.Smtp));

      Post("/add", args =>
      {
        string name = args.Name;
        decimal price = args.Price;

        return new { IsValid = true, Message = "Product added sucessfully", Data = new { name, price } };
      });

      
      Post<LogingRequest>("/login", Login);
      Post<UserDataRequest>(UserInfo, "Customer");
      Post(RequestTime);

      _identityProvider = identityProvider;
    }

    private object RequestTime(dynamic arg)
    {
      return DateTime.UtcNow;
    }

    private UserDto UserInfo(UserDataRequest req)
    {
      return new UserDto
      {
        Id = req.Id,
        Name = "Aldo",
        Lastname = "Romo"
      };
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


  public class UserDataRequest
  {
    public Guid Id { get; set; }
  }

  public class UserDto
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Lastname { get; set; }
  }
}