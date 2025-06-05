using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using IrisProxy.Models;
using IrisProxy.TransactionData;
using Newtonsoft.Json;

namespace IrisProxy.Controllers
{
    [RoutePrefix("api/TransactionData")]
    public class TransactionDataController : ApiController
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

        [HttpGet, Route("Locations")]
        public async Task<IHttpActionResult> Locations(string token)
        {
            if (!BasicAuth.Decode(ActionContext.Request, out NetworkCredential creds))
                return StatusCode(HttpStatusCode.Unauthorized);

            var iris = new T2IrisApi(creds);
            var client = new T2IrisApi(creds).GetTransactionDataServiceClient();

            getLocationsResponse res = await client.getLocationsAsync(new LocationRequest()
            {
                token = token
            });

            return Json(res.LocationResponse, _serialSettings);
        }

        [HttpGet, Route("PaymentTypes")]
        public async Task<IHttpActionResult> PaymentTypes(string token)
        {
            if (!BasicAuth.Decode(ActionContext.Request, out NetworkCredential creds))
                return StatusCode(HttpStatusCode.Unauthorized);

            var iris = new T2IrisApi(creds);
            var client = iris.GetTransactionDataServiceClient();

            getPaymentTypesResponse res = await client.getPaymentTypesAsync(new PaymentTypeRequest()
            {
                token = token
            });

            return Json(res.PaymentTypeResponse, _serialSettings);
        }

        [HttpGet, Route("Paystations")]
        public async Task<IHttpActionResult> Paystations(string token)
        {
            if (!BasicAuth.Decode(ActionContext.Request, out NetworkCredential creds))
                return StatusCode(HttpStatusCode.Unauthorized);

            var iris = new T2IrisApi(creds);
            var client = iris.GetTransactionDataServiceClient();

            getPaystationsResponse res = await client.getPaystationsAsync(new PaystationRequest()
            {
                token = token
            });

            return Json(res.PaystationResponse, _serialSettings);
        }

        [HttpGet, Route("ProcessingStatusTypes")]
        public async Task<IHttpActionResult> ProcessingStatusTypes(string token)
        {
            if (!BasicAuth.Decode(ActionContext.Request, out NetworkCredential creds))
                return StatusCode(HttpStatusCode.Unauthorized);

            var iris = new T2IrisApi(creds);
            var client = iris.GetTransactionDataServiceClient();

            getProcessingStatusTypesResponse res = await client.getProcessingStatusTypesAsync(new ProcessingStatusTypeRequest()
            {
                token = token
            });

            return Json(res.ProcessingStatusTypeResponse, _serialSettings);
        }

        [HttpGet, Route("TransactionByUpdateDate")]
        public async Task<IHttpActionResult> TransactionByUpdateDate(string token, DateTime updateDateFrom, DateTime updateDateTo)
        {
            if (!BasicAuth.Decode(ActionContext.Request, out NetworkCredential creds))
                return StatusCode(HttpStatusCode.Unauthorized);

            var iris = new T2IrisApi(creds);
            var client = iris.GetTransactionDataServiceClient();

            getTransactionByUpdateDateResponse res = await client.getTransactionByUpdateDateAsync(new TransactionByUpdateDateRequest()
            {
                token = token,
                updateDateFrom = updateDateFrom,
                updateDateTo = updateDateTo
            });

            return Json(res.TransactionResponse, _serialSettings);
        }

        [HttpGet, Route("TransactionTypes")]
        public async Task<IHttpActionResult> TransactionTypes(string token)
        {
            if (!BasicAuth.Decode(ActionContext.Request, out NetworkCredential creds))
                return StatusCode(HttpStatusCode.Unauthorized);

            var iris = new T2IrisApi(creds);
            var client = iris.GetTransactionDataServiceClient();

            getTransactionTypesResponse res = await client.getTransactionTypesAsync(new TransactionTypeRequest()
            {
                token = token
            });

            return Json(res.TransactionTypeResponse, _serialSettings);
        }
    }
}