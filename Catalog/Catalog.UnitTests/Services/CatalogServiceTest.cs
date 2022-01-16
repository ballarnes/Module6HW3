using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;
using Moq;

namespace Catalog.UnitTests.Services;

public class CatalogServiceTest
{
    private readonly ICatalogService _catalogService;

    private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
    private readonly Mock<ICatalogBrandRepository> _catalogBrandRepository;
    private readonly Mock<ICatalogTypeRepository> _catalogTypeRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;

    private readonly CatalogItem _testItem = new CatalogItem()
    {
        Name = "Name",
        Description = "Description",
        Price = 1000,
        AvailableStock = 100,
        CatalogBrandId = 1,
        CatalogTypeId = 1,
        PictureFileName = "1.png"
    };

    private readonly CatalogBrand _testBrand = new CatalogBrand()
    {
        Brand = "Name"
    };

    private readonly CatalogType _testType = new CatalogType()
    {
        Type = "Name"
    };

    public CatalogServiceTest()
    {
        _catalogItemRepository = new Mock<ICatalogItemRepository>();
        _catalogBrandRepository = new Mock<ICatalogBrandRepository>();
        _catalogTypeRepository = new Mock<ICatalogTypeRepository>();
        _mapper = new Mock<IMapper>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransaction()).Returns(dbContextTransaction.Object);

        _catalogService = new CatalogService(_dbContextWrapper.Object, _logger.Object, _catalogItemRepository.Object, _catalogBrandRepository.Object, _catalogTypeRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = "TestName",
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogItemSuccess = new CatalogItem()
        {
            Name = "TestName"
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = "TestName"
        };

        _catalogItemRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _catalogItemRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_Success()
    {
        // arrange
        var id = 1;
        var name = "TestName";

        var catalogItemSuccess = new CatalogItem()
        {
            Id = id,
            Name = name
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Id = id,
            Name = name
        };

        _catalogItemRepository.Setup(s => s.GetByIdAsync(
            It.Is<int>(i => i == id))).ReturnsAsync(catalogItemSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetByIdAsync(id);

        // assert
        result.Should().NotBeNull();
        result?.Name.Should().Be(name);
        result?.Id.Should().Be(id);
    }

    [Fact]
    public async Task GetByIdAsync_Failed()
    {
        // arrange
        Task<CatalogItem>? testResult = null;
        var id = 1000;

        _catalogItemRepository.Setup(s => s.GetByIdAsync(
            It.Is<int>(i => i == id))).Returns(testResult!);

        // act
        var result = await _catalogService.GetByIdAsync(id);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByBrandAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 4;
        var brand = "TestName";

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    CatalogBrand = new CatalogBrand() { Brand = brand },
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogItemSuccess = new CatalogItem()
        {
            CatalogBrand = new CatalogBrand() { Brand = brand }
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            CatalogBrand = new CatalogBrandDto() { Brand = brand }
        };

        _catalogItemRepository.Setup(s => s.GetByBrandAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            It.Is<string>(i => i == brand))).ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetByBrandAsync(testPageSize, testPageIndex, brand);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetByBrandAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;
        var brand = "TestName";

        _catalogItemRepository.Setup(s => s.GetByBrandAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            It.Is<string>(i => i == brand))).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetByBrandAsync(testPageSize, testPageIndex, brand);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByTypeAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 4;
        var type = "TestName";

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    CatalogType = new CatalogType() { Type = type },
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogItemSuccess = new CatalogItem()
        {
            CatalogType = new CatalogType() { Type = type }
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            CatalogType = new CatalogTypeDto() { Type = type }
        };

        _catalogItemRepository.Setup(s => s.GetByTypeAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            It.Is<string>(i => i == type))).ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetByTypeAsync(testPageSize, testPageIndex, type);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetByTypeAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;
        var type = "TestName";

