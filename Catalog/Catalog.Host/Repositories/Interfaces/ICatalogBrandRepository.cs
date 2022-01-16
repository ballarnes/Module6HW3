using Catalog.Host.Data;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogBrandRepository
{
    Task<int?> AddBrandAsync(string brand);
    Task<string?> RemoveBrandAsync(int id);
    Task<string?> UpdateBrandAsync(int id, string brand);
}