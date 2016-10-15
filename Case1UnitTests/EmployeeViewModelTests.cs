using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpdeskViewModels;

namespace Case1UnitTests
{
    [TestClass]
    public class EmployeeViewModelTests
    {
        string eid = "";
        string did = "";

        public EmployeeViewModelTests()
        {
            ViewModelUtils vUtil = new ViewModelUtils();
            vUtil.LoadCollections();

            EmployeeViewModel vm = new EmployeeViewModel();
            vm.Lastname = "Smartypants";
            vm.GetByLastname();
            eid = vm.Id;
            did = vm.DepartmentId;
        }

        [TestMethod]
        public void TestGetByLastnameShouldPopulateProps()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.Lastname = "Smartypants";
            vm.GetByLastname();
            Assert.IsTrue(vm.DepartmentId.Length == 24);
        }

        [TestMethod]
        public void TestGetByIdShouldPopulateProps()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.Id = eid;
            vm.GetById();
            Assert.IsTrue(vm.DepartmentId.Length == 24);
        }

        [TestMethod]
        public void TestCreateShouldReturnNewId()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.Title = "Mr.";
            vm.Firstname = "Jake";
            vm.Lastname = "Test";
            vm.Phoneno = "(555)111-1111";
            vm.Email = "jakeojero@hotmail.com";
            vm.Version = 1;
            vm.DepartmentId = did;
            vm.Create();

            Assert.IsTrue(vm.Id.Length == 24);
        }

        [TestMethod]
        public void TestUpdateShouldReturnOk()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.Id = eid;
            vm.GetById();
            vm.Phoneno = "(555)555-9999";
            Assert.IsTrue(vm.Update() == 1);
        }

        [TestMethod]
        public void TestUpdateShouldReturnStale()
        {
            EmployeeViewModel vm1 = new EmployeeViewModel();
            EmployeeViewModel vm2 = new EmployeeViewModel();
            vm1.Id = eid;
            vm2.Id = eid;
            vm1.GetById();
            vm2.GetById();
            vm1.Phoneno = "(555)555-1111";
            vm2.Phoneno = "(555)555-2222";
            vm1.Update();
            Assert.IsTrue(vm2.Update() == -2);
        }

        [TestMethod]
        public void TestDeleteShouldReturnOne()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.Lastname = "Smartypants";
            vm.GetByLastname();
            Assert.IsTrue(vm.Delete() == 1);
        }

    }
}
