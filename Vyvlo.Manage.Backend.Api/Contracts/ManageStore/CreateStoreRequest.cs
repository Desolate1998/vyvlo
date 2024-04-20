namespace Vyvlo.Manage.Backend.Api.Contracts.ManageStore;

public class CreateStoreRequest
{
    public string Name { get; set; }
    public string Description  { get; set; }
    public IFormFile StoreImage { get; set; }
}
