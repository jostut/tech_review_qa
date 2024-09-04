using Dapper;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;
using System.Net;

namespace StargateAPI.Business.Commands
{
    public class CreateAstronautDuty : IRequest<CreateAstronautDutyResult>
    {
        public int PersonId { get; set; }

        public string Rank { get; set; }

        public string DutyTitle { get; set; }

        public DateTime DutyStartDate { get; set; }

        public DateTime DutyEndDate { get; set; }
    }

    public class CreateAstronautDutyPreProcessor : IRequestPreProcessor<CreateAstronautDuty>
    {
        private readonly StargateContext _context;

        public CreateAstronautDutyPreProcessor(StargateContext context)
        {
            _context = context;
        }

        public Task Process(CreateAstronautDuty request, CancellationToken cancellationToken)
        {
            var verifyNoOverlap = _context.AstronautDuties.FirstOrDefault(z => z.PersonId == request.PersonId &&
                (request.DutyStartDate < z.DutyEndDate && request.DutyEndDate > z.DutyStartDate));

            if (verifyNoOverlap is not null) throw new BadHttpRequestException("Duty overlaps with an existing duty");

            return Task.CompletedTask;
        }
    }

    public class CreateAstronautDutyHandler : IRequestHandler<CreateAstronautDuty, CreateAstronautDutyResult>
    {
        private readonly StargateContext _context;

        public CreateAstronautDutyHandler(StargateContext context)
        {
            _context = context;
        }
        public async Task<CreateAstronautDutyResult> Handle(CreateAstronautDuty request, CancellationToken cancellationToken)
        { 

            var newAstronautDuty = new AstronautDuty()
            {
                PersonId = request.PersonId,
                Rank = request.Rank,
                DutyTitle = request.DutyTitle,
                DutyStartDate = request.DutyStartDate.Date,
                DutyEndDate = request.DutyEndDate.Date
            };

            await _context.AstronautDuties.AddAsync(newAstronautDuty);

            await _context.SaveChangesAsync();

            return new CreateAstronautDutyResult()
            {
                Success = true,
                Message = "Successfully Created"
            };
        }
    }

    public class CreateAstronautDutyResult : BaseResponse
    {
    }
}
