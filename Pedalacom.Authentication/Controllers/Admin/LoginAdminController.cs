using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pedalacom.Authentication.BLogic.Authentication;
using Pedalacom.Authentication.Models;

namespace Pedalacom.Authentication.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginAdminController : ControllerBase
    {
        private readonly BasicAuthenticationHandler _basicAuthenticationHandler;

        public LoginAdminController(BasicAuthenticationHandler basicAuthenticationHandler)
        {
            _basicAuthenticationHandler = basicAuthenticationHandler;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var adminAuthenticateResult = await _basicAuthenticationHandler.HandleAuthenticationAsyncWrapper();

            if (adminAuthenticateResult.Succeeded)
            {
               
                var authenticationUser = new AuthenticationUser(model.Username, "AdminAuthentication", true);
                return Ok(new { success = true, message = "Admin login successful", user = authenticationUser });
            }
            else
            {
                
                return BadRequest(new { success = false, message = "Invalid admin credentials" });
            }
        }

    }
}
