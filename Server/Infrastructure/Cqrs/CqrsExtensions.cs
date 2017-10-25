using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Nancy;
using Nancy.Configuration;
using Nancy.TinyIoc;
using NetCoreTemplate.Helpers;
using Newtonsoft.Json;

namespace NetCoreTemplate.Infrastructure.Cqrs
{

    public static class CqrsExtensions
    {
        public static Response Handle(this NancyContext context, TinyIoCContainer container, Type handler)
        {
            var request = context.ToDynamic(GetSerializerSettings());
            Type requestType = request.GetType();
            Type handleType = GetTypeImplements(handler.MakeGenericType(requestType));
            dynamic handlerInstance = container.Resolve(handleType);
            dynamic result = handlerInstance.Handle(request);

            return new CqrsResponse(result, container.Resolve<INancyEnvironment>());
        }

        private static JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            };
        }

        private static Type GetTypeImplements(Type type)
        {
            return Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(c => type.IsAssignableFrom(c) && c.IsClass && !c.IsAbstract && !c.IsGenericParameter);

            // return AppDomain.CurrentDomain.GetAssemblies()
            //                 .Where(x => x.FullName.StartsWith(_startNameToSearch))
            //                 .SelectMany(s => s.GetTypes())
            //                 .FirstOrDefault(c => typeof(T).IsAssignableFrom(c) && c.IsClass && !c.IsAbstract && !c.IsGenericParameter);
        }
    }
}