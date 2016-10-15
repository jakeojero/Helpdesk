using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpdeskViewModels;

namespace Case1UnitTests
{
    [TestClass]
    public class DepartmentViewModelTests
    {
        string did = "";

        public DepartmentViewModelTests()
        {
            ViewModelUtils vUtil = new ViewModelUtils();
            vUtil.LoadCollections();

            DepartmentViewModel vm = new DepartmentViewModel();
            vm.Name = "Administration";
            vm.GetByDepartmentName();
            did = vm.Id;
        }

        [TestMethod]
        public void TestGetByDepartmentNameShouldPopulateProps()
        {
            DepartmentViewModel vm = new DepartmentViewModel();
            vm.Name = "Administration";
            vm.GetByDepartmentName();
            Assert.IsTrue(vm.Id.Length == 24);
        }

        [TestMethod]
        public void TestGetByIdShouldPopulateProps()
        {
            DepartmentViewModel vm = new DepartmentViewModel();
            vm.Id = did;
            vm.GetById();
            Assert.IsTrue(vm.Id.Length == 24);
        }

        [TestMethod]
        public void TestCreateShouldReturnNewId()
        {
            DepartmentViewModel vm = new DepartmentViewModel();
            vm.Name = "Test";
            vm.Version = 1;
            vm.Create();
            Assert.IsTrue(vm.Id.Length == 24);
        }

        [TestMethod]
        public void TestUpdateShouldReturnOk()
        {
            DepartmentViewModel vm = new DepartmentViewModel();
            vm.Name = "Lab";
            vm.GetByDepartmentName();
            vm.Name = "Update";
            Assert.IsTrue(vm.Update() == 1);
        }

        [TestMethod]
        public void TestUpdateShouldReturnStale()
        {
            DepartmentViewModel vm = new DepartmentViewModel();
            DepartmentViewModel vm2 = new DepartmentViewModel();
            vm.Name = "Maintenance";
            vm2.Name = "Maintenance";
            vm.GetByDepartmentName();
            vm2.GetByDepartmentName();
            vm.Name = "Update2";
            vm2.Name = "Update3";
            vm.Update();
            Assert.IsTrue(vm2.Update() == -2);
        }

        [TestMethod]
        public void TestDeleteShouldReturnOne()
        {
            DepartmentViewModel vm = new DepartmentViewModel();
            vm.Name = "Administration";
            vm.GetByDepartmentName();
            Assert.IsTrue(vm.Delete() == 1);
        }
    }
}
