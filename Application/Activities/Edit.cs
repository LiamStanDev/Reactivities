using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities;

public class Edit
{
    public class Command : IRequest<Result<Unit?>>
    {
        public Activity Activity { get; set; }
    }

    // Validator
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit?>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<Unit?>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var activity = await _context.Activities.FindAsync(request.Activity.Id);
            if (activity == null)
            {
                return Result<Unit?>.Success(null);
            }
            // this method is not a good way because you need to write a lot of codes.
            // activity.Title = request.Activity.Title ??= activity.Title; // user may not set this property

            _mapper.Map(request.Activity, activity); // don't forget to add service in IOC.
            var result = await _context.SaveChangesAsync() > 0;
            if (!result)
            {
                return Result<Unit?>.Failure("Failed to update the activity");
            }
            return Result<Unit?>.Success(Unit.Value);
        }
    }
}
