using AutoMapper;
using Domain;

namespace Application.Core;

public class MappingProfiles : Profile // this is from Auto Mapper Dependency injection
{
    public MappingProfiles()
    {
        CreateMap<Activity, Activity>();
    }
}
