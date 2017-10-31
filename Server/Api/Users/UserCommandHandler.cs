using System.Collections.Generic;
using NetCoreTemplate.Api.Users.Commands;
using NetCoreTemplate.Infrastructure.Authentication.Contracts;
using NetCoreTemplate.Infrastructure.Middleware.ApiHandler.Contracts;

namespace NetCoreTemplate.Api.Users
{
  public class UserCommandHandler : ApiBaseHandler, ICommandHandler<CreateUserCommand>, ICommandHandler<LoginCommand>
  {
    public IIdentityProvider IdentityProvider { get; }


    public UserCommandHandler(IIdentityProvider identityProvider)
    {
      IdentityProvider = identityProvider;
    }


    public object Handle(CreateUserCommand command)
    {
      return new
      {
        HasError = false,
        Message = $"User {command.NewUser.Name} created successfully"
      };
    }

    public object Handle(LoginCommand command)
    {
      return new
      {
        IsValid = true,
        Token = IdentityProvider.AuthenticateUser(command.UserLogin, command.Password)
      };
    }
  }
}