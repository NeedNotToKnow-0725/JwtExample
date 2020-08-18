using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtExample.Configs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtExample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;
        private readonly JwtOption jwtOption;
        private readonly SigningCredentials signingCredentials;

        public UserController(IOptions<JwtOption> options)
        {
            jwtOption = options.Value;
            jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.SecretKey));
            signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        }

        [HttpGet("RequestToken")]
        public JsonResult RequestToken()
        {
            DateTime utcNow = DateTime.UtcNow;

            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString())
            };

            DateTime expiredDateTime = utcNow.AddMinutes(jwtOption.ExpiryMinutes);
            string token = jwtSecurityTokenHandler.WriteToken(new JwtSecurityToken(jwtOption.ValidIssuer, jwtOption.ValidAudience, claims, utcNow, expiredDateTime, signingCredentials));

            return new JsonResult(new { token });
        }

        [Authorize]
        [HttpGet("GetData")]
        public JsonResult GetData()
        {
            return new JsonResult(new { data = "Success" });
        }
    }
}
