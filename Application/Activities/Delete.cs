using MediatR;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Persistence;

namespace Application.Core;

public class Delete
{
    public class Command : IRequest
    {
        public Guid Id;
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.Id); // FindAsync will return null if not exist.

            _context.Remove(activity); // add activity with a tag of "Deleted"

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
