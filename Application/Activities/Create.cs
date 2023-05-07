using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(
                x => x.UserName == _userAccessor.GetUsername()
            );

            var attendee = new ActivityAttendee
            {
                AppUser = user,
                Activity = request.Activity,
                IsHost = true
            };

            request.Activity.Attendees.Add(attendee); // Activity Attendeees initail value is null, so need to change to empty arrray

            _context.Activities.Add(request.Activity); // AddAsyc can't use because add only use for entity tracking.

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!result)
                return Result<Unit>.Failure("Fauled to create activity");

            return Result<Unit>.Success(Unit.Value); // Unit is void
        }
    }
}
