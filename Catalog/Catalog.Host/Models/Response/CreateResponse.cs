namespace Catalog.Host.Models.Response;

public class CreateResponse<T>
{
    public T Id { get; set; } = default(T) !;
}