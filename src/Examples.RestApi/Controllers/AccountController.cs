using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Examples.RestApi.Auth;
using Examples.RestApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Examples.RestApi.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
    {
        private readonly List<User> _people = new List<User>
        {
            new User { Login="admin@gmail.com", Password="12345", Role = "admin" },
            new User { Login="qwerty@gmail.com", Password="55555", Role = "user" }
        };
 
        [HttpPost("/token")]
        public IActionResult Token(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }
 
            var now = DateTime.UtcNow;
            
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(encodedJwt);
        }
 
        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User person = _people.FirstOrDefault(x => x.Login == username && x.Password == password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }
    }