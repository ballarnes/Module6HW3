using Catalog.Host.Data;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogTypeRepository
{
    Task<int?> AddTypeAsync(string type);
    Task<string?> RemoveTypeAsync(int id);
    Task<string?> UpdateTypeAsync(int id, string type);
}