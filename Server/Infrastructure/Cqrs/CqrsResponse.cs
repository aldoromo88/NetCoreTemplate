using System;
using System.IO;
using Nancy;
using Nancy.Configuration;
using Nancy.Json;
using Newtonsoft.Json;

namespace NetCoreTemplate.Infrastructure.Cqrs
{
    public class CqrsResponse : Response
    {
        private readonly JsonConfiguration _configuration;

        public CqrsResponse(dynamic result, INancyEnvironment environment)
        {
            _configuration = environment.GetValue<JsonConfiguration>();
            Contents = result == null ? NoBody : GetJsonContents(result);
            ContentType = DefaultContentType;
            StatusCode = HttpStatusCode.OK;
        }

        private string DefaultContentType
        {
            get { return string.Concat("application/json", Encoding); }
        }

        private string Encoding
        {
            get { return string.Concat("; charset=", _configuration.DefaultEncoding.WebName); }
        }

        private Action<Stream> GetJsonContents(dynamic result)
        {
            return stream =>
            {
                var serializer = JsonSerializer.Create();

                using (var sw = new StreamWriter(stream))
                using (var jsonTextWriter = new JsonTextWriter(sw))
                {
                    serializer.Serialize(jsonTextWriter, result);
                }

            };
        }
    }
}