using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpdeskViewModels;

namespace Case1UnitTests
{
    [TestClass]
    public class CallViewModelTests
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
        [ExpectedException(typeof(NullReferenceException))]
        public void ComprehensiveVMTestsShouldReturnTrue()
        {
            CallViewModel cvm = new CallViewModel();
            EmployeeViewModel evm = new EmployeeViewModel();
            ProblemViewModel pvm = new ProblemViewModel();
            cvm.DateOpened = DateTime.Now;
            cvm.DateClosed = null;
            cvm.OpenStatus = true;
            evm.Lastname = "Smartypants";
            evm.GetByLastname();
            cvm.EmployeeId = evm.Id;
            evm.Lastname = "Burner";
            evm.GetByLastname();
            cvm.TechId = evm.Id;
            pvm.Description = "Memory Upgrade";
            pvm.GetByProblemDescription();
            cvm.ProblemId = pvm.Id;
            cvm.Notes = "Bigshot has bad RAM, Burner to fix it";
            cvm.Version = 1;
            cvm.Create();
            this.tstCtx.WriteLine("New Call Generated - Id = " + cvm.Id);
            cvm.GetById();
            this.tstCtx.WriteLine("New Call Retrieved");
            cvm.Notes = "\n Ordered new RAM!";

            if(cvm.Update() == 1)
            {
                this.tstCtx.WriteLine("Call was updated " + cvm.Notes);
            }
            else
            {
                this.tstCtx.WriteLine("Call was not updated");
            }

            if(cvm.Update() == -2)
            {
                this.tstCtx.WriteLine("Call was not updated data was stale");
            }

            if(cvm.Delete() == 1)
            {
                this.tstCtx.WriteLine("Call was deleted");
            }
            else
            {
                this.tstCtx.WriteLine("Call was not deleted");
            }

            cvm.GetById(); // should throw expected exception
        }
    }
}
