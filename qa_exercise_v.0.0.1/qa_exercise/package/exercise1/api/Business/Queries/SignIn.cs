using Dapper;
using MediatR;
using StargateAPI.Business.Data;
using StargateAPI.Business.Utilities;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Queries
{
    public class SignIn : IRequest<SignInResult>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SignInHandler : IRequestHandler<SignIn, SignInResult>
    {
        public readonly StargateContext _context;
        public SignInHandler(StargateContext context)
        {
            _context = context;
        }
        public async Task<SignInResult> Handle(SignIn request, CancellationToken cancellationToken)
        {
            var result = new SignInResult();

            var query = $"SELECT b.Id FROM [Account] as a INNER JOIN [Person] as b on a.PersonId = b.Id WHERE Username = '{request.Username}' AND Password = '{request.Password}'";

            var isSuccessful = await _context.Connection.QueryAsync<int>(query);

            if (isSuccessful.Any())
            {
                var sessionObject = new SessionCookie
                {
                    PersonId = isSuccessful.First()
                };


                result.SessionCookie = HashHelper.ComputeBase64(sessionObject);
                result.Message = "Successful";
                result.ResponseCode = 200;
                result.Success = true;
            }
            else
            {
                result.Success = false;
                result.Message = "Wrong Username or Password";
                result.ResponseCode = 401;
            }



            return result;
        }
    }

    public class SignInResult : BaseResponse
    {
        public string SessionCookie { get; set; }

    }
}
