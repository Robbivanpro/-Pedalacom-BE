using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Text;
using PedalacomOfficial.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using library.Security;

namespace Pedalacom.Authentication.BLogic.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly AdventureWorksLt2019Context _dbContext;
        private readonly Cryptography.CredenzialiSale _cryptoLibrary;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            AdventureWorksLt2019Context dbContext,
            Cryptography.CredenzialiSale cryptoLibrary) : base(options, logger, encoder, clock)
        {
            _dbContext = dbContext;
            _cryptoLibrary = cryptoLibrary;
        }

        public async Task<AuthenticateResult> HandleAuthenticationAsyncWrapper()
        {
            return await HandleAuthenticateAsync();
        }


        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Response.Headers.Add("WWW-Authenticate", "Basic");

            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Autorizzazione mancante: Impossibile accedere al servizio");
            }

            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var authorizationRegEx = new Regex(@"Basic (.*)");

            if (!authorizationRegEx.IsMatch(authorizationHeader))
            {
                return AuthenticateResult.Fail("Autorizzazione non valida: Impossibile accedere al servizio");
            }

            var authorizationBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(authorizationRegEx.Replace(authorizationHeader, "$1")));
            var authorizationSplit = authorizationBase64.Split(':', 2);

            if (authorizationSplit.Length != 2)
            {
                return AuthenticateResult.Fail("Autorizzazione non valida: Impossibile accedere al servizio");
            }

            var username = authorizationSplit[0];
            var password = authorizationSplit[1];

            var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.EmailAddress == username);
            //var admin = await _dbContext.Admins.FirstOrDefaultAsync(a => a.Username == username);

            if ((customer == null || !Cryptography.DecryptSaltCredential(_dbContext, username, password)) //&&
               // (admin == null || !Cryptography.DecryptSaltCredential(_dbContext, username, password))
                )
            {
                return AuthenticateResult.Fail("Nome utente o password non validi: Impossibile accedere al servizio");
            }

            var isAuthenticated = customer != null; //|| admin != null;
            var authenticationUser = new AuthenticationUser(username, "BasicAuthentication", isAuthenticated);
            var claims = new ClaimsPrincipal(new ClaimsIdentity(authenticationUser));

            return AuthenticateResult.Success(new AuthenticationTicket(claims, "BasicAuthentication"));
        }
        
    }
}
