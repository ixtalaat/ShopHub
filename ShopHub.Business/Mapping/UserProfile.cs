using AutoMapper;
using myshop.Entities.Models;
using ShopHub.Business.Dtos.User;

namespace ShopHub.Business.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ApplicationUser, UserDto>()
            .ForMember(
            dest => dest.IsLocked,
        opt => opt.MapFrom(src =>
                src.LockoutEnd.HasValue &&
                src.LockoutEnd > DateTimeOffset.UtcNow))
            .ReverseMap();
    }
}
