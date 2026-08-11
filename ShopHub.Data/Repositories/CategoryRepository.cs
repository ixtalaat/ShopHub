using ShopHub.Data.Context;
using ShopHub.Entities.Models;
using ShopHub.Business.Interfaces.Repositories;

namespace ShopHub.Data.Repositories;

internal class CategoryRepository(ApplicationDbContext context) : GenericRepository<Category>(context), ICategoryRepository
{

}
