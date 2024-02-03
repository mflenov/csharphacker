using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiTest.Model;

namespace ApiTest.Controllers
{
    [AllowAnonymous]
    public class AuthController: ControllerBase
	{
        private readonly IJWTAuthenticationManager jwtAuthenticationManager;

        public AuthController(IJWTAuthenticationManager jwtAuthenticationManager)
		{
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserCredentials userCred)
        {
            var token = jwtAuthenticationManager.Authenticate(userCred.Email, userCred.Password);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }
    }
}

