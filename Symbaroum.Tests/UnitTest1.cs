using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Symbaroum.Tests
{
    [TestClass]
    public static class Configuration
    {
        [AssemblyInitialize]
        public static void Configure(TestContext testContext)
        {
            LocalDbHelper.GetLocalDb("testdb", deleteIfExists: true);
        }
        
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
