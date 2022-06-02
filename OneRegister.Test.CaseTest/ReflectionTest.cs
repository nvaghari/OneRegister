using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneRegister.Data.SuperEntities;
using OneRegister.Security.Attributes;
using OneRegister.Security.Model;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Test.CaseTest
{
    [TestClass]
    public class ReflectionTest
    {
        [TestMethod]
        public void GettingEnums()
        {
            var dataAssembly = Assembly.Load("OneRegister.Data");
            var enums = dataAssembly.GetTypes()
                .Where(t => t.IsEnum && t.IsPublic)
                .OrderBy(t => t.Name)
                .ToList();

            List<CodeList> yellowList = new();
            for (int i = 0; i < enums.Count; i++)
            {
                var values = enums[i].GetEnumValues();
                for (int j = 0; j < values.Length; j++)
                {
                    var value = values.GetValue(j);
                    yellowList.Add(new CodeList
                    {
                        Name = enums[i].Name,
                        Value = value.ToString(),
                        Key = (int)value,
                        Code = i + 1
                    });
                }
            }
        }

        [TestMethod]
        public void GetPermissionAttributeOverMethod()
        {
            var assembly = Assembly.Load("OneRegister.Web");

            var permissionList = assembly.GetTypes()
                //.Where(t => t.IsClass && t.Name.Contains("MasterCardController"))
                .SelectMany(type => type.GetMethods(), (type, method) => new { TypeName = type.Name, MethodName = method.Name, Attributes = method.CustomAttributes })
                .SelectMany(methods => methods.Attributes, (method, attribute) => new { method.TypeName, method.MethodName, attribute.AttributeType, attribute.NamedArguments })
                .Where(attribute => attribute.AttributeType == typeof(PermissionAttribute))
                .Select(attribute =>  new { attribute.TypeName, attribute.MethodName, attribute.NamedArguments })
                .Select(a => new PermissionAttibuteModel
                {
                    Id = a.NamedArguments.Where(arguments => arguments.MemberName == "Id").FirstOrDefault().TypedValue.Value.ToString(),
                    OrganizationId = a.NamedArguments.Where(arguments => arguments.MemberName == "OrganizationId").FirstOrDefault().TypedValue.Value.ToString(),
                    Name = a.NamedArguments.Where(arguments => arguments.MemberName == "Name").FirstOrDefault().TypedValue.Value.ToString(),
                    ClassName = a.TypeName,
                    MethodName = a.MethodName,
                    DomainName = BasicOrganizations.GetName(a.NamedArguments.Where(arguments => arguments.MemberName == "OrganizationId").FirstOrDefault().TypedValue.Value.ToString())
                })
                .ToList();
            Assert.IsTrue(1==1);
        }
    }
}