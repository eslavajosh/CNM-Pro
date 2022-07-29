using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Organizations;
using Sabio.Models.Interfaces;
using Sabio.Models.Requests;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/organizations")]
    [ApiController]
    public class OrganizationApiController : BaseApiController
    {
        private IOrganizationService _service = null;
        private ILookUpService _lookUpService = null;
        private IAuthenticationService<int> _authService = null;
        public OrganizationApiController(IOrganizationService service, ILookUpService lookUpService
            , ILogger<OrganizationApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
            _lookUpService = lookUpService;
        }

        [HttpPost("")]
        public ActionResult<ItemResponse<int>> Create(OrganizationAddRequest model)
        {
            ObjectResult result = null;
            try
            {
                IUserAuthData userId = _authService.GetCurrentUser();
                int id = _service.Create(model, userId.Id);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };
                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Organization>> GetOrganizationById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Organization organization = _service.GetOrganizationById(id);

                if (organization == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application resource not found.");
                }

                else
                {
                    response = new ItemResponse<Organization> { Item = organization };
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

        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<Organization>>> GetOrganizationByCreatedBy(int pageIndex, int pageSize, int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Organization organization = _service.GetOrganizationByCreatedBy(pageIndex, pageSize, id);

                if (organization == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application resource not found.");
                }

                else
                {
                    response = new ItemResponse<Organization> { Item = organization };
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

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> DeleteOrganizationById(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.DeleteOrganizationById(id);
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
        public ActionResult<ItemResponse<Paged<Organization>>> GetAll(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Organization> list = _service.GetAll(pageIndex, pageSize);

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Organization>> { Item = list };
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

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(OrganizationUpdateRequest model)
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
                response = new ErrorResponse($"Exception Error: ${ex.Message}");
            }
            return StatusCode(code, response);
        }

        [HttpGet("states")]
        public ActionResult<ItemResponse<State>> GetStates()
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                List<State> state = _service.GetStates();

                if (state == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found.");
                }
                else
                {
                    response = new ItemResponse<List<State>> { Item = state };
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

        [HttpGet("types")]
        public ActionResult<ItemResponse<LookUp>> GetOrganizationTypes()
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                List<LookUp> orgType = _service.GetOrganizationTypes();

                if (orgType == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found.");
                }
                else
                {
                    response = new ItemResponse<List<LookUp>> { Item = orgType };
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
    } 
}
