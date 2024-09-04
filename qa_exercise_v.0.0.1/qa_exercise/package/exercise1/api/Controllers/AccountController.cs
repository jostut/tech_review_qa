using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Queries;
using StargateAPI.Business.Utilities;
using System.Net;

namespace StargateAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest sirRequest)
        {
            try
            {
                var result = await _mediator.Send(new SignIn()
                {
                    Username = sirRequest.Username,
                    Password = HashHelper.ComputeHash(sirRequest.Password)
                });

                if (result.Success)
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddDays(1)
                    };

                    // Add the cookie to the response
                    Response.Cookies.Append("ACTS_Session", result.SessionCookie, cookieOptions);
                    return this.GetResponse(new BaseResponse()
                    {
                        ResponseCode = result.ResponseCode,
                        Success = result.Success,
                        Message = result.Message
                    });

                }
                else
                {
                    return this.GetResponse(result);
                }
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
            catch(UnauthorizedAccessException ex)
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
                    ResponseCode = (int)HttpStatusCode.InternalServerError,
                    Success = false,
                    Message = "Server Error"
                });
            }
            


        }

        [HttpGet]
        [Route("signout")]
        public async Task<IActionResult> SignOut()
        {
            try
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddDays(-1)
                };

                // Add the cookie to the response
                Response.Cookies.Append("ACTS_Session", "", cookieOptions);
                return this.GetResponse(new BaseResponse()
                {
                    ResponseCode = 200,
                    Success = true,
                    Message = "Successful"
                });
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
            catch (Exception ex)
            {
                return this.GetResponse(new BaseResponse()
                {
                    ResponseCode = (int)HttpStatusCode.InternalServerError,
                    Success = false,
                    Message = "Server Error"
                });
            }
            
        }
    }
}