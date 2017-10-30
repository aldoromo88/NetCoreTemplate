using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace NetCoreTemplate.Infrastructure.Middleware
{

  public class DefaultSinglePageFileMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly string _defaultSinglePageFile;
    private readonly IFileProvider _fileProvider;

    public DefaultSinglePageFileMiddleware(RequestDelegate next, IHostingEnvironment hostingEnv, string defaultSinglePageFile)
    {

      if (next == null)
      {
        throw new ArgumentNullException(nameof(next));
      }

      if (hostingEnv == null)
      {
        throw new ArgumentNullException(nameof(hostingEnv));
      }
      _next = next;
      _defaultSinglePageFile = defaultSinglePageFile;
      _fileProvider = ResolveFileProvider(hostingEnv);
    }


    public Task Invoke(HttpContext context)
    {
      if (IsGetOrHeadMethod(context.Request.Method))
      {
        var path = context.Request.Path.ToString();
        if (!path.Contains("."))
        {
          context.Request.Path = new PathString(context.Request.PathBase + _defaultSinglePageFile);
        }
      }
      return _next(context);
    }

    internal static IFileProvider ResolveFileProvider(IHostingEnvironment hostingEnv)
    {
      if (hostingEnv.WebRootFileProvider == null)
      {
        throw new InvalidOperationException("Missing FileProvider.");
      }
      return hostingEnv.WebRootFileProvider;
    }

    internal static bool IsGetOrHeadMethod(string method)
    {
      return HttpMethods.IsGet(method) || HttpMethods.IsHead(method);
    }
  }
}