using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NetCoreTemplate.Infrastructure.Middleware.ApiHandler.Contracts;

namespace NetCoreTemplate.Infrastructure.Middleware.ApiHandler
{
  public static class ApiHandlerExtension
  {
    public static IApplicationBuilder UseApiHandler(this IApplicationBuilder app)
    {
      if (app == null)
      {
        throw new ArgumentNullException(nameof(app));
      }

      app.MapWhen(ctx => IsPost(ctx.Request.Method), app2 =>
      {
        app2.Map("/api/command", appCommand => appCommand.UseMiddleware<ApiHandlerMiddleware>(typeof(ICommandHandler<>)));
        app2.Map("/api/query", appCommand => appCommand.UseMiddleware<ApiHandlerMiddleware>(typeof(IQueryHandler<>)));
      });

      return app;
    }

    internal static bool IsPost(string method)
    {
      return HttpMethods.IsPost(method);
    }

    public static void RegisterAllOpenImplementationsOf<T>(this IServiceCollection container)
    {
      container.RegisterAllOpenImplementationsOf(typeof(T));
    }

    public static void RegisterAllOpenImplementationsOf(this IServiceCollection container, Type type)
    {
      var types = type.GetTypesImplements();
      foreach (var t in types)
      {
        foreach (var i in t.GetInterfaces())
        {
          container.AddTransient(i, t);
        }
      }
    }

    private static IEnumerable<Type> GetTypesImplements(this Type type)
    {
      return Assembly.GetExecutingAssembly().GetTypes()
          .Where(c => c.IsClass && !c.IsAbstract && c.GetInterfaces().Any(i => i.Name == type.Name))
          .ToList();
    }

  }
}