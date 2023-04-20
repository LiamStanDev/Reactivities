using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

// 概念：
// 使用MediatR主要是想要實現CQRS
// 所以該List被指定為Query表示不對數據庫進行改動
// Controller會Seed(new Query())，然後Query會被Handler接收到進行操作
// 所以就是讓Controller用Query與Handler進行溝通，且使用Query來存處信息
public class List
{
    public class Query : IRequest<Result<List<Activity>>> { } // Query need a return because we use the CQRS => IRequest need a <List<Activity>> type

    public class Handler : IRequestHandler<Query, Result<List<Activity>>> // 要指定回傳直
    {
        private readonly DataContext _context;

        public Handler(DataContext context) // dependency injection
        {
            _context = context;
        }

        public async Task<Result<List<Activity>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var activities = await _context.Activities.ToListAsync();
            return Result<List<Activity>>.Success(activities);
        }
    }
}
