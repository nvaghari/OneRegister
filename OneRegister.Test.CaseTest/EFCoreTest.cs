using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneRegister.Data.Context;
using OneRegister.Data.Entities.MerchantRegistration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Test.CaseTest
{
    [TestClass]
    public class EFCoreTest
    {
        private readonly OneRegisterContext _context;
        public EFCoreTest()
        {
            if(_context == null)
            {
                var option = new DbContextOptionsBuilder<OneRegisterContext>()
                    .UseSqlServer("Data Source=10.88.28.112;Initial Catalog=OneRegister;User ID=sa;Password=abc123;MultipleActiveResultSets=True")
                    .Options;
                _context = new OneRegisterContext(option);
            }
        }
        [TestMethod]
        public void ConnectivityTest()
        {
            Assert.IsTrue(_context.Database.CanConnect());
        }
        [TestMethod]
        public void QueryProjectionPerformanceTest()
        {
            var mid = new Guid("81b99ef9-0311-49a9-bed8-68839fab791a");
            var query = _context.Merchants.AsQueryable();
            //query = query.Include(m => m.MerchantInfo);
            query = query.Include(m => m.OrganizationFiles);
            //var result = query.SingleOrDefault(m => m.Id == mid);
            var result = query.ToList();
            query = query.AsSplitQuery();
            Assert.IsTrue(result != null);
        }
    }
}
