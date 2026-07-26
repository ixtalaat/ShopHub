using myshop.Data.Context;
using ShopHub.Business.Interfaces.Repositories;

namespace ShopHub.Data.Repositories;

internal class UnitOfWork(
    ApplicationDbContext context, 
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IUserRepository userRepository) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;

    public IProductRepository Products { get; } = productRepository;

    public ICategoryRepository Categories { get; } = categoryRepository;
    public IUserRepository Users { get; } = userRepository;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
