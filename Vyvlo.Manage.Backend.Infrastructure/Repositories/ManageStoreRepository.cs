using Vyvlo.Manage.Backend.Domain.Database;
using Vyvlo.Manage.Backend.Domain.Repositories;
using Vyvlo.Manage.Backend.Infrastructure.Cassandra;

namespace Vyvlo.Manage.Backend.Infrastructure.Repositories;

public class ManageStoreRepository(CassandraDB db) : IManageStoreRepository
{
    async Task IManageStoreRepository.AssignUserStoreRightsAsync(Guid userId, Guid storeId)
    {
        var session = db.GetSession();
        var statement = (await session.PrepareAsync("insert into user_store_management_rights(user_id, store_id) Values(?,?)"))
                                      .Bind(userId, storeId);
        await session.ExecuteAsync(statement);
    }

    async Task<bool> IManageStoreRepository.UserAllowedToManageStoreAsync(Guid userId, Guid storeId)
    {
        var session = db.GetSession();
        var statement = (await session.PrepareAsync("select user_id from user_store_management_rights where user_id =? and store_id =? limit 1"))
                                      .Bind(userId, storeId);
        var result = await session.ExecuteAsync(statement);
        return result.Any();
    }

    async Task<bool> IManageStoreRepository.CheckIfStoreExistsAsync(string name, Guid userId)
    {
        var session = db.GetSession();
        var statement = (await session.PrepareAsync("select store_id  from user_owned_stores where user_id =? and name =? limit 1"))
                                      .Bind(userId, name);

        var result = await session.ExecuteAsync(statement);
        return result.Any();
    }
    
    async Task IManageStoreRepository.CreateStoreAsync(Store store)
    {
        var session = db.GetSession();
        var statement = (await session.PrepareAsync("INSERT INTO stores(store_id, owner_id, description, name) VALUES(?,?,?,?)"))
                                      .Bind(store.StoreId, store.StoreOwnerId, store.Description, store.Name);
        await session.ExecuteAsync(statement);

        statement = (await session.PrepareAsync("INSERT INTO user_owned_stores(user_id, store_id, name) VALUES(?,?,?)"))
                                  .Bind(store.StoreOwnerId, store.StoreId, store.Name);
        await session.ExecuteAsync(statement);
    }

    async Task IManageStoreRepository.CreateCategoryAsync(StoreCategory category)
    {
        var session = db.GetSession();
        var statement = (await session.PrepareAsync("INSERT INTO categories(category_id, store_id, name, description, meta_tags) VALUES(?,?,?,?,?)"))
                                      .Bind(category.CategoryId, category.StoreId, category.Name, category.Description, category.MetaTags);
        await session.ExecuteAsync(statement);

        statement = (await session.PrepareAsync("INSERT INTO store_categories(name, store_id, category_id) VALUES(?,?,?)"))
                                  .Bind(category.Name, category.StoreId, category.CategoryId);
        await session.ExecuteAsync(statement);
    }

    async Task<List<StoreCategory>> IManageStoreRepository.GetAllCategoriesAsync(Guid storeId)
    {
        var session = db.GetSession();
        
        var statement = (await session.PrepareAsync("select category_id, name, description, meta_tags from categories where store_id =?"))
                                      .Bind(storeId);

        var result = await session.ExecuteAsync(statement);

        return result.Select(row => new StoreCategory()
        {
            CategoryId = row.GetValue<Guid>("category_id"),
            Name = row.GetValue<string>("name"),
            Description = row.GetValue<string>("description"),
            MetaTags = row.GetValue<HashSet<string>>("meta_tags")
        }).ToList(); 
    }

    async Task IManageStoreRepository.UpdateCategoryAsync(StoreCategory category)
    {
        var session = db.GetSession();
        var statement = (await session.PrepareAsync("UPDATE categories SET name =?, description =?, meta_tags =? WHERE category_id =? AND store_id=? IF EXISTS"))
                                      .Bind(category.Name, category.Description, category.MetaTags, category.CategoryId, category.StoreId);
        await session.ExecuteAsync(statement);

        statement = (await session.PrepareAsync("UPDATE store_categories SET name =? WHERE category_id =? AND store_id=? IF EXISTS"))
                                  .Bind(category.Name, category.CategoryId, category.StoreId);
        await session.ExecuteAsync(statement);
    }

    async Task<bool> IManageStoreRepository.CheckIfCategoryExistsAsync(string name, Guid storeId)
    {
        var session = db.GetSession();
        var statement = (await session.PrepareAsync("select category_id from store_categories where store_id =? and name =? limit 1"))
                                      .Bind(storeId, name);
        var result = await session.ExecuteAsync(statement);
        return result.Any();
    }

    async Task<StoreCategory?> IManageStoreRepository.GetCategoryAsync(Guid storeId, Guid categoryId)
    {
        var session = db.GetSession();
        var statement = (await session.PrepareAsync("select name, description, meta_tags from categories where store_id =? and category_id =? limit 1"))
                                      .Bind(storeId, categoryId);
        var result = await session.ExecuteAsync(statement);
        var row = result.FirstOrDefault();
        return row is null ? null : new StoreCategory()
        {
            CategoryId = categoryId,
            Name = row.GetValue<string>("name"),
            Description = row.GetValue<string>("description"),
            MetaTags = row.GetValue<HashSet<string>>("meta_tags")
        };
    }
}