        _catalogItemRepository.Setup(s => s.GetByTypeAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            It.Is<string>(i => i == type))).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetByTypeAsync(testPageSize, testPageIndex, type);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetBrandsAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;

        var pagingPaginatedBrandsSuccess = new PaginatedItems<CatalogBrand>()
        {
            Data = new List<CatalogBrand>()
            {
                new CatalogBrand()
                {
                    Brand = "TestName",
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogBrandSuccess = new CatalogBrand()
        {
            Brand = "TestName"
        };

        var catalogBrandDtoSuccess = new CatalogBrandDto()
        {
            Brand = "TestName"
        };

        _catalogItemRepository.Setup(s => s.GetBrandsAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).ReturnsAsync(pagingPaginatedBrandsSuccess);

        _mapper.Setup(s => s.Map<CatalogBrandDto>(
            It.Is<CatalogBrand>(i => i.Equals(catalogBrandSuccess)))).Returns(catalogBrandDtoSuccess);

        // act
        var result = await _catalogService.GetBrandsAsync(testPageSize, testPageIndex);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetBrandsAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _catalogItemRepository.Setup(s => s.GetBrandsAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItemsResponse<CatalogBrand>>)null!);

        // act
        var result = await _catalogService.GetBrandsAsync(testPageSize, testPageIndex);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTypesAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;

        var pagingPaginatedTypesSuccess = new PaginatedItems<CatalogType>()
        {
            Data = new List<CatalogType>()
            {
                new CatalogType()
                {
                    Type = "TestName",
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogTypeSuccess = new CatalogType()
        {
            Type = "TestName"
        };

        var catalogTypeDtoSuccess = new CatalogTypeDto()
        {
            Type = "TestName"
        };

        _catalogItemRepository.Setup(s => s.GetTypesAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).ReturnsAsync(pagingPaginatedTypesSuccess);

        _mapper.Setup(s => s.Map<CatalogTypeDto>(
            It.Is<CatalogType>(i => i.Equals(catalogTypeSuccess)))).Returns(catalogTypeDtoSuccess);

        // act
        var result = await _catalogService.GetTypesAsync(testPageSize, testPageIndex);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetTypesAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _catalogItemRepository.Setup(s => s.GetTypesAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItemsResponse<CatalogType>>)null!);

        // act
        var result = await _catalogService.GetTypesAsync(testPageSize, testPageIndex);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateProductAsync_Success()
    {
        // arrange
        var testResult = 1;

        _catalogItemRepository.Setup(s => s.AddItemAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.CreateProductAsync(_testItem.Name, _testItem.Description, _testItem.Price, _testItem.AvailableStock, _testItem.CatalogBrandId, _testItem.CatalogTypeId, _testItem.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task CreateProductAsync_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogItemRepository.Setup(s => s.AddItemAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.CreateProductAsync(_testItem.Name, _testItem.Description, _testItem.Price, _testItem.AvailableStock, _testItem.CatalogBrandId, _testItem.CatalogTypeId, _testItem.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task RemoveItem_Success()
    {
        // arrange
        var id = 1;
        var testResult = "Success";

        _catalogItemRepository.Setup(s => s.RemoveItemAsync(
            It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.RemoveItemAsync(id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task RemoveItem_Failed()
    {
        // arrange
        var id = 1;
        string? testResult = null;

        _catalogItemRepository.Setup(s => s.RemoveItemAsync(
            It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.RemoveItemAsync(id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateItem_Success()
    {
        // arrange
        var id = 1;
        var testResult = "Success";

        _catalogItemRepository.Setup(s => s.UpdateItemAsync(
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.UpdateItemAsync(id, _testItem.Name, _testItem.Description, _testItem.Price, _testItem.AvailableStock, _testItem.CatalogBrandId, _testItem.CatalogTypeId, _testItem.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateItem_Failed()
    {
        // arrange
        var id = 1;
        string? testResult = null;

        _catalogItemRepository.Setup(s => s.UpdateItemAsync(
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.UpdateItemAsync(id, _testItem.Name, _testItem.Description, _testItem.Price, _testItem.AvailableStock, _testItem.CatalogBrandId, _testItem.CatalogTypeId, _testItem.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task AddBrand_Success()
    {
        // arrange
        var testResult = 1;

        _catalogBrandRepository.Setup(s => s.AddBrandAsync(
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.AddBrandAsync(_testBrand.Brand);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task AddBrand_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogBrandRepository.Setup(s => s.AddBrandAsync(
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.AddBrandAsync(_testBrand.Brand);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task RemoveBrand_Success()
    {
        // arrange
        var id = 1;
        var testResult = "Success";

        _catalogBrandRepository.Setup(s => s.RemoveBrandAsync(
            It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.RemoveBrandAsync(id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task RemoveBrand_Failed()
    {
        // arrange
        var id = 1;
        string? testResult = null;

        _catalogBrandRepository.Setup(s => s.RemoveBrandAsync(
            It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.RemoveBrandAsync(id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateBrand_Success()
    {
        // arrange
        var id = 1;
        var testResult = "Success";

        _catalogBrandRepository.Setup(s => s.UpdateBrandAsync(
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.UpdateBrandAsync(id, _testBrand.Brand);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateBrand_Failed()
    {
        // arrange
        var id = 1;
        string? testResult = null;

        _catalogBrandRepository.Setup(s => s.UpdateBrandAsync(
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.UpdateBrandAsync(id, _testBrand.Brand);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task AddType_Success()
    {
        // arrange
        var testResult = 1;

        _catalogTypeRepository.Setup(s => s.AddTypeAsync(
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.AddTypeAsync(_testType.Type);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task AddType_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogTypeRepository.Setup(s => s.AddTypeAsync(
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.AddTypeAsync(_testType.Type);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task RemoveType_Success()
    {
        // arrange
        var id = 1;
        var testResult = "Success";

        _catalogTypeRepository.Setup(s => s.RemoveTypeAsync(
            It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.RemoveTypeAsync(id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task RemoveType_Failed()
    {
        // arrange
        var id = 1;
        string? testResult = null;

        _catalogTypeRepository.Setup(s => s.RemoveTypeAsync(
            It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.RemoveTypeAsync(id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateType_Success()
    {
        // arrange
        var id = 1;
        var testResult = "Success";

        _catalogTypeRepository.Setup(s => s.UpdateTypeAsync(
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.UpdateTypeAsync(id, _testType.Type);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateType_Failed()
    {
        // arrange
        var id = 1;
        string? testResult = null;

        _catalogTypeRepository.Setup(s => s.UpdateTypeAsync(
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.UpdateTypeAsync(id, _testType.Type);

        // assert
        result.Should().Be(testResult);
    }
}