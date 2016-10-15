using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpdeskDAL;


namespace Case1UnitTests
{
    [TestClass]
    public class ProblemDAOTests
    {
        string pid = "";

        public ProblemDAOTests()
        {
            DALUtils util = new DALUtils();
            util.LoadCollections();

            ProblemDAO dao = new ProblemDAO();
            Problem prob = dao.GetByProblemDescription("Device Not Plugged In");
            pid = prob.GetIdAsString();
        }

        [TestMethod]
        public void TestGetByProblemDescriptionShouldReturnProblem()
        {
            ProblemDAO dao = new ProblemDAO();
            Assert.IsInstanceOfType(dao.GetByProblemDescription("Device Not Turned On"), typeof(Problem));
        }

        [TestMethod]
        public void TestGetByProblemIdShouldReturnProblem()
        {
            ProblemDAO dao = new ProblemDAO();
            Assert.IsInstanceOfType(dao.GetByProblemId(pid), typeof(Problem));
        }

        [TestMethod]
        public void TestUpdateShouldReturnOk()
        {
            ProblemDAO dao = new ProblemDAO();
            Problem prob = dao.GetByProblemDescription("Hard Drive Failure");
            prob.Description = "No Ram";
            Assert.IsTrue(dao.Update(prob) == UpdateStatus.Ok);
        }

        [TestMethod]
        public void TestUpdateShouldReturnStale()
        {
            ProblemDAO dao = new ProblemDAO();
            Problem prob = dao.GetByProblemDescription("Memory Failure");
            Problem prob2 = dao.GetByProblemDescription("Memory Failure");
            prob.Description = "Butcher";
            prob2.Description = "LOL";
            UpdateStatus status = dao.Update(prob);
            Assert.IsTrue(dao.Update(prob2) == UpdateStatus.Stale);
        }

        [TestMethod]
        public void TestCreateShouldReturnNewId()
        {
            ProblemDAO dao = new ProblemDAO();
            Problem prob = new Problem();
            prob.Description = "Testing";
            prob.Version = 1;
            Assert.IsTrue(dao.Create(prob).GetIdAsString().Length == 24);
        }

        [TestMethod]
        public void TestDeleteShouldReturnOne()
        {
            ProblemDAO dao = new ProblemDAO();
            Problem prob = dao.GetByProblemDescription("Testing");
            if(prob == null)
            {
                TestCreateShouldReturnNewId();
                prob = dao.GetByProblemDescription("Testing");
            }
            Assert.IsTrue(dao.Delete(prob.GetIdAsString()) == 1);
        }
    }
}
