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
using Newtonsoft.Json;
using Organiser.Managers;
using Organiser.Models;

namespace Organiser.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IUnitOfWork uow;
        private readonly IRepository<User> userRepository;
        private readonly UserManager userManager;
        private readonly EmailManager emailManager;

        public AccountController(IUnitOfWork uow, IRepository<User> userRepository, UserManager userManager, EmailManager emailManager)
        {
            this.uow = uow;
            this.userRepository = userRepository;
            this.userManager = userManager;
            this.emailManager = emailManager;
        }

        [HttpPost("register")]
        public void Register([FromForm] UserRegisterViewModel model)
        {
            var uri = new UriBuilder(
                Request.Scheme,
                Request.Host.Host,
                Request.Host.Port ?? -1,
                "emailConfirmation");
            userManager.Register(uri, model);
        }

        [HttpGet("confirmRegistration")]
        public void ConfirmRegistration(string username, string token)
        {
            if (token == null)
            {
                throw new Exception("token is null");
            }

            var user = userRepository.Get(x => x.Username == username).FirstOrDefault();
            if (user == null)
            {
                throw new Exception("Invalid username");
            }

            if (user.IsConfirmed)
            {
                throw new Exception("User email is already registered");
            }

            var confirmation = userManager.IsOriginToken(user.Email, token);
            if (confirmation)
            {
                user.IsConfirmed = true;
                userRepository.Update(user);
                uow.Commit();
            }
        }

        [HttpPost("token")]
        public async Task Token([FromForm] UserLoginViewModel model)
        {
            var user = userManager.CheckPassword(model.Login, model.Password);
            var identity = GetIdentity(user);
            if (user == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password.");
                return;
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
                user = new User { Username = model.Login, PasswordHash = model.Password }
            };

            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        private ClaimsIdentity GetIdentity(User user)
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
    }
}