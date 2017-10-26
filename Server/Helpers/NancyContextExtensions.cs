using System.IO;
using Nancy;
using Newtonsoft.Json;

namespace NetCoreTemplate.Helpers
{
  public static class NancyContextExtensions
  {

    public static dynamic ToDynamic(this NancyContext context, JsonSerializerSettings settings = null)
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

    public static T ToDto<T>(this NancyContext context, JsonSerializerSettings settings = null) where T : class
    {
      var serializer = settings == null ? JsonSerializer.Create() : JsonSerializer.Create(settings);
      using (var sr = new StreamReader(context.Request.Body))
      {
        using (var jsonTextReader = new JsonTextReader(sr))
        {
          return serializer.Deserialize<T>(jsonTextReader);
        }
      }
    }
  }
}