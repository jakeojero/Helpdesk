using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelpdeskViewModels;

namespace HelpdeskWebsite.Controllers
{
    public class CallController : ApiController
    {
        [Route("api/calls")]
        public IHttpActionResult Get()
        {
            try
            {
                CallViewModel call = new CallViewModel();
                List<CallViewModel> allCalls = call.GetAll();
                return Ok(allCalls);
            }
            catch(Exception ex)
            {
                return BadRequest("Retrieve Failed - Contact Tech Support");
            }
        }

        [Route("api/calls/{id}")]
        public IHttpActionResult GetCallById(string id)
        {
            try
            {
                CallViewModel call = new CallViewModel();
                call.Id = id;
                call.GetById();
                return Ok(call);
            }
            catch(Exception ex)
            {
                return BadRequest("Retrieve failed - Contact Tech Support");
            }
        }

        [Route("api/calls")]
        public IHttpActionResult Put(CallViewModel call)
        {
            try
            {
                int retVal = call.Update();
                switch(retVal)
                {
                    case 1:
                        return Ok("Ok! Call updated!");
                    case -1:
                        return Ok("Call not updated!");
                    case -2:
                        return Ok("Data is Stale, Call not updated");
                    default:
                        return Ok("Call not updated!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Update Failed - Contact Tech Support");
            }
        }

        [Route("api/calls")]
        public IHttpActionResult Create(CallViewModel call)
        {
            try
            {
                call.Create();
                if(call.Id != "")
                {
                    return Ok("Ok! Call Created!");
                }
                else
                {
                    return Ok("Error! Call could not be created");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Update Failed - Contact Tech Support");
            }
        }

        [HttpDelete]
        [Route("api/calls/{id}")]
        public IHttpActionResult Delete(string id)
        {
            try
            {
                CallViewModel call = new CallViewModel();
                call.Id = id;
                long delRetCall = call.Delete();

                switch(delRetCall)
                {
                    case 1:
                        return Ok("Ok! Call has been deleted!");
                    case 2:
                        return Ok("Error! Call does not Exist!");
                    default:
                        return Ok("Call not Deleted!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Update Failed - Contact Tech Support");
            }
        }
    }
}