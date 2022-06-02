using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneRegister.Domain.Model.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Test.CaseTest
{
    [TestClass]
    public class JwtTokenTest
    {
        private JWTConfig jwtConfig => new()
        {
            SecretKey = "fSoB36TtPx9sgiXO3r0mU5xESyY4IejO",
            Issuer = "OneRegister",
            Audience = "ExportApp",
            ExpiryInMinutes = 20
        };
        public string GetToken(string user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new() { new Claim("User", user) };
            var token = new JwtSecurityToken(
                jwtConfig.Issuer,
                jwtConfig.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credential);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParams = new TokenValidationParameters {
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = jwtConfig.Audience,
                ValidIssuer = jwtConfig.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey))
            };
            try
            {
                var claims = tokenHandler.ValidateToken(token,validationParams, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [TestMethod]
        public void Authorize()
        {
            var token = GetToken("Nader");

            Assert.IsTrue(ValidateToken(token));
        }
        [TestMethod]
        public void AuthorizeExpire()
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2MzIyMjIxMzQsImlzcyI6Ik9uZVJlZ2lzdGVyIiwiYXVkIjoiRXhwb3J0QXBwIn0._yAgLboxsvLTI-gzheaMugVdBZ-lDen1dd9d0T5eE6k";

            Assert.IsFalse(ValidateToken(token));
        }
    }
}
