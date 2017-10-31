using System;
using System.Collections.Generic;

namespace NetCoreTemplate.Infrastructure.Middleware.ApiHandler.Contracts
{
  public abstract class ApiBaseHandler:IApiHandler
  {

    public bool AuthRequired { get; private set; }
    public List<string> RolesRequired { get; private set; }
    public RolesRequiredMode Mode { get; private set; }

    protected ApiBaseHandler(string roleRequired):this(true, new List<string>{roleRequired})
    {
    }

    protected ApiBaseHandler(bool authRequired = false, List<string> rolesRequired = null, RolesRequiredMode mode = RolesRequiredMode.Any)
    {
      RolesRequired = rolesRequired ?? new List<string>();
      AuthRequired = authRequired || RolesRequired.Count > 0;
      Mode = mode;
    }
  }
}