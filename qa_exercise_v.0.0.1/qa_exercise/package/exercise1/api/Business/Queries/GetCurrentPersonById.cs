using Dapper;
using MediatR;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Queries
{
    public class GetCurrentPersonById : IRequest<GetCurrentPersonByIdResult>
    {
        public required int Id { get; set; } = 0;
    }

    public class GetCurrentPersonByIdHandler : IRequestHandler<GetCurrentPersonById, GetCurrentPersonByIdResult>
    {
        private readonly StargateContext _context;
        public GetCurrentPersonByIdHandler(StargateContext context)
        {
            _context = context;
        }

        public async Task<GetCurrentPersonByIdResult> Handle(GetCurrentPersonById request, CancellationToken cancellationToken)
        {
            var result = new GetCurrentPersonByIdResult();

            var query = $"SELECT Id, Name FROM [Person] WHERE '{request.Id}' = Id";

            var person = await _context.Connection.QueryAsync<Person>(query);

            result.Person = person.FirstOrDefault();

            return result;
        }
    }

    public class GetCurrentPersonByIdResult : BaseResponse
    {
        public Person Person { get; set; }
    }
}
