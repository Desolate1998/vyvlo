using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vyvlo.Manage.Backend.Domain.Database;

namespace Vyvlo.Manage.Backend.Domain.Repositories;

public interface IManageStoreRepository
{
    public Task<bool> CheckIfStoreExistsAsync(string name, Guid userId);
    public Task CreateStoreAsync(Store store);
    public Task<bool> UserAllowedToManageStoreAsync (Guid userId, Guid storeId);
    public Task AssignUserStoreRightsAsync(Guid userId, Guid storeId);  
    public Task<bool> CheckIfCategoryExistsAsync(string name, Guid storeId);
    public Task CreateCategoryAsync(StoreCategory category);
    public Task<List<StoreCategory>> GetAllCategoriesAsync(Guid storeId);
    public Task UpdateCategoryAsync(StoreCategory category);
    public Task<StoreCategory?> GetCategoryAsync(Guid storeId, Guid categoryId);
}
