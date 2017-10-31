using System.Collections.Generic;
using NetCoreTemplate.Api.Users.Queries;
using NetCoreTemplate.Api.Users.Dtos;
using NetCoreTemplate.Infrastructure.Authentication.Contracts;
using NetCoreTemplate.Infrastructure.Middleware.ApiHandler.Contracts;


namespace NetCoreTemplate.Api.Users
{
  public class UserQueryHandler : ApiBaseHandler, IQueryHandler<UserDataQuery>
  {
    public UserQueryHandler():base("Customer")
    {
    }

    public object Handle(UserDataQuery query)
    {
      return new UserDto
      {
        Id = query.Id,
        Name = "Aldo",
        Lastname = "Romo",
        Age = 29
      };

    }
  }
}