using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneRegister.Domain.Services.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Test.CaseTest
{
    [TestClass]
    public class HashTest
    {
        [TestMethod]
        public void RPPSHA256()
        {
            var text = "8CHECKACCT14";

            var hashStr = CryptoService.SHA256ToHex(text);

            Assert.AreEqual("4eedaeb5e0100ce4bb89775eaf71cff5d458ab2c8d014ecbb1719b83cbb7014b",hashStr);
        }
    }
}
