using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Activities;

// 概念：
// 使用MediatR主要是想要實現CQRS
// 所以該List被指定為Query表示不對數據庫進行改動
// Controller會Seed(new Query())，然後Query會被Handler接收到進行操作
// 所以就是讓Controller用Query與Handler進行溝通，且使用Query來存處信息
public class List
{
    public class Query : IRequest<List<Activity>> { } // Query need a return because we use the CQRS => IRequest need a <List<Activity>> type

    public class Handler : IRequestHandler<Query, List<Activity>> // 要指定回傳直
    {
        private readonly DataContext _context;

        public Handler(DataContext context, ILogger<List> logger) // dependency injection
        {
            _context = context;
        }

        public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
        {
            // cancellation token is used when user no longer what to want and cancel the HTTP request
            // the cancellation token will get the information from API controller
            // we need to pass the token from API controller by Send method to Handler.
            return await _context.Activities.ToListAsync(cancellationToken);
        }
    }
}
