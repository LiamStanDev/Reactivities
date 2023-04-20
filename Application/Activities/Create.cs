using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities;

public class Create
{
    public class Command : IRequest<Result<Unit>> // Command don't return anything
    {
        public Activity Activity { get; set; }
    }

    // For this service
    // public class CommandValidator : AbstractValidator<Activity>
    // {
    //     public CommandValidator()
    //     {
    //         RuleFor(x => x.Title).NotEmpty();
    //     }
    // }

    // For standalone validator
    public class CommandValidator : AbstractValidator<Command> // because we want the whole activity
    {
        public CommandValidator()
        {
            RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            _context.Activities.Add(request.Activity); // AddAsyc can't use because add only use for entity tracking.
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!result)
                return Result<Unit>.Failure("Fauled to create activity");

            return Result<Unit>.Success(Unit.Value); // Unit is void
        }
    }
}
