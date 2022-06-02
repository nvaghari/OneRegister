using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneRegister.Security.Attributes;
using OneRegister.Security.Contract;
using OneRegister.Security.Model;
using OneRegister.Security.Services;
using OneRegister.Security.Services.Collecting;
using OneRegister.Security.Test.Mocks;
using OneRegister.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Security.Test
{

    [TestClass]
    public class PermissionCollectorTest
    {
        private IServiceProvider _serviceProvider;
        public PermissionCollectorTest()
        {
            var sc = new ServiceCollection();
            sc.AddScoped<IPermissionCollector, PermissionCollector>();
            sc.AddScoped<IPermissionRepository, MockPermissionRepository>();
            sc.AddScoped<IPermissionService, PermissionService>();

            _serviceProvider = sc.BuildServiceProvider();
        }
        [TestMethod]
        public void CheckIfIocContainerCanBeResolved()
        {
            Assert.IsNotNull(_serviceProvider);
        }
        [TestMethod]
        public void CheckIfIocContainerCanResolveIPermissionCollector()
        {
            var collector = _serviceProvider.GetService<IPermissionCollector>();
            Assert.IsNotNull(collector);
        }
        [TestMethod]
        public void CheckIfCanFindAttributes()
        {
            var collector = _serviceProvider.GetService<IPermissionCollector>();

            var result = collector.CollectMethodAttributes("OneRegister.Web");

            Assert.IsTrue(result.Any());
        }
        [TestMethod]
        public void TryToGetAttributesOverClass()
        {
            var permissionType = typeof(IPermissionAttribute);
            var assembly = Assembly.Load("OneRegister.Web");
            var t = assembly.GetTypes()
                .SelectMany(t => t.CustomAttributes, (type, attribute) => new { type.Name, attribute.AttributeType, attribute.NamedArguments })
                .Where(attribute => permissionType.IsAssignableFrom(attribute.AttributeType))
                .ToList();
        }
        [TestMethod]
        public void DuplicateAttributesFounded()
        {
            var collector = _serviceProvider.GetService<IPermissionCollector>();

            var attributes = collector.CollectMethodAttributes("OneRegister.Web");
            attributes.Add(attributes.First());
            Assert.ThrowsException<ApplicationException>(() => collector.ValidatePermissionCollection(attributes));
        }

        [TestMethod]
        public void CheckIfRepositoryIsNull()
        {
            var repository = _serviceProvider.GetService<IPermissionRepository>();

            Assert.IsNotNull(repository.RetrievePermissions());
        }
        [TestMethod]
        public void CheckIfPermissionModelEquatorWork()
        {
            var a = new PermissionAttibuteModel()
            {
                Id = "89B6D466-F95F-4F4B-970E-E04C60556B2C",
                Name = "View Register Page",
                OrganizationId = BasicOrganizations.MasterCard_ID,
                MethodName = "Register",
                ClassName = "MasterCardController"
            };

            var b = new PermissionAttibuteModel()
            {
                Id = "89B6D466-F95F-4F4B-970E-E04C60556B2C",
                Name = "View Register Page",
                OrganizationId = BasicOrganizations.MasterCard_ID,
                MethodName = "Register",
                ClassName = "MasterCardController"
            };

            Assert.AreEqual(a, b);
            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(a == b);

            b.MethodName = "Test";
            Assert.AreNotEqual(a, b);

        }
        [TestMethod]
        public void FindAddedPermission()
        {

            var _collector = _serviceProvider.GetService<IPermissionCollector>();
            var collectedPermission = _collector.CollectMethodAttributes("OneRegister.Web");
            var permissions = _collector.ValidatePermissionCollection(collectedPermission);

            var _repo = _serviceProvider.GetService<IPermissionRepository>();
            var repoPermissions = _repo.RetrievePermissions();

            var _permissionService = _serviceProvider.GetService<IPermissionService>();
            var toAddPermissions = _permissionService.GetAddPermissions(permissions, repoPermissions);
            Assert.AreEqual(1, toAddPermissions.Count);

            var toDeletePermissions = _permissionService.GetDeletePermissions(permissions,repoPermissions);
            Assert.AreEqual(1,toDeletePermissions.Count);

            var toUpdatePermissions = _permissionService.GetUpdatePermissions(permissions, repoPermissions);
            Assert.AreEqual(0, toUpdatePermissions.Count);
        }
    }
}
