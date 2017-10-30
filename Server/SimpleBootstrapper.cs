using NetCoreTemplate.Config;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Nancy.Authentication.Stateless;
using NetCoreTemplate.Infrastructure.Authentication.Contracts;

namespace NetCoreTemplate
{
    internal class SimpleBootstrapper : DefaultNancyBootstrapper
    {
        private AppConfiguration appConfig;

        public SimpleBootstrapper(AppConfiguration appConfig)
        {
            this.appConfig = appConfig;
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            container.Register<IAppConfiguration>(appConfig);
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            var identityProvider = container.Resolve<IIdentityProvider>();
            var statelessAuthConfig = new StatelessAuthenticationConfiguration(identityProvider.GetUserIdentity);
            StatelessAuthentication.Enable(pipelines, statelessAuthConfig);
            base.ApplicationStartup(container, pipelines);
        }
    }
}