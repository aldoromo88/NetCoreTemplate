using System.Collections.Generic;

namespace NetCoreTemplate.Infrastructure.Middleware.ApiHandler.Contracts
{
  public interface ICommandHandler<T> 
  {
    object Handle(T command);
  }
}