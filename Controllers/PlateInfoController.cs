using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using IrisProxy.Models;
using IrisProxy.PlateInfo;
using Newtonsoft.Json;

namespace IrisProxy.Controllers
{
    [RoutePrefix("api/PlateInfo")]
    public class PlateInfoController : ApiController
    {
        private readonly JsonSerializerSettings _serialSettings = new JsonSerializerSettings
        {
            Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
        };

        [HttpGet, Route("")]
        public IHttpActionResult Index()
        {
            return Content(HttpStatusCode.OK, "OK");
        }

        [HttpGet, Route("ExpiredPlates")]
        public async Task<IHttpActionResult> ExpiredPlates(string token, int? gracePeriod = null)
        {
            if (!BasicAuth.Decode(ActionContext.Request, out NetworkCredential creds))
                return StatusCode(HttpStatusCode.Unauthorized);

            var iris = new T2IrisApi(creds);
            var client = iris.GetPlateInfoServiceClient();

            getExpiredPlatesResponse res = await client.getExpiredPlatesAsync(new ExpiredPlateInfoRequest()
            {
                token = token,
                gracePeriod = gracePeriod ?? 0,
                gracePeriodSpecified = gracePeriod.HasValue
            }); 
            
            return Json(res.PlateInfoResponse, _serialSettings);
        }

        [HttpGet, Route("ExpiredPlatesByGroup")]
        public async Task<IHttpActionResult> ExpiredPlatesByGroup(string token, string groupName, int? gracePeriod = null)
        {
            if (!BasicAuth.Decode(ActionContext.Request, out NetworkCredential creds))
                return StatusCode(HttpStatusCode.Unauthorized);

            var iris = new T2IrisApi(creds);
            var client = iris.GetPlateInfoServiceClient();

            getExpiredPlatesByGroupResponse res = await client.getExpiredPlatesByGroupAsync(new ExpiredPlateInfoByGroupRequest
            {
                token = token,
                gracePeriod = gracePeriod ?? 0,
                gracePeriodSpecified = gracePeriod.HasValue,
                groupName = groupName
            });

            return Json(res.PlateInfoResponse, _serialSettings);
        }

        [HttpGet, Route("ExpiredPlatesByRegion")]
        public async Task<IHttpActionResult> ExpiredPlatesByRegion(string token, string region, int? gracePeriod = null)
        {
            if (!BasicAuth.Decode(ActionContext.Request, out NetworkCredential creds))
                return StatusCode(HttpStatusCode.Unauthorized);

            var iris = new T2IrisApi(creds);
            var client = iris.GetPlateInfoServiceClient();

            getExpiredPlatesByRegionResponse res = await client.getExpiredPlatesByRegionAsync(new ExpiredPlateInfoByRegionRequest
            {
                token = token,
                gracePeriod = gracePeriod ?? 0,
                gracePeriodSpecified = gracePeriod.HasValue,
                region = region
            });

            return Json(res.PlateInfoResponse, _serialSettings);
        }

        [HttpGet, Route("Groups")]
        public async Task<IHttpActionResult> Groups(string token)
        {
            if (!BasicAuth.Decode(ActionContext.Request, out NetworkCredential creds))
                return StatusCode(HttpStatusCode.Unauthorized);

            var iris = new T2IrisApi(creds);
            var client = iris.GetPlateInfoServiceClient();

            getGroupsResponse res = await client.getGroupsAsync(new GroupRequest
            {
                token = token
            });

            return Json(res.GroupResponse, _serialSettings);
        }

        [HttpGet, Route("PlateInfo")]
        public async Task<IHttpActionResult> PlateInfo(string token, string plateNumber)
        {
            if (!BasicAuth.Decode(ActionContext.Request, out NetworkCredential creds))
                return StatusCode(HttpStatusCode.Unauthorized);

            var iris = new T2IrisApi(creds);
            var client = iris.GetPlateInfoServiceClient();

            getPlateInfoResponse res = await client.getPlateInfoAsync(new PlateInfoByPlateRequest
            {
                token = token,
                plateNumber = plateNumber
            }); 
            
            return Json(res.PlateInfoByPlateResponse, _serialSettings);
        }

        [HttpGet, Route("Regions")]
        public async Task<IHttpActionResult> Regions(string token)
        {
            if (!BasicAuth.Decode(ActionContext.Request, out NetworkCredential creds))
                return StatusCode(HttpStatusCode.Unauthorized);

            var iris = new T2IrisApi(creds);
            var client = iris.GetPlateInfoServiceClient();

            getRegionsResponse res = await client.getRegionsAsync(new RegionRequest
            {
                token = token
            });

            return Json(res.RegionResponse, _serialSettings);
        }

        [HttpGet, Route("ValidPlates")]
        public async Task<IHttpActionResult> ValidPlates(string token, int? gracePeriod = null)
        {
            if (!BasicAuth.Decode(ActionContext.Request, out NetworkCredential creds))
                return StatusCode(HttpStatusCode.Unauthorized);

            var iris = new T2IrisApi(creds);
            var client = iris.GetPlateInfoServiceClient();

            getValidPlatesResponse res = await client.getValidPlatesAsync(new PlateInfoRequest
            {
                token = token,
                gracePeriod = gracePeriod ?? 0,
                gracePeriodSpecified = gracePeriod.HasValue
            });

            return Json(res.PlateInfoResponse, _serialSettings);
        }

        [HttpGet, Route("ValidPlatesByGroup")]
        public async Task<IHttpActionResult> ValidPlatesByGroup(string token, string groupName, int? gracePeriod = null)
        {
            if (!BasicAuth.Decode(ActionContext.Request, out NetworkCredential creds))
                return StatusCode(HttpStatusCode.Unauthorized);

            var iris = new T2IrisApi(creds);
            var client = iris.GetPlateInfoServiceClient();

            getValidPlatesByGroupResponse res = await client.getValidPlatesByGroupAsync(new PlateInfoByGroupRequest
            {
                token = token,
                gracePeriod = gracePeriod ?? 0,
                gracePeriodSpecified = gracePeriod.HasValue,
                groupName = groupName
            });

            return Json(res.PlateInfoResponse, _serialSettings);
        }

        [HttpGet, Route("ValidPlatesByRegion")]
        public async Task<IHttpActionResult> ValidPlatesByRegion(string token, string region, int? gracePeriod = null)
        {
            if (!BasicAuth.Decode(ActionContext.Request, out NetworkCredential creds))
                return StatusCode(HttpStatusCode.Unauthorized);

            var iris = new T2IrisApi(creds);
            var client = iris.GetPlateInfoServiceClient();

            getValidPlatesByRegionResponse res = await client.getValidPlatesByRegionAsync(new PlateInfoByRegionRequest
            {
                token = token,
                region = region,
                gracePeriod = gracePeriod ?? 0,
                gracePeriodSpecified = gracePeriod.HasValue
            });

            return Json(res.PlateInfoResponse, _serialSettings);
        }
    }
}