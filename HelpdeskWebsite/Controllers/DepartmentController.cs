using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelpdeskViewModels;

namespace HelpdeskWebsite.Controllers
{
    public class DepartmentController : ApiController
    {
        [Route("api/departments")]
        public IHttpActionResult Get()
        {
            try
            {
                DepartmentViewModel dep = new DepartmentViewModel();
                List<DepartmentViewModel> allDepartments = dep.GetAll();
                return Ok(allDepartments);
            }
            catch(Exception ex)
            {
                return BadRequest("Retrieve failed - Contact Tech Support");
            }
        }

        [Route("api/departments/{id}")]
        public IHttpActionResult GetDepById(string id)
        {
            try
            {
                DepartmentViewModel dep = new DepartmentViewModel();
                dep.Id = id;
                dep.GetById();
                return Ok(dep);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed - Contact Tech Support");
            }
        }

        [Route("api/departmentname/{name}")]
        public IHttpActionResult GetDepByName(string name)
        {
            try
            {
                DepartmentViewModel dep = new DepartmentViewModel();
                dep.Name = name;
                dep.GetByDepartmentName();
                return Ok(dep);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed - Contact Tech Support");
            }
        }

        [Route("api/departments")]
        public IHttpActionResult Put(DepartmentViewModel dep)
        {
            try
            {
                int retVal = dep.Update();
                switch (retVal)
                {
                    case 1:
                        return Ok("Ok! Department " + dep.Name + " updated!");
                    case -1:
                        return Ok("Department" + dep.Name + " not updated!");
                    case -2:
                        return Ok("Data is Stale for " + dep.Name + ", Department not updated");
                    default:
                        return Ok("Department " + dep.Name + " not updated!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Update Failed - Contact Tech Support");
            }
        }

        [Route("api/departments")]
        public IHttpActionResult Create(DepartmentViewModel dep)
        {
            try
            {
                dep.Create();
                if (dep.Id != "")
                {
                    return Ok("Ok! Department " + dep.Name + " Created!");
                }
                else
                {
                    return Ok("Error! Department could not be created");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Create Failed - Contact Tech Support");

            }
        }

        [HttpDelete]
        [Route("api/departments/{id}")]
        public IHttpActionResult Delete(string id)
        {
            try
            {
                DepartmentViewModel dep = new DepartmentViewModel();
                dep.Id = id;
                long delRetVal = dep.Delete();

                switch (delRetVal)
                {
                    case 1:
                        return Ok("Ok! Department " + dep.Name + " has been Deleted!");
                    case 0:
                        return Ok("Error! Department does not Exist!");
                    default:
                        return Ok("Department " + dep.Name + " not deleted!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Delete Failed - Contact Tech Support");
            }

        }

    }
}