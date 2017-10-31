using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace NetCoreTemplate.Infrastructure.Middleware.ApiHandler.Helpers
{

  public static class HttpContextExtensions
  {
    public static dynamic ToDynamic(this HttpContext context, JsonSerializerSettings settings = null)
    {
      var serializer = JsonSerializer.Create(settings);
      using (var sr = new StreamReader(context.Request.Body))
      {
        using (var jsonTextReader = new JsonTextReader(sr))
        {
          return (dynamic)serializer.Deserialize(jsonTextReader);
        }
      }
    }

    public static void ResponseWithErrorCode(this HttpContext context, int statusCode)
    {
      context.Response.Clear();
      context.Response.StatusCode = statusCode;
      context.Response.ContentType = "text/html; charset=utf-8";
      
    }

    public static void ResponseAsJson(this HttpContext context, object obj)
    {
      context.Response.Clear();
      context.Response.StatusCode = 200;
      context.Response.ContentType = "application/json; charset=utf-8";
      var serializer = JsonSerializer.Create();
      using (var sw = new StreamWriter(context.Response.Body))
      {
        using (var jsonTextWriter = new JsonTextWriter(sw))
        {
          serializer.Serialize(jsonTextWriter, obj);
        }
      }
      
    }
  }
}