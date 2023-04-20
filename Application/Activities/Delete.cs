using MediatR;
using Persistence;

namespace Application.Core;

public class Delete
{
    public class Command : IRequest<Result<Unit?>>
    {
        public Guid Id;
    }

    public class Handler : IRequestHandler<Command, Result<Unit?>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit?>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var activity = await _context.Activities.FindAsync(request.Id); // FindAsync will return null if not exist.
            // if (activity == null)
            // {
            //     return Result<Unit?>.Success(null); // becasue value tyep like Unit can't assign null, so I change Unit to nullable type Unit?
            // }
            _context.Remove(activity); // add activity with a tag of "Deleted"

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return Result<Unit?>.Failure("Failed to delete the activity");
            }

            return Result<Unit?>.Success(Unit.Value);
        }
    }
}
