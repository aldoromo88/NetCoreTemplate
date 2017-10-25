using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Nancy.Owin;
using NetCoreTemplate.Config;

namespace NetCoreTemplate
{
  public class Startup
    {
        private readonly IConfiguration config;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(env.ContentRootPath)
                            .AddJsonFile("appsettings.json");
            config = builder.Build();
        }

        public void Configure(IApplicationBuilder app)
        {
            var appConfig = new AppConfiguration();
            ConfigurationBinder.Bind(config, appConfig);

            app.UseOwin(x => x.UseNancy(opt => opt.Bootstrapper = new SimpleBootstrapper(appConfig)));
        }
    }
}
