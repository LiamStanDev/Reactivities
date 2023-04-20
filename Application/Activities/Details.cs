using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities;

public class Details
{
    // 這個Query有包含請求
    public class Query : IRequest<Result<Activity>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<Activity>>
    {
        private DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Activity>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var activity = await _context.Activities.FindAsync(request.Id);

            return Result<Activity>.Success(activity);
        }
    }
}
