using Microsoft.Extensions.Caching.Memory;
using OneRegister.Security.Attributes;
using OneRegister.Security.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Web.Services.Menu.SideMenu
{
    public class SideMenuService : ISideMenuService
    {
        private const string _menuCacheKey = "Menus";
        private readonly IPermissionService _permissionService;
        private readonly IMemoryCache _memoryCache;

        public SideMenuService(IPermissionService permissionService, IMemoryCache memoryCache)
        {
            _permissionService = permissionService;
            _memoryCache = memoryCache;
        }

        public List<SideMenuModel> GetCollectedMenus()
        {
            if (_memoryCache.TryGetValue(_menuCacheKey, out List<SideMenuModel> chachedMenus))
            {
                return chachedMenus;
            }
            var menus = CollectMenuList();
            _memoryCache.Set(_menuCacheKey, menus);
            return menus;
        }
        private List<SideMenuModel> CollectMenuList()
        {
            var assembly = Assembly.GetEntryAssembly();
            var typeMenus = GetTypeMenuAttributes(assembly).ToList();
            var methodMenus = GetMethodMenuAttributes(assembly).ToList();


            return typeMenus.Concat(methodMenus).ToList();

        }

        private static IEnumerable<SideMenuModel> GetMethodMenuAttributes(Assembly assembly)
        {
            return assembly.GetTypes()
                .SelectMany(type => type.GetMethods(), (type, method) => new { TypeName = type.Name, MethodName = method.Name, method.CustomAttributes })
                .SelectMany(method => method.CustomAttributes, (method, attribute) => new { method.TypeName, method.MethodName, attribute })
                .Where(a => a.attribute.AttributeType == typeof(MenuAttribute))
                .Select(a => new SideMenuModel
                {
                    ClassName = a.TypeName,
                    MethodName = a.MethodName,
                    Id = Guid.Parse(a.attribute.ConstructorArguments[0].Value.ToString()),
                    Name = a.attribute.ConstructorArguments[1].Value.ToString(),
                    Order = Convert.ToInt32(a.attribute.NamedArguments.Where(arg => arg.MemberName == nameof(MenuAttribute.Order)).FirstOrDefault().TypedValue.Value.ToString()),
                    Parent = a.attribute.NamedArguments.Where(arg => arg.MemberName == nameof(MenuAttribute.Parent)).FirstOrDefault().TypedValue.Value == null ? null : Guid.Parse(a.attribute.NamedArguments.Where(arg => arg.MemberName == nameof(MenuAttribute.Parent)).FirstOrDefault().TypedValue.Value.ToString()),
                    FasIcon = a.attribute.NamedArguments.Where(arg => arg.MemberName == nameof(MenuAttribute.FasIcon)).FirstOrDefault().TypedValue.Value?.ToString(),
                    Link = a.attribute.NamedArguments.Where(arg => arg.MemberName == nameof(MenuAttribute.Link)).FirstOrDefault().TypedValue.Value?.ToString(),
                });
        }

        private static IEnumerable<SideMenuModel> GetTypeMenuAttributes(Assembly assembly)
        {
            return assembly.GetTypes()
                .SelectMany(type => type.CustomAttributes, (type, attribute) => new { TypeName = type.Name, attribute })
                .Where(a => a.attribute.AttributeType == typeof(MenuAttribute))
                .Select(a => new SideMenuModel
                {
                    ClassName = a.TypeName,
                    MethodName = string.Empty,
                    Id = Guid.Parse(a.attribute.ConstructorArguments[0].Value.ToString()),
                    Name = a.attribute.ConstructorArguments[1].Value.ToString(),
                    Order = Convert.ToInt32(a.attribute.NamedArguments.Where(arg => arg.MemberName == nameof(MenuAttribute.Order)).FirstOrDefault().TypedValue.Value.ToString()),
                    Parent = a.attribute.NamedArguments.Where(arg => arg.MemberName == nameof(MenuAttribute.Parent)).FirstOrDefault().TypedValue.Value == null ? null : Guid.Parse(a.attribute.NamedArguments.Where(arg => arg.MemberName == nameof(MenuAttribute.Parent)).FirstOrDefault().TypedValue.Value.ToString()),
                    FasIcon = a.attribute.NamedArguments.Where(arg => arg.MemberName == nameof(MenuAttribute.FasIcon)).FirstOrDefault().TypedValue.Value?.ToString(),
                    Link = a.attribute.NamedArguments.Where(arg => arg.MemberName == nameof(MenuAttribute.Link)).FirstOrDefault().TypedValue.Value?.ToString(),
                });
        }

        public List<SideMenuModel> GetAuthorizedMenus(ClaimsPrincipal User)
        {
            List<SideMenuModel> menus = new();
            var userRoles = _permissionService.GetUserRoles(User);
            if (userRoles == null || userRoles.Count == 0) return menus;
            menus = GetCollectedMenus();
            if (userRoles.Contains(BasicRoles.SuperAdmin.name))
            {
                return menus;
            }
            var athorizedMenuIds = _permissionService.GetAuthorizedMenus(User);
            return menus.IntersectBy(athorizedMenuIds, m => m.Id).ToList();
        }
    }
}
