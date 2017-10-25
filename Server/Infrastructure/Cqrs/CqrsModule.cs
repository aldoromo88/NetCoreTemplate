using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Security;
using Nancy.TinyIoc;
using NetCoreTemplate.Helpers;
using NetCoreTemplate.Infrastructure.Authentication.Contracts;
using NetCoreTemplate.Infrastructure.Cqrs.Contracts;

namespace NetCoreTemplate.Infrastructure.Cqrs
{
    public class CqrsModule : NancyModule
    {
        public CqrsModule(TinyIoCContainer container, IIdentityProvider identityProvider)
        {
            var statelessAuthConfig = new StatelessAuthenticationConfiguration(identityProvider.GetUserIdentity);
            StatelessAuthentication.Enable(this, statelessAuthConfig);

            this.RequiresAuthentication();

            Post("/api/command", args => Context.Handle(container, typeof(ICommandHandler<>)));
            Post("/api/query", args => Context.Handle(container, typeof(IQueryHandler<>)));
        }
    }
}