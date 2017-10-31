using System.Collections.Generic;

namespace NetCoreTemplate.Infrastructure.Middleware.ApiHandler.Contracts
{
  public interface IQueryHandler<T>
  {
    object Handle (T query);
  }
}