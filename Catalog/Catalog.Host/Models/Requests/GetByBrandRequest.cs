namespace Catalog.Host.Models.Requests;

public class GetByBrandRequest
{
    public int PageIndex { get; set; }

    public int PageSize { get; set; }
    public string? Brand { get; set; }
}