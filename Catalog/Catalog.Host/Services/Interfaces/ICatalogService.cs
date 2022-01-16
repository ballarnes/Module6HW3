using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogService
{
    Task<PaginatedItemsResponse<CatalogItemDto>?> GetCatalogItemsAsync(int pageSize, int pageIndex);
    Task<CatalogItemDto> GetByIdAsync(int id);
    Task<PaginatedItemsResponse<CatalogItemDto>> GetByBrandAsync(int pageSize, int pageIndex, string brand);
    Task<PaginatedItemsResponse<CatalogItemDto>> GetByTypeAsync(int pageSize, int pageIndex, string type);
    Task<PaginatedItemsResponse<CatalogBrandDto>> GetBrandsAsync(int pageSize, int pageIndex);
    Task<PaginatedItemsResponse<CatalogTypeDto>> GetTypesAsync(int pageSize, int pageIndex);
    Task<int?> CreateProductAsync(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
    Task<string?> RemoveItemAsync(int id);
    Task<string?> UpdateItemAsync(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
    Task<int?> AddBrandAsync(string brand);
    Task<string?> RemoveBrandAsync(int id);
    Task<string?> UpdateBrandAsync(int id, string brand);
    Task<int?> AddTypeAsync(string type);
    Task<string?> RemoveTypeAsync(int id);
    Task<string?> UpdateTypeAsync(int id, string type);
}