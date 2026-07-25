using AutoMapper;
using myshop.Entities.Models;
using ShopHub.Business.Dtos.Category;
using ShopHub.Business.Interfaces.Repositories;
using ShopHub.Business.Interfaces.Services;

namespace ShopHub.Business.Services;

internal class CategoryService(IUnitOfWork unitOfWork, IMapper mapper) : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _unitOfWork.Categories.GetAllAsync(cancellationToken);

        return _mapper.Map<IReadOnlyList<CategoryDto>>(categories);
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
    }
    public async Task UpdateAsync(CategoryDto categoryDto, CancellationToken cancellationToken = default)
    {
        var category = _mapper.Map<Category>(categoryDto);
        _unitOfWork.Categories.Update(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
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
    }
}
