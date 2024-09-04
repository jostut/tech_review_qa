using Dapper;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;
using System.Net;

namespace StargateAPI.Business.Commands
{
    public class DeleteAstronautDuty : IRequest<DeleteAstronautDutyResult>
    {
        public int Id { get; set; }
        public int PersonId { get; set; }

    }

    public class DeleteAstronautDutyPreProcessor : IRequestPreProcessor<DeleteAstronautDuty>
    {
        private readonly StargateContext _context;

        public DeleteAstronautDutyPreProcessor(StargateContext context)
        {
            _context = context;
        }

        public Task Process(DeleteAstronautDuty request, CancellationToken cancellationToken)
        {
            var verifyExists = _context.AstronautDuties.FirstOrDefault(z => z.PersonId == request.PersonId && z.Id == request.Id);

            if (verifyExists is null) throw new BadHttpRequestException("Duty does not exist.");

            return Task.CompletedTask;
        }
    }

    public class DeleteAstronautDutyHandler : IRequestHandler<DeleteAstronautDuty, DeleteAstronautDutyResult>
    {
        private readonly StargateContext _context;

        public DeleteAstronautDutyHandler(StargateContext context)
        {
            _context = context;
        }
        public async Task<DeleteAstronautDutyResult> Handle(DeleteAstronautDuty request, CancellationToken cancellationToken)
        {

            var duty = _context.AstronautDuties.FirstOrDefault(z => z.Id == request.Id);

            if(duty is not null)
            {
                _context.AstronautDuties.Remove(duty);
            }

            await _context.SaveChangesAsync();

            return new DeleteAstronautDutyResult()
            {
                Success = true,
                Message = "Successfully Deleted"
            };
        }
    }

    public class DeleteAstronautDutyResult : BaseResponse
    {

    }
}
