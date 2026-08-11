using AutoMapper;
using ShopHub.Entities.Models;
using ShopHub.Business.Dtos.Product;

namespace ShopHub.Business.Mapping;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}
