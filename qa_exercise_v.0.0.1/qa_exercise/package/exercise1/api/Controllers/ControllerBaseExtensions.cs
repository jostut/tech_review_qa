using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StargateAPI.Business.Utilities;

namespace StargateAPI.Controllers
{
    public static class ControllerBaseExtensions
    {

        public static IActionResult GetResponse(this ControllerBase controllerBase, BaseResponse response)
        {
            var httpResponse = new ObjectResult(response);
            httpResponse.StatusCode = response.ResponseCode;
            return httpResponse;
        }

        public static SessionCookie GetSessionCookie(this ControllerBase controllerBase, HttpRequest request)
        {
            var cookie = request.Cookies.FirstOrDefault();

            if(cookie.Value == null)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            var sessionValue = cookie.Value;

            string jsonCookie = HashHelper.ComputeJSONString(sessionValue);

            return JsonConvert.DeserializeObject<SessionCookie>(jsonCookie);

        }
    }
}