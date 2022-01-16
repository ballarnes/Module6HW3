using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class CatalogBrandRepository : ICatalogBrandRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogBrandRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<int?> AddBrandAsync(string brand)
    {
        var newBrand = await _dbContext.CatalogBrands.AddAsync(new CatalogBrand
        {
            Brand = brand
        });

        await _dbContext.SaveChangesAsync();

        return newBrand.Entity.Id;
    }

    public async Task<string?> RemoveBrandAsync(int id)
    {
        var brand = await _dbContext.CatalogBrands.Where(c => c.Id == id).FirstOrDefaultAsync();
        _dbContext.CatalogBrands.Remove(brand!);

        await _dbContext.SaveChangesAsync();

        return $"Successfully deleted. ({DateTime.UtcNow.ToString("dddd, dd MMMM yyyy HH:mm:ss")})";
    }

    public async Task<string?> UpdateBrandAsync(int id, string brand)
    {
        var brandFromDb = await _dbContext.CatalogBrands.Where(i => i.Id == id).FirstOrDefaultAsync();
        brandFromDb!.Brand = brand;
        _dbContext.CatalogBrands.Update(brandFromDb);

        await _dbContext.SaveChangesAsync();

        return $"Successfully updated. ({DateTime.UtcNow.ToString("dddd, dd MMMM yyyy HH:mm:ss")})";
    }
}