using System;
using System.Diagnostics;
// added for Mongo driver
using MongoDB.Driver;

namespace HelpdeskDAL
{
    /// <summary>
    /// Utility Class for Case Study #1
    ///     Containing common error routine, db creation methods
    /// </summary>
    public class DALUtils
    {
        DbContext ctx;

        public DALUtils()
        {
            ctx = new DbContext();
        }

        /// <summary>
        /// Common DAL Error Method
        /// </summary>
        /// <param name="e">Exception thrown</param>
        /// <param name="obj">Class throwing exception</param>
        /// <param name="method">Method throwing exception</param>
        public static void ErrorRoutine(Exception e, string obj, string method)
        {
            if (e.InnerException != null)
            {
                Trace.WriteLine("Error in DAL, object=" + obj +
                               ", method=" + method +
                               " , inner exception message=" +
                               e.InnerException.Message);
                throw e.InnerException;
            }
            else
            {
                Trace.WriteLine("Error in DAL, object=" + obj +
                           ", method=" + method + " , message=" +
                           e.Message);

                throw e;
            }
        }
        /// <summary>
        /// Main Loading Method for Mongo based HelpdeskD
        /// Revisions: Added Problem methods
        /// </summary>
        public bool LoadCollections()
        {
            bool createOk = false;

            try
            {
                DropAndCreateCollections();
                LoadDepartments();
                LoadEmployees();
                LoadProblems();
                createOk = true;
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "DALUtils", "LoadCollections");
            }
            return createOk;
        }


        private void DropAndCreateCollections()
        {
            if (ctx.Db.GetCollection<Employee>("employees") != null)
            {
                ctx.Db.DropCollection("employees");
            }
            if (ctx.Db.GetCollection<Department>("departments") != null)
            {
                ctx.Db.DropCollection("departments");
            }
            if (ctx.Db.GetCollection<Problem>("problems") != null)
            {
                ctx.Db.DropCollection("problems");
            }

            ctx.Db.CreateCollection("problems");
            ctx.Db.CreateCollection("departments");
            ctx.Db.CreateCollection("employees");
        }

        private void LoadDepartments()
        {
            InsertDepartment("Administration");
            InsertDepartment("Sales");
            InsertDepartment("Food Services");
            InsertDepartment("Lab");
            InsertDepartment("Maintenance");
        }

        private void LoadProblems()
        {
            InsertProblem("Device Not Plugged In");
            InsertProblem("Device Not Turned On");
            InsertProblem("Hard Drive Failure");
            InsertProblem("Memory Failure");
            InsertProblem("Power Supply Failure");
            InsertProblem("Password fails due to Caps Lock being on");
            InsertProblem("Network Card Faulty");
            InsertProblem("Cpu Fan Failure");
            InsertProblem("Memory Upgrade");
            InsertProblem("Graphics Upgrade");
            InsertProblem("Needs software upgrade");
        }

        /// <summary>
        /// helper method to actually insert decription for problem
        /// </summary>
        /// <param name="description"></param>
        private void InsertProblem(string description)
        {
            Problem prb = new Problem();
            prb.Description = description;
            prb.Version = 1;
            ctx.Db.GetCollection<Problem>("problems").InsertOne(prb);
        }

        private void LoadEmployees()
        {
            InsertEmployee("Mr.", "Bigshot", "Smartypants", "(555) 555-5551", "bs@abc.com", "Administration");
            InsertEmployee("Mrs.", "Penny", "Pincher", "(555) 555-5551", "pp@abc.com", "Administration");
            InsertEmployee("Mr.", "Smoke", "Andmirrors", "(555) 555-5552", "sa@abc.com", "Sales");
            InsertEmployee("Mr.", "Sam", "Slick", "(555) 555-5552", "ss@abc.com", "Sales");
            InsertEmployee("Mr.", "Sloppy", "Joe", "(555) 555-5553", "sj@abc.com", "Food Services");
            InsertEmployee("Mr.", "Franken", "Beans", "(555) 555-5553", "fb@abc.com", "Food Services");
            InsertEmployee("Mr.", "Bunsen", "Burner", "(555) 555-5554", "bb@abc.com", "Lab");
            InsertEmployee("Ms.", "Petrie", "Dish", "(555) 555-5554", "pd@abc.com", "Lab");
            InsertEmployee("Ms.", "Mopn", "Glow", "(555) 555-5555", "mg@abc.com", "Maintenance");
            InsertEmployee("Mr.", "Spickn", "Span", "(555) 555-5555", "sps@abc.com", "Maintenance");
        }

        /// <summary>
        /// helper method to actually insert name property into document
        /// </summary>
        /// <param name="name"></param>
        private void InsertDepartment(string name)
        {
            Department dep = new Department();
            dep.DepartmentName = name;
            dep.Version = 1;
            ctx.Db.GetCollection<Department>("departments").InsertOne(dep);
        }

        private void InsertEmployee(string title,
                                    string first,
                                    string last,
                                    string phone,
                                    string email,
                                    string dept)
        {
            Employee emp = new Employee();
            emp.Title = title;
            emp.Firstname = first;
            emp.Lastname = last;
            emp.Phoneno = phone;
            emp.Email = email;
            emp.Version = 1;
            var filter = Builders<Department>.Filter.Eq("DepartmentName", dept);
            var dep = ctx.Db.GetCollection<Department>("departments").Find(filter).SingleOrDefault();
            emp.DepartmentId = dep.Id;
            ctx.Db.GetCollection<Employee>("employees").InsertOne(emp);
        }
    }
}
