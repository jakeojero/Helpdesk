using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpdeskDAL;

namespace Case1UnitTests
{
    [TestClass]
    public class DepartmentDAOTests
    {
        string did = "";
        public DepartmentDAOTests()
        {
            DALUtils util = new DALUtils();
            util.LoadCollections();

            DepartmentDAO dao = new DepartmentDAO();
            Department dep = dao.GetByDepartmentName("Administration");
            did = dep.GetIdAsString();
        }

        [TestMethod]
        public void TestGetByDepartmentNameShouldReturnDepartment()
        {
            DepartmentDAO dao = new DepartmentDAO();
            Assert.IsInstanceOfType(dao.GetByDepartmentName("Administration"), typeof(Department));
        }

        [TestMethod]
        public void TestGetByDepartmentIdShouldReturnDepartment()
        {
            DepartmentDAO dao = new DepartmentDAO();
            Assert.IsInstanceOfType(dao.GetByDepartmentId(did), typeof(Department));
        }

        [TestMethod]
        public void TestCreateShouldReturnNewId()
        {
            DepartmentDAO dao = new DepartmentDAO();
            Department dep = new Department();
            dep.DepartmentName = "Medical";
            dep.Version = 1;
            // 12 byte hex = 24 byte string
            Assert.IsTrue(dao.Create(dep).GetIdAsString().Length == 24);
        }

        [TestMethod]
        public void TestUpdateShouldReturnOk()
        {
            DepartmentDAO dao = new DepartmentDAO();
            Department dep = dao.GetByDepartmentName("Maintenance");
            dep.DepartmentName = "Butcher";
            Assert.IsTrue(dao.Update(dep) == UpdateStatus.Ok);
        }

        [TestMethod]
        public void TestUpdateShouldReturnStale()
        {
            DepartmentDAO dao = new DepartmentDAO();
            Department dep = dao.GetByDepartmentName("Lab");
            Department dep2 = dao.GetByDepartmentName("Lab");
            dep.DepartmentName = "Butcher";
            dep2.DepartmentName = "LOL";
            UpdateStatus status = dao.Update(dep);
            Assert.IsTrue(dao.Update(dep2) == UpdateStatus.Stale);
        }

        [TestMethod]
        public void TestDeleteShouldReturnOne()
        {
            DepartmentDAO dao = new DepartmentDAO();
            Department dep = dao.GetByDepartmentName("Medical");
            if (dep == null)//did delete run before create 
            {
                TestCreateShouldReturnNewId();
                dep = dao.GetByDepartmentName("Medical");
            }
            Assert.IsTrue(dao.Delete(dep.GetIdAsString()) == 1);

        }


    }
}

