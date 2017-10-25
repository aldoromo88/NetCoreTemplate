using System;
using System.Threading;
using System.Threading.Tasks;
using Nancy;

namespace NetCoreTemplate.Helpers
{
  public static class ModuleExtensions
  {

    /// <summary>
    /// Declares a route for POST requests.
    /// </summary>
    /// <typeparam name="T">The return type of the <paramref name="action"/></typeparam>
    /// <param name="path">The path that the route will respond to</param>
    /// <param name="action">Action that will be invoked when the route it hit</param>
    /// <param name="name">Name of the route</param>
    /// <param name="condition">A condition to determine if the route can be hit</param>
    public static void Post<TRequest, TResult>(this NancyModule module, string path, Func<TRequest, TResult> action, Func<NancyContext, bool> condition = null, string name = null) where TRequest : class
    {
      module.Post(path, args => {
          TRequest dto = module.Context.ToDto<TRequest>();
          return module.Response.AsJson(action(dto));
      }, condition, name);
    }


    /// <summary>
    /// Declares a route for POST requests.
    /// </summary>
    /// <typeparam name="T">The return type of the <paramref name="action"/></typeparam>
    /// <param name="path">The path that the route will respond to</param>
    /// <param name="action">Action that will be invoked when the route it hit</param>
    /// <param name="name">Name of the route</param>
    /// <param name="condition">A condition to determine if the route can be hit</param>
    // public static void Post<TRequest, TResult>(this NancyModule module, string path, Func<TRequest, CancellationToken, Task<TResult>> action, Func<NancyContext, bool> condition = null, string name = null) where TRequest : class
    // {

    //   TRequest dto = module.Context.ToDto<TRequest>();
    //   Func<dynamic, CancellationToken, Task<TResult>> wrapper = (Func<dynamic, CancellationToken, Task<TResult>>)action;
    //   module.Post(path, wrapper, condition, name);
    // }

  }
}