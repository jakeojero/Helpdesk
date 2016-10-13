using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace HelpdeskDAL
{
    public class EmployeeDAO
    {
        private IRepository repo;

        public EmployeeDAO()
        {
            repo = new HelpdeskRepository();
        }

        public Employee GetByLastname(string name)
        {
            Employee emp = null;
            var builder = Builders<Employee>.Filter;
            var filter = builder.Eq("Lastname", name);

            try
            {
                emp = repo.GetOne<Employee>(filter);
            }
            catch(Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "EmployeeDAO", "GetByLastname");
            }

            return emp;
        }

        public Employee GetById(string id)
        {
            Employee emp = null;
            var builder = Builders<Employee>.Filter;
            var filter = builder.Eq("Id", new ObjectId(id));

            try
            {
                emp = repo.GetOne<Employee>(filter);
            }
            catch(Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "EmployeeDAO", "GetById");
            }
            return emp;
        }

        public List<Employee> GetAll()
        {
            List<Employee> emps = null;

            try
            {
                emps = repo.GetAll<Employee>();
            }
            catch(Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "EmployeeDAO", "GetAll");
            }

            return emps;
        }

        public UpdateStatus Update(Employee emp)
        {
            UpdateStatus status = UpdateStatus.Failed;
            repo = new HelpdeskRepository(new DbContext());

            try
            {
                var builder = Builders<Employee>.Filter;
                var filter = builder.Eq("Id", emp.Id) & builder.Eq("Version", emp.Version);
                var update = Builders<Employee>.Update
                    .Set("DepartmentId", emp.DepartmentId)
                    .Set("Email", emp.Email)
                    .Set("Firstname", emp.Firstname)
                    .Set("Lastname", emp.Lastname)
                    .Set("Phoneno", emp.Phoneno)
                    .Set("Title", emp.Title)
                    .Inc("Version", 1);

                status = repo.Update(emp.Id.ToString(), filter, update);
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "EmployeeDAO", "Update");
            }

            return status;
        }

        public long Delete(string id)
        {
            repo = new HelpdeskRepository(new DbContext());
            long delete_status = 0;

            try
            {
               delete_status = repo.Delete<Employee>(id);              
            }
            catch(Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "EmployeeDAO", "Delete");
            }

            return delete_status;
        }

        public Employee Create(Employee emp)
        {
            repo = new HelpdeskRepository(new DbContext());
            Employee empRet = null;
            try
            {
                empRet = repo.Create<Employee>(emp);
            }
            catch(Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "EmployeeDAO", "Create");
            }

            return empRet;
        }

        private bool Exists(ObjectId id)
        {
            DbContext ctx = new DbContext();
            var collection = ctx.GetCollection<Employee>();
            var filter = Builders<Employee>.Filter.Eq("Id", id);
            var emps = collection.Find(filter);
            return (emps.Count() > 0);
        }
    }
}
