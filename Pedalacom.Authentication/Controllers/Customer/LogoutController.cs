using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Pedalacom.Authentication.Controllers.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            
            await HttpContext.SignOutAsync();

           
            return Ok("Logout successful");
        }

    }
}
