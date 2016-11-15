using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpdeskDAL;

namespace Case1UnitTests
{
    [TestClass]
    public class CallDAOTests
    {

        private TestContext tstCtx;

        public TestContext TestContext
        {
            get
            {
                return tstCtx;
            }
            set
            {
                tstCtx = value;
            }
        }

        [TestMethod]
        public void ComprehensiveDAOTestsShouldReturnTrue()
        {
            CallDAO cdao = new CallDAO();
            EmployeeDAO edao = new EmployeeDAO();
            ProblemDAO pdao = new ProblemDAO();
            Call call = new Call();
            call.DateOpened = System.DateTime.Now;
            call.DateClosed = null;
            call.OpenStatus = true;
            call.SetEmployeeIdFromString(edao.GetByLastname("Smartypants").GetIdAsString());
            call.SetTechIdFromString(edao.GetByLastname("Burner").GetIdAsString());
            call.SetProblemIdFromString(pdao.GetByProblemDescription("Memory Upgrade").GetIdAsString());
            call.Notes = "Bigshot has bad RAM, burner to fix it";
            call.Version = 1;
            call = cdao.Create(call);
            this.tstCtx.WriteLine("New Call Generated - Id = " + call.GetIdAsString());
            call = cdao.GetById(call.GetIdAsString());
            this.tstCtx.WriteLine("New Call Retrieved");
            call.Notes = "\n Ordered new RAM!";

            if(cdao.Update(call) == UpdateStatus.Ok)
            {
                this.tstCtx.WriteLine("Call was updated " + call.Notes);
            }
            else
            {
                this.tstCtx.WriteLine("Call was not updated!");
            }

            if(cdao.Update(call) == UpdateStatus.Stale)
            {
                this.tstCtx.WriteLine("Call not updated due to stale data");
            }

            if(cdao.Delete(call.GetIdAsString()) == 1)
            {
                this.tstCtx.WriteLine("Call was deleted!");
            }
            else
            {
                this.tstCtx.WriteLine("Call was not deleted");
            }

            Assert.IsNull(cdao.GetById(call.GetIdAsString()));

        }


    }
}

