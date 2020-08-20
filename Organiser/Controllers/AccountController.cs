using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Database.Entities;
using Database.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Organiser.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IUnitOfWork uow;
        private readonly IRepository<User> userRepository;

        public AccountController(IUnitOfWork uow, IRepository<User> userRepository)
        {
            this.uow = uow;
            this.userRepository = userRepository;
        }

        [HttpPost("token")]
        public IActionResult Token([FromForm] User model)
        {
            var identity = GetIdentity(model.Username, model.PasswordHash);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." + model.Username + model.PasswordHash});
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Json(response);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User user = userRepository.Get().FirstOrDefault(x => x.Username == username && x.PasswordHash == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("ID",user.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}