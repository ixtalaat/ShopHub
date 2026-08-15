using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using ShopHub.Entities.Models;
using ShopHub.Business.Dtos.Category;
using ShopHub.Business.Interfaces.Repositories;
using ShopHub.Business.Interfaces.Services;

namespace ShopHub.Business.Services;

internal class CategoryService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IMemoryCache cache) : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;

    private const string CategoriesCacheKey = "Categories_All";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(CategoriesCacheKey, out IReadOnlyList<CategoryDto>? cachedCategories)
            && cachedCategories is not null)
        {
            return cachedCategories;
        }

        var categories = await _unitOfWork.Categories.GetAllAsync(cancellationToken);
        var categoryDtos = _mapper.Map<IReadOnlyList<CategoryDto>>(categories);

        _cache.Set(CategoriesCacheKey, categoryDtos, CacheDuration);

        return categoryDtos;
    }

    public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var category =  await _unitOfWork.Categories.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<CategoryDto>(category);
    }
    public async Task CreateAsync(CategoryDto categoryDto, CancellationToken cancellationToken = default)
    {
        var category = _mapper.Map<Category>(categoryDto);
        await _unitOfWork.Categories.AddAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _cache.Remove(CategoriesCacheKey);
    }
    public async Task UpdateAsync(CategoryDto categoryDto, CancellationToken cancellationToken = default)
    {
        var category = _mapper.Map<Category>(categoryDto);
        _unitOfWork.Categories.Update(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _cache.Remove(CategoriesCacheKey);
    }
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id, cancellationToken);
        if (category is null)
        {
            throw new ArgumentNullException(nameof(category));
        }
        _unitOfWork.Categories.Delete(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _cache.Remove(CategoriesCacheKey);
    }
}
