using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreTemplate.Config;
using NetCoreTemplate.Infrastructure.Authentication;
using NetCoreTemplate.Infrastructure.Authentication.Contracts;
using NetCoreTemplate.Infrastructure.Middleware.ApiHandler;
using NetCoreTemplate.Infrastructure.Middleware.ApiHandler.Contracts;
using NetCoreTemplate.Infrastructure.Middleware.DefaultSinglePageFile;

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

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddTransient<IIdentityProvider, IdentityProvider>();

      var appConfig = new AppConfiguration();
      ConfigurationBinder.Bind(config, appConfig);
      services.AddSingleton<IAppConfiguration>(appConfig);
      services.RegisterAllOpenImplementationsOf<IApiHandler>();
    }


    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {


      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
        {
          HotModuleReplacement = true
        });
      }

      app.UseDefaultSinglePageFile();
      app.UseStaticFiles();
      app.UseApiHandler();
      app.UseOwin();
    }
  }
}
