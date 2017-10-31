using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NetCoreTemplate.Infrastructure.Authentication.Contracts;
using NetCoreTemplate.Infrastructure.Middleware.ApiHandler.Contracts;
using NetCoreTemplate.Infrastructure.Middleware.ApiHandler.Helpers;
using Newtonsoft.Json;

namespace NetCoreTemplate.Infrastructure.Middleware.ApiHandler
{
  public class ApiHandlerMiddleware
  {
    private readonly Type _handlerType;
    private readonly JsonSerializerSettings _serializerSettings;
    private readonly Dictionary<Type, Type> _resolutionsCache;

    public ApiHandlerMiddleware(RequestDelegate _, Type handlerType)
    {
      _handlerType = handlerType;
      _resolutionsCache = new Dictionary<Type, Type>();
      _serializerSettings = new JsonSerializerSettings
      {
        TypeNameHandling = TypeNameHandling.All,
        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
      };
    }


    public Task InvokeAsync(HttpContext context, IServiceProvider container, IIdentityProvider identityProvider)
    {
      var request = context.ToDynamic(_serializerSettings);
      Type requestType = request.GetType();
      //Type concreteHandler = GetTypeImplements(_handlerType.MakeGenericType(requestType));
      //dynamic handlerInstance = container.GetService(concreteHandler);

      dynamic handlerInstance = container.GetService(_handlerType.MakeGenericType(requestType));

      IApiHandler apiHandler = handlerInstance as IApiHandler;


      ClaimsPrincipal identity = null;
      if (apiHandler.AuthRequired)
      {
        identity = identityProvider.GetUserIdentity(context);

        if (identity == null)
        {
          context.ResponseWithErrorCode(401);
          return Task.CompletedTask;
        }
      }

      if (apiHandler.RolesRequired.Count > 0)
      {
        bool hasPermision = false;
        switch (apiHandler.Mode)
        {
          case RolesRequiredMode.Any:
            hasPermision = apiHandler.RolesRequired.Any(r => identity.IsInRole(r));
            break;
          case RolesRequiredMode.All:
            hasPermision = apiHandler.RolesRequired.All(r => identity.IsInRole(r));
            break;
        }

        if (!hasPermision)
        {
          context.ResponseWithErrorCode(403);
          return Task.CompletedTask;
        }
      }


      object result = handlerInstance.Handle(request);
      context.ResponseAsJson(result);
      return Task.CompletedTask;
    }

    // private Type GetTypeImplements(Type type)
    // {
    //   if (_resolutionsCache.ContainsKey(type))
    //     return _resolutionsCache[type];

    //   var handler = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(c => type.IsAssignableFrom(c) && c.IsClass && !c.IsAbstract && !c.IsGenericParameter);
    //   _resolutionsCache.Add(type, handler);
    //   return handler;
    // }

  }
}