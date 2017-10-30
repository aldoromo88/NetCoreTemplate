using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace NetCoreTemplate.Infrastructure.Middleware
{

  public static class DefaultSinglePageFile
  {

    public static IApplicationBuilder UseDefaultSinglePageFile(this IApplicationBuilder app, string defaultSinglePageFile = "index.html")
    {
      if (app == null)
      {
        throw new ArgumentNullException(nameof(app));
      }


      if (!string.IsNullOrEmpty(defaultSinglePageFile))
      {
        string siglePageFile = defaultSinglePageFile.StartsWith("/") ? defaultSinglePageFile : "/" + defaultSinglePageFile;
        return app.UseMiddleware<DefaultSinglePageFileMiddleware>(siglePageFile);
      }
      return app.UseMiddleware<DefaultSinglePageFileMiddleware>("/index.html");
    }
  }
}