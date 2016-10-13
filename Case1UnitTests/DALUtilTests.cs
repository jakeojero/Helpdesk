using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpdeskDAL;

namespace Case1UnitTests
{
    [TestClass]
    public class DALUtilTests
    {
        [TestMethod]
        public void TestLoadCollectionsShouldReturnTrue()
        {
            DALUtils util = new DALUtils();
            Assert.IsTrue(util.LoadCollections());
        }
    }
}
