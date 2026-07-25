using AutoMapper;
using myshop.Entities.Models;
using ShopHub.Business.Dtos.Product;
using ShopHub.Business.Interfaces.Repositories;
using ShopHub.Business.Interfaces.Services;

namespace ShopHub.Business.Services;

internal class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

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
        await _unitOfWork.Products.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
    public async Task UpdateAsync(ProductDto productDto, CancellationToken cancellationToken = default)
    {
        var product = _mapper.Map<Product>(productDto);
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

        _unitOfWork.Products.Delete(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
