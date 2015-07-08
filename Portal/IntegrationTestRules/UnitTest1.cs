using System;
using System.Configuration;
using System.IO;
using System.Linq;

using Core.DB;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTestRules
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void Init()
        {
            Manager.ResolveConnection();
        }


        [TestMethod]
        public void TestMethod1()
        {
            
        }
    }
}
