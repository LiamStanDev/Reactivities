using Domain;
using MediatR;
using Persistence;

namespace Application.Activities;

public class Create
{
    public class Command : IRequest // Command don't return anything
    {
        public Activity Activity { get; set; }
    }
    public class Handler : IRequestHandler<Command>
    {
        private DataContext _context;
        public Handler(DataContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            _context.Activities.Add(request.Activity); // AddAsyc can't use because add only use for entity tracking.
            await _context.SaveChangesAsync(cancellationToken); // this is the actual command to save in database

            return Unit.Value; // Unit is void
        }
    }
}
