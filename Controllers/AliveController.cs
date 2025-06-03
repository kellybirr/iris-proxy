using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Web.Http;

namespace IrisProxy.Controllers
{
    [RoutePrefix("api/Alive")]
    public class AliveController : ApiController
    {
        // GET: /Alive/
        [HttpGet]
        public IHttpActionResult Index()
        {
            var results = new List<string>();
            try
            {
                AssemblyName asmName = typeof(AliveController).GetTypeInfo().Assembly.GetName();

                results.Add($"TIME: {DateTime.UtcNow:s}");
                results.Add($"MACHINE: {Environment.MachineName}");
                results.Add($"VERSION: {asmName.Version};");

                results.Insert(0, "ALL IS GOOD");
            }
            catch (Exception ex)
            {
                results.Clear();
                results.Add("NOT GOOD");
                results.Add(ex.ToString());

                ActionContext.Response.StatusCode = HttpStatusCode.ServiceUnavailable;
            }

            return Json(results);
        }

    }
}
