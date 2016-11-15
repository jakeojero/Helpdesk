using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HelpdeskWebsite.Controllers
{
    public class PDFController : ApiController
    {
        [Route("api/employeereport")]
        public IHttpActionResult GetEmployeeReport()
        {
            try
            {
                Reports.ReportPDF rep = new Reports.ReportPDF();
                rep.GenerateEmployeeReport();
                return Ok("report generated");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Error " + ex.Message);
                return BadRequest("Retrieve Failed - Couldn't generate sample report");
            }
        }

        [Route("api/callreport")]
        public IHttpActionResult GetCallReport()
        {
            try
            {
                Reports.ReportPDF rep = new Reports.ReportPDF();
                rep.GenerateCallReport();
                return Ok("report generated");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Error " + ex.Message);
                return BadRequest("Retrieve Failed - Couldn't generate sample report");
            }
        }
    }
}