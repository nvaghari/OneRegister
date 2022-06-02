using OneRegister.Security.Attributes;
using OneRegister.Security.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OneRegister.Security.Services.Collecting
{
    public class PermissionCollector : IPermissionCollector
    {
        public List<PermissionAttibuteModel> CollectMethodAttributes(string assemblyName)
        {
            var assembly = Assembly.Load(assemblyName);
            if (assembly == null)
            {
                throw new ApplicationException("Assembly Doesn't Exist");
            }
            var permissionType = typeof(IPermissionAttribute);

            var methodPermissions = GetMethodsPermissions(assembly, permissionType);

            var typePermissions = GetTypePermissions(assembly, permissionType);

            return methodPermissions.Concat(typePermissions).ToList();
        }

        private static IEnumerable<PermissionAttibuteModel> GetMethodsPermissions(Assembly assembly,Type permissionType)
        {
            return assembly.GetTypes()
                            .SelectMany(type => type.GetMethods(), (type, method) => new { TypeName = type.Name, MethodName = method.Name, Attributes = method.CustomAttributes })
                            .SelectMany(methods => methods.Attributes, (method, attribute) => new { method.TypeName, method.MethodName, attribute.AttributeType, attribute.ConstructorArguments })
                            .Where(attribute => permissionType.IsAssignableFrom(attribute.AttributeType))
                            .Select(attribute => new { attribute.TypeName, attribute.MethodName, attribute.ConstructorArguments, attribute.AttributeType})
                            .Select(a => new PermissionAttibuteModel
                                {
                                    Id = a.ConstructorArguments[0].Value.ToString(),
                                    Name = a.ConstructorArguments[1].Value.ToString(),
                                    OrganizationId = a.ConstructorArguments[2].Value.ToString(),
                                    ClassName = a.TypeName,
                                    MethodName = a.MethodName,
                                    AttributeType = a.AttributeType.Name
                                });
        }
        private static IEnumerable<PermissionAttibuteModel> GetTypePermissions(Assembly assembly, Type permissionType)
        {
            return assembly.GetTypes()
                            .SelectMany(types => types.CustomAttributes, (type, attribute) => new { type.Name, attribute.AttributeType, attribute.ConstructorArguments })
                            .Where(attribute => permissionType.IsAssignableFrom(attribute.AttributeType))
                            .Select(attribute => new { attribute.Name, attribute.ConstructorArguments, attribute.AttributeType })
                            .Select(a => new PermissionAttibuteModel
                            {
                                Id = a.ConstructorArguments[0].Value.ToString(),
                                Name = a.ConstructorArguments[1].Value.ToString(),
                                OrganizationId = a.ConstructorArguments[2].Value.ToString(),
                                ClassName = a.Name,
                                MethodName = string.Empty,
                                AttributeType = a.AttributeType.Name
                            });
        }

        public Dictionary<Guid, PermissionAttibuteModel> ValidatePermissionCollection(List<PermissionAttibuteModel> permissions)
        {

            try
            {
                Dictionary<Guid, PermissionAttibuteModel> dictionaryList = new();
                foreach (var permission in permissions)
                {
                    dictionaryList.Add(Guid.Parse(permission.Id), permission);
                }
                return dictionaryList;
            }
            catch (ArgumentException ex)
            {

                throw new ApplicationException("One of permissions has duplicate key please remove or regenerate the key: " + ex.Message);
            }
        }
    }
}
