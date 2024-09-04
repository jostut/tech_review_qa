using Dapper;
using MediatR;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Queries
{
    public class GetAstronautDutiesByPersonId : IRequest<GetAstronautDutiesByPersonIdResult>
    {
        public int Id { get; set; } = 0;
    }

    public class GetAstronautDutiesByPersonIdHandler : IRequestHandler<GetAstronautDutiesByPersonId, GetAstronautDutiesByPersonIdResult>
    {
        private readonly StargateContext _context;

        public GetAstronautDutiesByPersonIdHandler(StargateContext context)
        {
            _context = context;
        }

        public async Task<GetAstronautDutiesByPersonIdResult> Handle(GetAstronautDutiesByPersonId request, CancellationToken cancellationToken)
        {

            var result = new GetAstronautDutiesByPersonIdResult();

            var query = $"SELECT * FROM [AstronautDuty] WHERE {request.Id} = PersonId Order By DutyStartDate Desc";

            var duties = await _context.Connection.QueryAsync<AstronautDuty>(query);

            result.AstronautDuties = duties.ToList();

            return result;

        }
    }

    public class GetAstronautDutiesByPersonIdResult : BaseResponse
    {
        public List<AstronautDuty> AstronautDuties { get; set; } = new List<AstronautDuty>();
    }
}
