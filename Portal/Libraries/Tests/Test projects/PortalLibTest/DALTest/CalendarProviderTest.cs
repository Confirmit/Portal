using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock;
using ConfirmIt.PortalLib.DAL;
using ConfirmIt.PortalLib.Configuration;
using ConfirmIt.PortalLib;

namespace Confirmit.Portal.PortalLib.Test.DALTest
{
    /// <summary>
    /// Tests for CalendarProvider class.
    /// </summary>
    [TestClass]
    [ClearMocks]
    public class CalendarProviderTest
    {
        [TestInitialize()]
        public void TestInitialize()
        {
            MockManager.Init();

            CalendarSection section = new CalendarSection();
            section.ProviderType = "ConfirmIt.PortalLib.DAL.SqlClient.SqlCalendarProvider, PortalLib";

            using (RecordExpectations recorder = RecorderManager.StartRecording())
            {
                recorder.ExpectAndReturn(Globals.Settings.Calendar, section ).RepeatAlways();
                recorder.ExpectAndReturn(section.ConnectionString, "My connection string");
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CalendarProvider_Instance_Ok()
        {
            Assert.IsNotNull(CalendarProvider.Instance);
        }
    }
}
