using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiProjectOne.Models;
using WebApiProjectOne.ViewModels;

namespace WebApiProjectOne.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration Configuration;

        public AccountController(UserManager<IdentityUser> UserManager,
                                  SignInManager<IdentityUser> SignInManager,
                                  IConfiguration configuration)
        {
            userManager = UserManager;
            signInManager = SignInManager;
            Configuration = configuration;
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserToken>> Login([FromBody]UserInfoViewModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false , lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return BuildToken(model);
            }

            ModelState.AddModelError(string.Empty, "Login fallido");

            return BadRequest(ModelState);
        }


        [HttpPost("registro")]
        public async Task<ActionResult<UserToken>> Register([FromBody] UserInfoViewModel model )
        {
            var User = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(User, model.Password);

            if (result.Succeeded)
            {
                return BuildToken(model);
            }
            else
            {
                return BadRequest("Registro fallido");
            }
        }


        private UserToken BuildToken(UserInfoViewModel model)
        {

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };

            //Construyendo la llave
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Duracion del Token
            var expiration = DateTime.UtcNow.AddHours(3);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }


    }
}
