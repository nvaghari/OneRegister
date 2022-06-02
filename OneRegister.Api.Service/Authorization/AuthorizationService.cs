using OneRegister.Api.Service.Abstract.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneRegister.Api.Service.Model;
using Microsoft.Extensions.Options;
using OneRegister.Data.Identication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using OneRegister.Api.Service.Exceptions;

namespace OneRegister.Api.Service.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly TokenOption _options;
        private readonly SignInManager<OUser> _signInManager;
        private readonly UserManager<OUser> _userManager;

        public AuthorizationService(IOptions<TokenOption> options, SignInManager<OUser> signInManager, UserManager<OUser> userManager)
        {
            _options = options.Value;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<string> GetTokenAsync(string userName, string userKey)
        {
            var signInResult = await  _signInManager.PasswordSignInAsync(userName, userKey, false, false);
            if (!signInResult.Succeeded)
            {
                throw new AuthorizationException("Authentication Failed.");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new();
            claims.Add(new Claim("user", userName));
            var token = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(_options.ExpiryInMinutes),
                signingCredentials: credential);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParams = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = _options.Audience,
                    ValidIssuer = _options.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey))
                };
                var principal = tokenHandler.ValidateToken(token, validationParams, out SecurityToken validatedToken);
                return  principal.Identities.FirstOrDefault()?.Claims.FirstOrDefault(c => c.Type == "user")?.Value;
            }
            catch (Exception ex)
            {

                throw new AuthorizationException(ex.Message);
            }

        }
        public async Task<bool> IsUserValid(string userName)
        {
            var users =await  _userManager.GetUsersInRoleAsync("MasterCard Administrator");
            if (users.Select(u => u.UserName).Contains(userName))
            {
                return true;
            }

            return false;
        }
    }
}
