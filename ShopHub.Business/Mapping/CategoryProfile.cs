using AutoMapper;
using ShopHub.Entities.Models;
using ShopHub.Business.Dtos.Category;

namespace ShopHub.Business.Mapping;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
    }
}
