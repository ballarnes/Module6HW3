using Catalog.Host.Data;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogItemRepository
{
    Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize);
    Task<CatalogItem> GetByIdAsync(int id);
    Task<PaginatedItems<CatalogItem>> GetByBrandAsync(int pageIndex, int pageSize, string brand);
    Task<PaginatedItems<CatalogItem>> GetByTypeAsync(int pageIndex, int pageSize, string type);
    Task<PaginatedItems<CatalogBrand>> GetBrandsAsync(int pageIndex, int pageSize);
    Task<PaginatedItems<CatalogType>> GetTypesAsync(int pageIndex, int pageSize);
    Task<int?> AddItemAsync(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
    Task<string?> RemoveItemAsync(int id);
    Task<string?> UpdateItemAsync(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
}