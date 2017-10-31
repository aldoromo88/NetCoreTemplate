using System.Collections.Generic;

namespace NetCoreTemplate.Infrastructure.Middleware.ApiHandler.Contracts
{
  public interface IApiHandler
  {
    bool AuthRequired { get; }
    List<string> RolesRequired { get; }
    RolesRequiredMode Mode { get; }
  }
}