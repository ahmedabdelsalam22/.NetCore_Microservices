namespace Mango.Services.ShoppingCartAPI.Services.IServices
{
    public interface IRestService<T> where T : class
    {
        Task<List<T>> GetAllAsync(string url);
    }
}
