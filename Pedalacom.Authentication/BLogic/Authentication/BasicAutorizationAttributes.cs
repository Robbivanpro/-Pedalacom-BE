using Microsoft.AspNetCore.Authorization;
using System.Security.Policy;

namespace Pedalacom.Authentication.BLogic.Authentication
{
    public class BasicAuthorizationAttributes : AuthorizeAttribute
    {
        public BasicAuthorizationAttributes()
        {
            Policy = "BasicAuthentication";
        }
    }
}
