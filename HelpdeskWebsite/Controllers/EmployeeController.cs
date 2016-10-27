using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelpdeskViewModels;

namespace HelpdeskWebsite.Controllers
{
    public class EmployeeController : ApiController
    {

        /*
         *      GetAll()
         *      Controller Get Request to return a list, and load the list of employess into the Employees Page
         */
        [Route("api/employees")]
        public IHttpActionResult Get()
        {
            try
            {
                EmployeeViewModel emp = new EmployeeViewModel();
                List<EmployeeViewModel> allEmployees = emp.GetAll();
                return Ok(allEmployees);
            }
            catch(Exception ex)
            {
                return BadRequest("Retrieve failed - Contact Tech Support");
            }
        }

        /*
         *      GetByEmpId()
         *      Get Request return a given Employee by its id
         */
        [Route("api/employees/{id}")]
        public IHttpActionResult GetEmpById(string id)
        {
            try
            {
                EmployeeViewModel emp = new EmployeeViewModel();
                emp.Id = id;
                emp.GetById();
                return Ok(emp);
            }
            catch(Exception ex)
            {
                return BadRequest("Retrieve failed - Contact Tech Support");
            }
        }

        /*
         *      Put()
         *      Put Request used to update the Employee that was modified
         *      Returns  1 if successful
         *              -1 if unsuccessful
         *              -2 if the data is stale
         *
         */
        [Route("api/employees")]
        public IHttpActionResult Put(EmployeeViewModel emp)
        {
            try
            {
                int retVal = emp.Update();
                switch(retVal)
                {
                    case 1:
                        return Ok("Ok! Employee " + emp.Lastname + " updated!");
                    case -1:
                        return Ok("Employee" + emp.Lastname + " not updated!");
                    case -2:
                        return Ok("Data is Stale for " + emp.Lastname + ", Employee not updated");
                    default:
                        return Ok("Employee " + emp.Lastname + " not updated!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Update Failed - Contact Tech Support");
            }
        }

        /*
         *      Create()
         *      Post Request to Create a new Employee based on the Employee passed into it
         *      If the Id of the created employee is set then it was created successful
         */
        [Route("api/employees")]
        public IHttpActionResult Create(EmployeeViewModel emp)
        {
            try
            {
                emp.Create();
                if(emp.Id != "")
                {
                    return Ok("Ok! Employee " + emp.Lastname + " Created!");
                }
                else
                {
                    return Ok("Error! Employee could not be created");
                }
            }
            catch(Exception ex)
            {
                return BadRequest("Create Failed - Contact Tech Support");

            }
        }

        /*
         *      Delete()
         *      Delete Request deletes the specific Employee based on its id
         *      Returns 1 for success 0 for failure
         */
        [HttpDelete]
        [Route("api/employees/{id}")]
        public IHttpActionResult Delete(string id)
        {
            try
            {
                EmployeeViewModel emp = new EmployeeViewModel();
                emp.Id = id;
                long delRetVal = emp.Delete();

                switch(delRetVal)
                {
                    case 1:
                        return Ok("Ok! Employee " + emp.Lastname + " has been Deleted!");
                    case 0:
                        return Ok("Error! Employee does not Exist!");
                    default:
                        return Ok("Employee " + emp.Lastname + " not deleted!");
                }
            }
            catch(Exception ex)
            {
                return BadRequest("Delete Failed - Contact Tech Support");
            }

        }

    }
}