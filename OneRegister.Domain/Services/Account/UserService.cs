using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OneRegister.Core.Model.ControllerResponse;
using OneRegister.Data.Contract;
using OneRegister.Data.Identication;
using OneRegister.Data.SuperEntities;
using OneRegister.Domain.Extentions;
using OneRegister.Domain.Model.Account;
using OneRegister.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Domain.Services.Account
{
    public class UserService
    {
        private readonly UserManager<OUser> _userManager;
        private readonly IOrganizedRepository<OUser> _userRepository;
        public UserService(
            IOrganizedRepository<OUser> userRepository,
            UserManager<OUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }
        public Guid Add(OUser ouser, string password)
        {
            var result = _userManager.CreateAsync(ouser, password).Result;
            if (!result.Succeeded)
            {
                throw new ApplicationException(result.Errors.Select(e => e.Description).Aggregate((a, b) => a + " " + b));
            }
            return ouser.Id;
        }

        public void AddToRole(OUser user, string roleName)
        {
            user.SecurityStamp = Guid.NewGuid().ToString();
            var result = _userManager.AddToRoleAsync(user, roleName).Result;
            if (!result.Succeeded) throw new ApplicationException(result.Errors.Select(e => e.Description).Aggregate((a, b) => a + " " + b));
        }

        public void AddToRole(Guid userId, string roleName)
        {
            var user = _userRepository.GetByIdAsAdmin(userId);
            var result = _userManager.AddToRoleAsync(user, roleName).Result;
            if (!result.Succeeded) throw new ApplicationException(result.Errors.Select(e => e.Description).Aggregate((a, b) => a + " " + b));
        }

        public bool Any(Guid id)
        {
            return _userRepository.AnyByIdAsAdmin(id);
        }

        public SimpleResponse ChangePassword(OUser currentUser, string oldPassword, string newPassword)
        {
            var result = _userManager.ChangePasswordAsync(currentUser, oldPassword, newPassword).Result;
            if (result.Succeeded) return SimpleResponse.Success();
            return SimpleResponse.FailBecause(result.Errors.Select(e => e.Description).Aggregate((a, b) => a + " " + b));
        }

        public void Create(OUser ouser, string password, Guid? creatorId = null)
        {
            ouser.CreatedBy = ouser.ModifiedBy = creatorId;
            var result = _userManager.CreateAsync(ouser, password).Result;
            if (!result.Succeeded)
            {
                throw new ApplicationException(result.Errors.Select(e => e.Description).Aggregate((a, b) => a + " " + b));
            }
        }

        public FullResponse Create(UserRegisterModel model, ClaimsPrincipal principal)
        {
            try
            {
                if (string.CompareOrdinal(model.Password, model.PasswordConfirm) != 0)
                {
                    throw new ApplicationException("Password doesn't match");
                }
                var creatorId = new Guid(_userManager.GetUserId(principal));
                var ouser = new OUser
                {
                    OrganizationId = model.OrganizationId,
                    Email = model.Email.Trim(),
                    UserName = model.UserName.Trim(),
                    Name = model.Name.Trim(),
                    PhoneNumber = model.Phone.Trim(),
                    State = StateOfEntity.Pending,
                    CreatedBy = creatorId,
                    ModifiedBy = creatorId
                };
                var result = _userManager.CreateAsync(ouser, model.Password).Result;
                if (result.Succeeded)
                {
                    return FullResponse.Success;
                }
                else
                {
                    return new FullResponse
                    {
                        IsSuccessful = false,
                        Message = result.Errors.Select(e => e.Description).Aggregate((a, b) => a + " " + b)
                    };
                }
            }
            catch (Exception ex)
            {

                return ex.ToFullResponse();
            }
        }

        public void DisableUser(Guid userId)
        {
            var user = _userRepository.GetById(userId);
            user.State = StateOfEntity.Pending;
            _userRepository.Update(user);
        }

        public void EnableUser(Guid userId)
        {
            var user = _userRepository.GetById(userId);
            user.State = StateOfEntity.Complete;
            _userRepository.Update(user);
        }

        public OUser GetById(Guid userId, bool asNoTrack = false)
        {
            return _userRepository.GetById(userId);
        }
        public OUser GetAsAdmin(Guid userId, bool asNoTracking = false)
        {
            return _userRepository.GetByIdAsAdmin(userId,asNoTracking);
        }

        public OUser GetByUserName(string userName, string[] includes = null, bool IsNoTracking = false)
        {
            return _userRepository.FilteredEntities.SingleOrDefault(u => u.UserName == userName);
        }
        public OUser GetByUserNameNoLimit(string userName, string[] includes = null, bool IsNoTracking = false)
        {
            return _userRepository.Entities.SingleOrDefault(u => u.UserName == userName);
        }

        public IEnumerable<string> GetUserRoles(OUser user)
        {
            return _userManager.GetRolesAsync(user).Result;
        }

        public bool HasUserAccessToLogin(string userName)
        {
            var user = GetByUserNameNoLimit(userName,IsNoTracking:true);
            if (user != null && user.State != StateOfEntity.Complete) return false;
            return true;
        }

        public bool IsInRole(OUser user, string roleName)
        {
            return _userManager.IsInRoleAsync(user, roleName).Result;
        }

        public SimpleResponse ResetPassword(OUser user, Guid modifier)
        {
            user.ModifiedAt = DateTime.Now;
            user.ModifiedBy = modifier;
            var removeResult = _userManager.RemovePasswordAsync(user).Result;
            if (!removeResult.Succeeded) return SimpleResponse.FailBecause(removeResult.Errors.Select(e => e.Description).Aggregate((a, b) => a + " " + b));
            var resetResult = _userManager.AddPasswordAsync(user, BasicUser.AdminPassword).Result;
            if (!resetResult.Succeeded) return SimpleResponse.FailBecause(resetResult.Errors.Select(e => e.Description).Aggregate((a, b) => a + " " + b));
            return SimpleResponse.Success();
        }

        public List<RoleListModel> RetrieveRoles()
        {
            throw new NotImplementedException();
        }

        public List<UserListModel> RetrieveUsers()
        {
            var users = _userRepository.GetList(true);
            return users.Select(u => new UserListModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Name = u.Name,
                Email = u.Email,
                Organization = u.Organization.Name,
                Phone = u.PhoneNumber,
                IsEnable = u.State == StateOfEntity.Complete
            }).ToList();
        }
        public Guid SeedingAdd(OUser user, string password)
        {
            user.CreatedBy = user.ModifiedBy = BasicUser.AdminId;
            return Add(user, password);
        }

        public List<Organization> GetUserOrgDescendant(Guid id)
        {
            return _userRepository.Context.Organizations.Where(o => o.Parent.Id == id).ToList();
        }
        public List<OUser> GetUsersInRole(string name)
        {
            return _userManager.GetUsersInRoleAsync(name).Result.ToList();
        }

        public void ChangeOrganization(Guid userId, Guid orgId)
        {
            if(userId == BasicUser.AdminId)
            {
                throw new ApplicationException("Changing Admin organization is not possible");
            }
            var user = _userRepository.GetById(userId);
            if(user == null)
            {
                throw new ApplicationException("User doesn't exist");
            }
            user.OrganizationId = orgId;
            _userRepository.Update(user);
        }
        public Dictionary<string, string> GetMerchantAccountList()
        {
            var merchantRoleId = _userRepository.Context.Roles
                .AsNoTracking()
                .First(r => r.Name == BasicRoles.Merchant.name)
                .Id;
            return _userRepository.Context.Users
                .AsNoTracking()
                .Join(_userRepository.Context.UserRoles, u => u.Id, ur => ur.UserId,(u,ur) => new {User = u, ur.RoleId })
                .Where(x => x.RoleId == merchantRoleId && x.User.State != StateOfEntity.Pending)
                .Select(x => new { x.User.Id, x.User.Email, x.User.Name })
                .ToDictionary(u => u.Id.ToString(), u => $"{u.Email}({u.Name})");
        }
    }
}
