using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using RestApi.Utilities;
using System;
using System.Threading.Tasks;

namespace RestApi.Filters
{
    public class IsOwnerRequirement : IAuthorizationRequirement
    {
        public IsOwnerRequirement() {
            
        }
    }
}
