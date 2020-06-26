using JWT.Domain;
using JWT.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWT.Controllers
{
    /// <summary>
    /// CriarTokenController
    /// </summary>
    
    [Authorize]
    public class CriarTokenController : ControllerBase
    {
        private readonly AppSettings appSettings;

        /// <summary>
        /// CriarTokenController
        /// </summary>
        /// <param name="appSettings"></param>
        public CriarTokenController(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        /// <summary>
        /// SerializeObject
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented);
        }

        /// <summary>
        /// CreateToken
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        private string CreateToken(string Username)
        {
            var user = new Usuario
            {
                Username = Username
            };

            var subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, SerializeObject(user)) });
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.ApiKey)), SecurityAlgorithms.HmacSha512Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        /// <summary>
        /// GetToken
        /// </summary>
        /// <returns></returns>
        [HttpGet("CriarToken/{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetToken(string username)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

            return Ok(await Task.Factory.StartNew(() =>
            {
                return CreateToken(username);
            }));                
        }
    }
}