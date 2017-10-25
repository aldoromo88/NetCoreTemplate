using System;
using System.Collections.Generic;

namespace NetCoreTemplate.Infrastructure.Authentication.Contracts
{
    internal sealed class AuthToken
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserLogin { get; set; }
        public List<string> Roles { get; set; }
        public DateTime ExpirationDateTime { get; set; }
    }
}