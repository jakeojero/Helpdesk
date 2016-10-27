using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelpdeskViewModels;

namespace HelpdeskWebsite.Controllers
{
    public class ProblemController : ApiController
    {

        /*
         *      GetAll() Controller Get Request to load the list of items into the Problems Page
         *
         */
        [Route("api/problems")]
        public IHttpActionResult Get()
        {
            try
            {
                ProblemViewModel prob = new ProblemViewModel();
                List<ProblemViewModel> allProblems = prob.GetAll();
                return Ok(allProblems);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed - Contact Tech Support");
            }
        }

        /*
         *      GetByProblemId()
         *      Get Request return a given problem by its id
         *
         */
        [Route("api/problems/{id}")]
        public IHttpActionResult GetProblemById(string id)
        {
            try
            {
                ProblemViewModel prob = new ProblemViewModel();
                prob.Id = id;
                prob.GetById();
                return Ok(prob);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed - Contact Tech Support");
            }
        }

        /*
         *      Put()
         *      Put Request used to update the problem that was modified
         *      Returns 1 if successful
         *              -1 if unsuccessful
         *              -2 if the data is stale
         *
         */
        [Route("api/problems")]
        public IHttpActionResult Put(ProblemViewModel prob)
        {
            try
            {
                int retVal = prob.Update();
                switch (retVal)
                {
                    case 1:
                        return Ok("Ok! Problem " + prob.Description + " updated!");
                    case -1:
                        return Ok("Problem" + prob.Description + " not updated!");
                    case -2:
                        return Ok("Data is Stale for " + prob.Description + ", Problem not updated");
                    default:
                        return Ok("Department " + prob.Description + " not updated!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Update Failed - Contact Tech Support");
            }
        }

        /*
         *      Create()
         *      Post Request to Create a new Problem based on the Problem passed into it
         *
         */
        [Route("api/problems")]
        public IHttpActionResult Create(ProblemViewModel prob)
        {
            try
            {
                prob.Create();
                if (prob.Id != "")
                {
                    return Ok("Ok! Problem " + prob.Description + " Created!");
                }
                else
                {
                    return Ok("Error! Problem could not be created");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Create Failed - Contact Tech Support");

            }
        }

        /*
         *      Delete()
         *      Delete Request deletes the specific problem based on its id
         *
         */
        [HttpDelete]
        [Route("api/problems/{id}")]
        public IHttpActionResult Delete(string id)
        {
            try
            {
                ProblemViewModel prob = new ProblemViewModel();
                prob.Id = id;
                long delRetVal = prob.Delete();

                switch (delRetVal)
                {
                    case 1:
                        return Ok("Ok! Problem " + prob.Description + " has been Deleted!");
                    case 0:
                        return Ok("Error! Problem does not Exist!");
                    default:
                        return Ok("Problem " + prob.Description + " not deleted!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Delete Failed - Contact Tech Support");
            }

        }
    }
}