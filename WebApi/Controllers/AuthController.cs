using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult Authenticate([FromBody]Credential credential)
        {
            //Verify the credential
            if (credential.UserName=="admin" && credential.Password=="password")
            {
                //Creating the security context
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@mywww.com"),
                    new Claim("Department", "HR"),
                    new Claim("Admin", "true"),
                    new Claim("Manager", "true"),
                    new Claim("EmploymentDate", "2022-01-01")
                };

                var expiresAt=DateTime.UtcNow.AddMinutes(30);

                return Ok(new
                {
                    access_token = "",
                    expires_at = expiresAt
                });
            }

            ModelState.AddModelError("Unauthorized!", "You are not authorized to access the endpoint!");
            return Unauthorized(ModelState);
        }


    }


    public class Credential
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
