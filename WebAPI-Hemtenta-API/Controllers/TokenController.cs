using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API_Web_API_Kurs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public TokenController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        // POST /api/token
        [HttpPost]
        public async Task<ActionResult> CreateAsync(Credentials credentials)
        {
            var user = await _userManager.FindByNameAsync(credentials.UserName);

            var hasAccess = await _userManager.CheckPasswordAsync(user, credentials.Password);


            if (!hasAccess)
            {
                return Unauthorized(); // 401 Unauthorized
            }

            var token = GenerateToken(user);

            return Ok( new{ token });
        }

        private string GenerateToken(IdentityUser user)
        {
            var signingKey = Convert.FromBase64String(_configuration["Token:SigningKey"]);

            var expirationInMinutes = int.Parse(_configuration["Token:ExpirationInMinutes"]);


            bool isAdmin = _userManager.IsInRoleAsync(user, "Administrator").Result;
            SecurityTokenDescriptor tokenDescriptor;

            if (isAdmin)
            {

                 tokenDescriptor = new SecurityTokenDescriptor
                {
                    // (iat) Issued At Time
                    IssuedAt = DateTime.UtcNow,

                    // (exp) Expiration Time
                    Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes),



                    Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("userid", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("hasRole", "admin"), // user.GetRoles();
                }),



                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(signingKey),
                        SecurityAlgorithms.HmacSha256Signature)
                };

            }
            else
            {
                 tokenDescriptor = new SecurityTokenDescriptor
                {
                    // (iat) Issued At Time
                    IssuedAt = DateTime.UtcNow,

                    // (exp) Expiration Time
                    Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes),



                    Subject = new ClaimsIdentity(new List<Claim>
                    {
                        new Claim("userid", user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim("hasRole", "User"), // user.GetRoles();

                    }),



                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(signingKey),
                        SecurityAlgorithms.HmacSha256Signature)
                };

            }

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            var token = jwtTokenHandler.WriteToken(jwtSecurityToken);

            return token;
        }
    }




    public class Credentials
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
