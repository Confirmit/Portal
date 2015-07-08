using System.IO;
using Core.DB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTestRules
{
    [TestClass]
    public class RuleTests
    {
        [TestInitialize]
        public void Init()
        {
            Manager.ResolveConnection();
        }


        [TestMethod]
        public void TestMethod1()
        {
            new DataBaseHelper().RestoreDatabaseFromOriginal();
            var count = new Query("Select Count(ID) from UptimeEventTypes").ExecScalar();
        }
    }
}
