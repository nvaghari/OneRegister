using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneRegister.Data.Context;
using System.Diagnostics;

namespace OneRegister.Test.CaseTest
{
    [TestClass]
    public class OrganizationTest
    {

        private readonly OneRegisterContext _oneRegisterContext;
        public OrganizationTest()
        {
            var serviceCollector = new ServiceCollection();
            serviceCollector.AddDbContext<OneRegisterContext>(o => o.UseSqlServer("Data Source=10.88.28.112;Initial Catalog=OneRegister;User ID=sa;Password=abc123;"));


            var serviceProvider = serviceCollector.BuildServiceProvider();
            _oneRegisterContext = serviceProvider.GetService<OneRegisterContext>();
        }

        [TestMethod]
        public void IsDbConnected()
        {
            Assert.IsTrue(_oneRegisterContext.Database.CanConnect());
        }
    }
}
