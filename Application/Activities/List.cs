using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class Query : IRequest<Result<List<ActivityDto>>> { } // Query need a return because we use the CQRS => IRequest need a <List<Activity>> type

    public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>> // 要指定回傳直
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper) // dependency injection
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<ActivityDto>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var activities = await _context.Activities
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Result<List<ActivityDto>>.Success(activities);
        }
    }
}
