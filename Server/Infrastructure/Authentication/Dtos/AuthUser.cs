using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace NetCoreTemplate.Infrastructure.Authentication.Contracts
{
    internal sealed class AuthUser : IIdentity
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string UserLogin { get; set; }

        public string AuthenticationType => "Customer";
        public bool IsAuthenticated => true;

        public AuthUser(Guid userId, string userName, string userLogin)
        {
            UserId = userId;
            Name = userName;
            UserLogin = userLogin;
        }
    }

    internal sealed class AuthPrincipal : IPrincipal
    {

        private List<string> Roles { get; set; }

        public IIdentity Identity { get; private set; }


        public bool IsInRole(string role)
        {
            return Roles.Contains(role);
        }

        public AuthPrincipal(Guid userId, string userName, string userLogin, List<string> roles)
        {
            Identity = new AuthUser(userId, userName, userLogin);
            Roles = roles;
        }
    }
}