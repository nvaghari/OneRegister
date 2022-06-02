using Microsoft.AspNetCore.Identity;
using OneRegister.Data.Identication;
using OneRegister.Data.SuperEntities;
using OneRegister.Domain.Model.Account;
using OneRegister.Core.Model.ControllerResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using OneRegister.Domain.Model.Configuration;
using System.Security.Claims;
using OneRegister.Security.Exceptions;
using Microsoft.Extensions.Logging;
using OneRegister.Security.Services.HttpContext;

namespace OneRegister.Domain.Services.Account
{
    public class AuthorizationService
    {
        private readonly RoleService _roleService;
        private readonly SignInManager<OUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthorizationService> _logger;
        private readonly IHttpContextPermissionHandler _permissionHandler;
        private readonly UserService _userService;
        public AuthorizationService(
            UserService userService,
            RoleService roleService,
            SignInManager<OUser> signInManager,
            IConfiguration configuration,
            ILogger<AuthorizationService> logger,
            IHttpContextPermissionHandler permissionHandler)
        {
            _userService = userService;
            _roleService = roleService;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
            _permissionHandler = permissionHandler;
        }

        private JWTConfig JWTConfig => _configuration.GetSection("Services:ExportPhoto").Get<JWTConfig>();
        public string GetToken(string userName)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTConfig.SecretKey));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new();
            claims.Add(new Claim("user", userName));
            var token = new JwtSecurityToken(
                JWTConfig.Issuer,
                JWTConfig.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(JWTConfig.ExpiryInMinutes),
                signingCredentials: credential);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParams = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = JWTConfig.Audience,
                ValidIssuer = JWTConfig.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTConfig.SecretKey))
            };

            var principal = tokenHandler.ValidateToken(token, validationParams, out SecurityToken validatedToken);
            var userClaim = principal.Identities.FirstOrDefault()?.Claims.FirstOrDefault(c => c.Type == "user");

            if (userClaim is null)
            {
                throw new ApplicationException("User does not exist in token");
            }

            return userClaim.Value;

        }
        public OUser GetCurrentUser()
        {
            var userId = _permissionHandler.UserId();

            return userId.HasValue ? _userService.GetById(userId.Value) : null;
        }

        public Guid? GetCurrentUserId()
        {
            return _permissionHandler.UserId();
        }

        public IEnumerable<string> GetUserRoles(OUser user)
        {
            return _userService.GetUserRoles(user);
        }

        public IEnumerable<OUser> GetUsersInRole(string name)
        {
            return _userService.GetUsersInRole(name);
        }

        public bool HasUserRole(string roleName)
        {
            return _userService.IsInRole(GetCurrentUser(), roleName);
        }

        public bool IsCurrentUserInRoles(string roles)
        {
            var userRoles = _userService.GetUserRoles(GetCurrentUser());
            var wantedRoles = roles.Split(',');
            return userRoles.Intersect(wantedRoles).Any();
        }

        public async Task<SimpleResponse> Login(LoginModel loginModel)
        {
            try
            {
                if (string.IsNullOrEmpty(loginModel.UserName?.Trim()))
                {
                    throw new AuthenticationException("How do we process your authentication without UserName?!");
                }

                var user = _userService.GetByUserNameNoLimit(loginModel.UserName, includes: new string[] { nameof(Organization) });
                if (user == null)
                {
                    throw new AuthenticationException($"the User: {loginModel.UserName} doesn't exist");
                }


                var checkPassword = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
                if (!checkPassword.Succeeded)
                {
                    throw new AuthenticationException($"the password for this user: {loginModel.UserName}  is not match");
                }
                List<Claim> claims = new();
                claims.Add(new Claim(ClaimTypes.UserData, user.Organization?.Path));
                await _signInManager.SignInWithClaimsAsync(user, false, claims);
                _logger.LogInformation($"Login Success->User: {loginModel.UserName}");
                return SimpleResponse.Success();
            }
            catch (AuthenticationException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return SimpleResponse.FailBecause(ex.FriendlyMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return SimpleResponse.FailBecause(ex.Message);
            }
        }

        public async Task<SimpleResponse> ExportLoginAsync(string user, string password)
        {
            if (string.IsNullOrEmpty(user)) return SimpleResponse.FailBecause("User is not provided");

            var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (signInResult.Succeeded)
            {
                bool hasUserAccessToLogin = _userService.HasUserAccessToLogin(user);
                if (hasUserAccessToLogin)
                {
                    return SimpleResponse.Success();
                }
                else
                {
                    return SimpleResponse.FailBecause("User is inactive. You are not allowed to login.");
                }
            }

            return SimpleResponse.FailBecause("Authentication Failed");
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
