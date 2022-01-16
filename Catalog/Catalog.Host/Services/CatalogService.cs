using AutoMapper;
using Catalog.Host.Configurations;
using Catalog.Host.Data;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogService : BaseDataService<ApplicationDbContext>, ICatalogService
{
    private readonly ICatalogItemRepository _catalogItemRepository;
    private readonly ICatalogBrandRepository _catalogBrandRepository;
    private readonly ICatalogTypeRepository _catalogTypeRepository;
    private readonly IMapper _mapper;

    public CatalogService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogItemRepository catalogItemRepository,
        ICatalogBrandRepository catalogBrandRepository,
        ICatalogTypeRepository catalogTypeRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _catalogItemRepository = catalogItemRepository;
        _catalogBrandRepository = catalogBrandRepository;
        _catalogTypeRepository = catalogTypeRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedItemsResponse<CatalogItemDto>?> GetCatalogItemsAsync(int pageSize, int pageIndex)
    {
        return await ExecuteSafe(async () =>
        {
            var result = await _catalogItemRepository.GetByPageAsync(pageIndex, pageSize);
            if (result == null)
            {
                return null;
            }

            return new PaginatedItemsResponse<CatalogItemDto>()
            {
                Count = result.TotalCount,
                Data = result.Data.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        });
    }

    public async Task<CatalogItemDto> GetByIdAsync(int id)
    {
        return await ExecuteSafe(async () =>
        {
            var result = await _catalogItemRepository.GetByIdAsync(id);
            var mapped = _mapper.Map<CatalogItemDto>(result);
            return mapped;
        });
    }

    public async Task<PaginatedItemsResponse<CatalogItemDto>> GetByBrandAsync(int pageSize, int pageIndex, string brand)
    {
        return await ExecuteSafe(async () =>
        {
            var result = await _catalogItemRepository.GetByBrandAsync(pageIndex, pageSize, brand);
            return new PaginatedItemsResponse<CatalogItemDto>()
            {
                Count = result.TotalCount,
                Data = result.Data.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        });
    }

    public async Task<PaginatedItemsResponse<CatalogItemDto>> GetByTypeAsync(int pageSize, int pageIndex, string type)
    {
        return await ExecuteSafe(async () =>
        {
            var result = await _catalogItemRepository.GetByTypeAsync(pageIndex, pageSize, type);
            return new PaginatedItemsResponse<CatalogItemDto>()
            {
                Count = result.TotalCount,
                Data = result.Data.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        });
    }

    public async Task<PaginatedItemsResponse<CatalogBrandDto>> GetBrandsAsync(int pageSize, int pageIndex)
    {
        return await ExecuteSafe(async () =>
        {
            var result = await _catalogItemRepository.GetBrandsAsync(pageIndex, pageSize);
            return new PaginatedItemsResponse<CatalogBrandDto>()
            {
                Count = result.TotalCount,
                Data = result.Data.Select(s => _mapper.Map<CatalogBrandDto>(s)).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        });
    }

    public async Task<PaginatedItemsResponse<CatalogTypeDto>> GetTypesAsync(int pageSize, int pageIndex)
    {
        return await ExecuteSafe(async () =>
        {
            var result = await _catalogItemRepository.GetTypesAsync(pageIndex, pageSize);
            return new PaginatedItemsResponse<CatalogTypeDto>()
            {
                Count = result.TotalCount,
                Data = result.Data.Select(s => _mapper.Map<CatalogTypeDto>(s)).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        });
    }

    public Task<int?> CreateProductAsync(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        return ExecuteSafe(() => _catalogItemRepository.AddItemAsync(name, description, price, availableStock, catalogBrandId, catalogTypeId, pictureFileName));
    }

    public Task<string?> RemoveItemAsync(int id)
    {
        return ExecuteSafe(() => _catalogItemRepository.RemoveItemAsync(id));
    }

    public Task<string?> UpdateItemAsync(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        return ExecuteSafe(() => _catalogItemRepository.UpdateItemAsync(id, name, description, price, availableStock, catalogBrandId, catalogTypeId, pictureFileName));
    }

    public Task<int?> AddBrandAsync(string brand)
    {
        return ExecuteSafe(() => _catalogBrandRepository.AddBrandAsync(brand));
    }

    public Task<string?> RemoveBrandAsync(int id)
    {
        return ExecuteSafe(() => _catalogBrandRepository.RemoveBrandAsync(id));
    }

    public Task<string?> UpdateBrandAsync(int id, string brand)
    {
        return ExecuteSafe(() => _catalogBrandRepository.UpdateBrandAsync(id, brand));
    }

    public Task<int?> AddTypeAsync(string type)
    {
        return ExecuteSafe(() => _catalogTypeRepository.AddTypeAsync(type));
    }

    public Task<string?> RemoveTypeAsync(int id)
    {
        return ExecuteSafe(() => _catalogTypeRepository.RemoveTypeAsync(id));
    }

    public Task<string?> UpdateTypeAsync(int id, string type)
    {
        return ExecuteSafe(() => _catalogTypeRepository.UpdateTypeAsync(id, type));
    }
}