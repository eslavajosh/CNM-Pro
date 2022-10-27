using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Requests.ContactUs;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Threading.Tasks;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/contact")]
    [ApiController]
    public class ContactUsController : BaseApiController
    {
        private IContactUsService _service = null;

        public ContactUsController(IContactUsService service, ILogger<PingApiController> logger) : base(logger)

        {
            _service = service;
         
        }

        [HttpPost()]
        public ActionResult<SuccessResponse> SendContactUsEmail(ContactUsAddRequest model)
        {


            int code = 200;
            BaseResponse response;

            try
            {

                _service.ContactUsEmail(model);
                response = new SuccessResponse();

            }

            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
                code = 500;
            }
            return StatusCode(code, response);
        }

    }
}
