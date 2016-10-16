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
                return BadRequest("Retrieve Failed - " + ex.Message);
            }
        }

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
                return BadRequest("retrieve failed - " + ex.Message);
            }
        }

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
                return BadRequest("Update Failed - " + ex.Message);
            }
        }

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
                return BadRequest("Create Failed - " + ex.Message);

            }
        }

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
                        return Ok("Error! Employee " + emp.Lastname + " employee does not Exist!");
                    default:
                        return Ok("Employee " + emp.Lastname + " not deleted!");
                }
            }
            catch(Exception ex)
            {
                return BadRequest("Delete Failed - " + ex.Message);
            }

        }

    }
}