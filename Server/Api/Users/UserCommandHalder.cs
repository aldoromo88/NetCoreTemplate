using NetCoreTemplate.Infrastructure.Cqrs.Contracts;
using NetCoreTemplate.Api.Users.Dtos.Commands;

namespace NetCoreTemplate.Api.Users
{
    public class UserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        public object Handle(CreateUserCommand command)
        {
            return new {
                HasError = false,
                Message = $"User {command.NewUser.Name} created successfully"
            };
        }
    }
}