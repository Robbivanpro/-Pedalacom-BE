﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pedalacom.Authentication.BLogic.Authentication;
using Pedalacom.Authentication.Models;

namespace Pedalacom.Authentication.Controllers.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly BasicAuthenticationHandler _basicAuthenticationHandler;

        public LoginController(BasicAuthenticationHandler basicAuthenticationHandler)
        {
            _basicAuthenticationHandler = basicAuthenticationHandler;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var authenticateResult = await _basicAuthenticationHandler.HandleAuthenticationAsyncWrapper();

            if (authenticateResult.Succeeded)
            {
                
                var authenticationUser = new AuthenticationUser(model.Username, "BasicAuthentication", true);
                return Ok(new { success = true, message = "Login successful", user = authenticationUser });
            }
            else
            {
                
                return BadRequest(new { success = false, message = "Invalid credentials" });
            }
        }
    }
}
