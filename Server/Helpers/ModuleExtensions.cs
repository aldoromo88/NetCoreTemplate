using System;
using System.Threading;
using System.Threading.Tasks;
using Nancy;
using Nancy.Extensions;
using Nancy.Security;

namespace NetCoreTemplate.Helpers
{
  public static class ModuleExtensions
  {
    // public static void Post(this NancyModule module, string path, Func<dynamic, object> action, string requiresRole)
    // {
    //   module.Post(path, action, true, requiresRole);
    // }

    // public static void Post(this NancyModule module, string path, Func<dynamic, object> action, bool requiresAuthentication = false, string requiresRole = null)
    // {
    //   module.Post(path, action, requiresAuthentication, requiresRole);
    // }

    // public static void Post<TRequest>(this NancyModule module, string path, Func<TRequest, object> action, bool requiresAuthentication = false, string requiresRole = null) where TRequest : class
    // {
    //   module.Post(path, args =>
    //   {

    //     if (requiresAuthentication)
    //     {
    //       module.RequiresAuthentication();
    //     }

    //     if (!string.IsNullOrEmpty(requiresRole))
    //     {
    //       module.RequiresRole(requiresRole);
    //     }

    //     TRequest dto = module.Context.ToDto<TRequest>();
    //     return module.Response.AsJson(action(dto));
    //   });
    // }

    public static void RequiresRole(this NancyModule module, string role)
    {
      module.AddBeforeHookOrExecute(ctx =>
      {
        Response response = null;
        if ((ctx.CurrentUser == null) || !ctx.CurrentUser.IsInRole(role))
        {
          response = new Response { StatusCode = HttpStatusCode.Forbidden };
        }
        return response;
      }, $"Role {role} is required");
    }
  }
}