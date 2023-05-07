using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class UpdateAttendance
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            this._context = context;
            this._userAccessor = userAccessor;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            // 找到database中的activity並要包含關聯的內容
            var activity = await _context.Activities
                .Include(a => a.Attendees)
                .ThenInclude(a => a.AppUser)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (activity == null)
            {
                return null;
            }

            // 從HttpContext中取得User名稱，然後使用找到對應的User
            var user = await _context.Users.FirstOrDefaultAsync(
                x => x.UserName == _userAccessor.GetUsername()
            );

            if (user == null)
            {
                return null;
            }

            var hostUsername = activity.Attendees.FirstOrDefault(x => x.IsHost)?.AppUser?.UserName;

            var attendance = activity.Attendees.FirstOrDefault(
                x => x.AppUser.UserName == user.UserName
            );

            if (attendance != null && hostUsername == user.UserName)
            {
                activity.IsCancelled = !activity.IsCancelled;
            }

            if (attendance != null && hostUsername != user.UserName)
            {
                activity.Attendees.Remove(attendance);
            }

            if (attendance == null)
            {
                attendance = new ActivityAttendee
                {
                    AppUser = user,
                    Activity = activity,
                    IsHost = false,
                };
                activity.Attendees.Add(attendance);
            }

            var result = await _context.SaveChangesAsync() > 0;
            return result
                ? Result<Unit>.Success(Unit.Value)
                : Result<Unit>.Failure("Problem updating attendance");
        }
    }
}
