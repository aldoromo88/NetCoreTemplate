using System;
using Nancy;
using Nancy.Security;
using NetCoreTemplate.Api;
using NetCoreTemplate.Helpers;

namespace NetCoreTemplate.Infrastructure.ModuleRouting
{
  public abstract class ApiModule : NancyModule
  {
    public ApiModule(string path) : base("/api/" + path)
    {

    }
    public void Post(string path, Func<dynamic, object> action, bool requiresAuthentication = false, string requiresRole = null)
    {
      base.Post(path, args =>
      {

        if (requiresAuthentication)
        {
          this.RequiresAuthentication();
        }

        if (!string.IsNullOrEmpty(requiresRole))
        {
          this.RequiresRole(requiresRole);
        }

        dynamic dto = this.Context.ToDynamic();
        object response = action(dto);
        return this.Response.AsJson(response);
      });
    }

    public void Post<TRequest>(string path, Func<TRequest, object> action, bool requiresAuthentication = false, string requiresRole = null) where TRequest : class
    {
      base.Post(path, args =>
      {

        if (requiresAuthentication)
        {
          this.RequiresAuthentication();
        }

        if (!string.IsNullOrEmpty(requiresRole))
        {
          this.RequiresRole(requiresRole);
        }

        TRequest dto = this.Context.ToDto<TRequest>();
        return this.Response.AsJson(action(dto));
      });
    }

    #region Post Methods for syntax sugar 
    public void Post(Func<dynamic, object> action, string requiresRole)
    {
      Post("/" + action.Method.Name, action, true, requiresRole);
    }

    public void Post(Func<dynamic, object> action, bool requiresAuthentication = false, string requiresRole = null)
    {
      Post("/" + action.Method.Name, action, requiresAuthentication, requiresRole);
    }

    public void Post<TRequest>(Func<TRequest, object> action, string requiresRole) where TRequest : class
    {
      Post<TRequest>("/" + action.Method.Name, action, true, requiresRole);
    }

    public void Post<TRequest>(Func<TRequest, object> action, bool requiresAuthentication = false, string requiresRole = null) where TRequest : class
    {
      Post<TRequest>("/" + action.Method.Name, action, requiresAuthentication, requiresRole);
    }

    public void Post(string path, Func<dynamic, object> action, string requiresRole)
    {
      Post(path, action, true, requiresRole);
    }

    public void Post<TRequest>(string path, Func<TRequest, object> action, string requiresRole) where TRequest : class
    {
      Post<TRequest>(path, action, true, requiresRole);
    }
    #endregion
  }
}