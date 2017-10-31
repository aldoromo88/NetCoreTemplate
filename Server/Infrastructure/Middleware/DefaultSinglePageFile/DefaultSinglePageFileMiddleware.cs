using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace NetCoreTemplate.Infrastructure.Middleware.DefaultSinglePageFile
{

  public class DefaultSinglePageFileMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly string _defaultSinglePageFile;

    public DefaultSinglePageFileMiddleware(RequestDelegate next, string defaultSinglePageFile)
    {

      if (next == null)
      {
        throw new ArgumentNullException(nameof(next));
      }
      _next = next;
      _defaultSinglePageFile = defaultSinglePageFile;

    }

    public async Task InvokeAsync(HttpContext context)
    {
      if (IsGetOrHeadMethod(context.Request.Method))
      {
        var path = context.Request.Path.ToString();
        if (!path.Contains("."))
        {
          context.Request.Path = new PathString(context.Request.PathBase + _defaultSinglePageFile);

        }
      }
      await _next(context);
    }

    internal static bool IsGetOrHeadMethod(string method)
    {
      return HttpMethods.IsGet(method) || HttpMethods.IsHead(method);
    }
  }
}