using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Newsletters;
using Sabio.Models.Requests.Newsletters;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/newsletter/subscriptions")]
    [ApiController]
    public class NewsletterSubscriptionApiController : BaseApiController
    {
        private INewsletterSubscriptionService _service = null;
        private IAuthenticationService<int> _authService = null;

        public NewsletterSubscriptionApiController(INewsletterSubscriptionService service
           , ILogger<NewsletterSubscriptionApiController> logger
           , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpPost("")]
        public ActionResult<ItemResponse<NewsletterSubscription>> Add(NewsletterSubscriptionAddRequest model)
        {

            int code = 201;
            BaseResponse response = null;

            try
            {
                _service.Add(model);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpPut("email")]
        public ActionResult<NewsletterSubscription> Update(NewsletterSubscriptionUpdateRequest model, string email)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Update(model);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<NewsletterSubscription>>> GetAllByPage(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
               Paged<NewsletterSubscription> page = _service.GetAllByPage(pageIndex, pageSize);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<NewsletterSubscription>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("")]
        public ActionResult<ItemsResponse<NewsletterSubscription>> GetAllSubscribed()
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                List<NewsletterSubscription> list = _service.GetAllSubscribed();

                if (list == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemsResponse<NewsletterSubscription> { Items = list};
                }
            }
            catch (Exception ex)
            {

                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }

            return StatusCode(iCode, response);

        }
    }
}
