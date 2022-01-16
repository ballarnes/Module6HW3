using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogItemRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize)
    {
        var totalItems = await _dbContext.CatalogItems
            .LongCountAsync();

        var itemsOnPage = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<CatalogItem> GetByIdAsync(int id)
    {
        var item = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .Where(c => c.Id == id)
            .FirstAsync();

        return new CatalogItem() { Id = item.Id, Name = item.Name, AvailableStock = item.AvailableStock, CatalogBrandId = item.CatalogBrandId, CatalogBrand = item.CatalogBrand, CatalogTypeId = item.CatalogTypeId, CatalogType = item.CatalogType, Description = item.Description, PictureFileName = item.PictureFileName, Price = item.Price };
    }

    public async Task<PaginatedItems<CatalogItem>> GetByBrandAsync(int pageIndex, int pageSize, string brand)
    {
        var totalItems = await _dbContext.CatalogItems
            .Where(i => i.CatalogBrand.Brand == brand)
            .LongCountAsync();

        var itemsOnPage = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .Where(i => i.CatalogBrand.Brand == brand)
            .OrderBy(c => c.Id)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<PaginatedItems<CatalogItem>> GetByTypeAsync(int pageIndex, int pageSize, string type)
    {
        var totalItems = await _dbContext.CatalogItems
            .Where(i => i.CatalogType.Type == type)
            .LongCountAsync();

        var itemsOnPage = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .Where(i => i.CatalogType.Type == type)
            .OrderBy(c => c.Id)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<PaginatedItems<CatalogBrand>> GetBrandsAsync(int pageIndex, int pageSize)
    {
        var totalBrands = await _dbContext.CatalogBrands
            .LongCountAsync();

        var brandsOnPage = await _dbContext.CatalogBrands
            .OrderBy(c => c.Id)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogBrand>() { TotalCount = totalBrands, Data = brandsOnPage };
    }

    public async Task<PaginatedItems<CatalogType>> GetTypesAsync(int pageIndex, int pageSize)
    {
        var totalTypes = await _dbContext.CatalogTypes
            .LongCountAsync();

        var typesOnPage = await _dbContext.CatalogTypes
            .OrderBy(c => c.Id)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogType>() { TotalCount = totalTypes, Data = typesOnPage };
    }

    public async Task<int?> AddItemAsync(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        var item = await _dbContext.CatalogItems.AddAsync(new CatalogItem
        {
            CatalogBrandId = catalogBrandId,
            CatalogTypeId = catalogTypeId,
            Description = description,
            Name = name,
            PictureFileName = pictureFileName,
            Price = price
        });

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<string?> RemoveItemAsync(int id)
    {
        var item = await _dbContext.CatalogItems.Where(c => c.Id == id).FirstAsync();
        _dbContext.CatalogItems.Remove(item);

        await _dbContext.SaveChangesAsync();

        return $"Successfully deleted. ({DateTime.UtcNow.ToString("dddd, dd MMMM yyyy HH:mm:ss")})";
    }

    public async Task<string?> UpdateItemAsync(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        var itemFromDb = await _dbContext.CatalogItems.Where(i => i.Id == id).FirstOrDefaultAsync();
        itemFromDb!.CatalogBrandId = catalogBrandId;
        itemFromDb.CatalogTypeId = catalogTypeId;
        itemFromDb.Description = description;
        itemFromDb.Name = name;
        itemFromDb.PictureFileName = pictureFileName;
        itemFromDb.Price = price;
        _dbContext.CatalogItems.Update(itemFromDb);

        await _dbContext.SaveChangesAsync();

        return $"Successfully updated. ({DateTime.UtcNow.ToString("dddd, dd MMMM yyyy HH:mm:ss")})";
    }
}