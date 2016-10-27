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

        /*
        *  GetByLastname()
        *  Uses the Repository and Gets the specific employee based on the "Lastname" of the Employee
        */
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

        /*
        *  GetById()
        *  Uses the Repository and Gets the specific employee based on the "id" of the Employee
        *  Returns an Employee Object
        */
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

        /*
        *  GetAll()
        *  Uses the Repository and Returns a List of all Employees
        */
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

        /*
       *  Update()
       *  Uses the Repository and creates an update definition to be used in the repo's update method
       *  Returns an UpdateStatus enum Number based on the result
       */
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

     /*
      *  Delete()
      *  Uses the Repository and deletes the Employee id that is passed to this function
      *  Returns a delete status number based on if the delete was successful or not
      */
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

        /*
         *  Create()
         *  Uses the Repository and creates the Employee based on the object that was passed in as an argument
         *  Returns the created Employee that was created
         */
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
