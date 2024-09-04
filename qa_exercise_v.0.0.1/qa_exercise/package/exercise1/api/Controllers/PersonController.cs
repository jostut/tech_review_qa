using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;

namespace StargateAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PersonController(IMediator mediator)
        {
            _mediator = mediator;
        }



        [HttpGet]
        public async Task<IActionResult> GetPersonById()
        {
            try
            {
                var sessionCookie = this.GetSessionCookie(Request);

                if (sessionCookie == null)
                {
                    throw new BadHttpRequestException("Invalid Session");
                }

                var result = await _mediator.Send(new GetCurrentPersonById()
                {
                    Id = sessionCookie.PersonId
                });

                return this.GetResponse(result);
            }
            catch(BadHttpRequestException ex)
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