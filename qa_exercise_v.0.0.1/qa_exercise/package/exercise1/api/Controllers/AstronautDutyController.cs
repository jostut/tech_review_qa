using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using StargateAPI.Business.Queries;
using StargateAPI.Business.Utilities;
using System.Net;
using System.Net.Http.Headers;


namespace StargateAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AstronautDutyController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AstronautDutyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAstronautDutiesByPersonId()
        {
            try
            {
                var sessionCookie = this.GetSessionCookie(Request);

                if (sessionCookie == null)
                {
                    throw new BadHttpRequestException("Invalid Session");
                }

                var result = await _mediator.Send(new GetAstronautDutiesByPersonId()
                {
                    Id = sessionCookie.PersonId
                });

                return this.GetResponse(result);

            }
            catch (BadHttpRequestException ex)
            {
                return this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.BadRequest
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.Unauthorized
                });
            }
            catch (Exception ex)
            {
                return this.GetResponse(new BaseResponse()
                {
                    Message = "Server error",
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> CreateAstronautDuty([FromBody] CreateAstronautDuty request)
        {
            try
            {
                var sessionCookie = this.GetSessionCookie(Request);

                if (sessionCookie == null)
                {
                    throw new BadHttpRequestException("Invalid Session");
                }

                request.PersonId = sessionCookie.PersonId;


                var result = await _mediator.Send(request);
                return this.GetResponse(result);
            }
            catch (BadHttpRequestException ex)
            {
                return this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.BadRequest
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.Unauthorized
                });
            }
            catch (Exception ex)
            {
                return this.GetResponse(new BaseResponse()
                {
                    Message = "Server Error",
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAstronautDuty([FromBody] DeleteAstronautDuty request)
        {
            try
            {
                var sessionCookie = this.GetSessionCookie(Request);

                if(sessionCookie == null)
                {
                    throw new BadHttpRequestException("Invalid Session");
                }

                request.PersonId = sessionCookie.PersonId;

                var result = await _mediator.Send(request);
                return this.GetResponse(result);
            }
            catch (BadHttpRequestException ex)
            {
                return this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.BadRequest
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.Unauthorized
                });
            }
            catch (Exception ex)
            {
                return this.GetResponse(new BaseResponse()
                {
                    Message = "Server Error",
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });
            }
        }
    }
}