using Application.Activities;
using Application.Profiles;
using AutoMapper;
using Domain;

namespace Application.Core;

public class MappingProfiles : Profile // this is from Auto Mapper Dependency injection
{
    public MappingProfiles()
    {
        CreateMap<Activity, Activity>();
        CreateMap<Activity, ActivityDto>()
            .ForMember(
                d => d.HostUsername, // destination member
                o =>
                    o.MapFrom( // mapping option
                        a =>
                            a.Attendees // get ICollection<ActivityAttendee>
                                .FirstOrDefault(x => x.IsHost)
                                .AppUser.UserName
                    )
            );
        CreateMap<ActivityAttendee, UserProfile>()
            .ForMember(u => u.DisplayName, o => o.MapFrom(aa => aa.AppUser.DisplayName))
            .ForMember(u => u.UserName, o => o.MapFrom(aa => aa.AppUser.UserName))
            .ForMember(u => u.Bio, o => o.MapFrom(aa => aa.AppUser.Bio));
    }
}
