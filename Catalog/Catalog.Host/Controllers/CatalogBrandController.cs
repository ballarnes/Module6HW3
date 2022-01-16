using System.Net;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBrandController : ControllerBase
{
    private readonly ILogger<CatalogBrandController> _logger;
    private readonly ICatalogService _catalogService;

    public CatalogBrandController(
        ILogger<CatalogBrandController> logger,
        ICatalogService catalogService)
    {
        _logger = logger;
        _catalogService = catalogService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateResponse<int?>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddBrand(CreateBrandRequest request)
    {
        var result = await _catalogService.AddBrandAsync(request.Brand!);

        if (result == null)
        {
            return BadRequest();
        }

        return Ok(new CreateResponse<int?>() { Id = result });
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> RemoveBrand(GetByIdRequest request)
    {
        var result = await _catalogService.RemoveBrandAsync(request.Id);

        if (result == null)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateBrand(UpdateBrandRequest request)
    {
        var result = await _catalogService.UpdateBrandAsync(request.Id, request.Brand);

        if (result == null)
        {
            return BadRequest();
        }

        return Ok();
    }
}