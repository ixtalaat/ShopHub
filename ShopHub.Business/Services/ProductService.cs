using AutoMapper;
using ShopHub.Entities.Models;
using ShopHub.Business.Dtos.Product;
using ShopHub.Business.Interfaces.Repositories;
using ShopHub.Business.Interfaces.Services;

namespace ShopHub.Business.Services;

internal class ProductService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IFileService fileService) : IProductService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IFileService _fileService = fileService;

    public async Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await _unitOfWork.Products.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<ProductDto>>(products);
    }
    public async Task<IReadOnlyList<ProductListDto>> GetAllWithCategoryAsync(CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Products.GetAllWithCategoryAsync(cancellationToken);
    }

    public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<ProductDto>(product);
    }

    public async Task CreateAsync(ProductDto productDto, CancellationToken cancellationToken = default)
    {
        var product = _mapper.Map<Product>(productDto);

        if (productDto.Image is not null)
        {
            product.ImageUrl = (await _fileService.UploadAsync(
                productDto.Image,
                "images/products",
                cancellationToken))!;
        } 
        else
        {
            product.ImageUrl = "images/products/placeholder.jpg";
        }

        

        await _unitOfWork.Products.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
    public async Task UpdateAsync(ProductDto productDto, CancellationToken cancellationToken = default)
    {
        var product = _mapper.Map<Product>(productDto);
        if (productDto.Image is not null)
        {
            await _fileService.DeleteAsync(product.ImageUrl);

            product.ImageUrl = (await _fileService.UploadAsync(
                productDto.Image,
                "images/products",
                cancellationToken))!;
        }

        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        await _fileService.DeleteAsync(product.ImageUrl);

        _unitOfWork.Products.Delete(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
