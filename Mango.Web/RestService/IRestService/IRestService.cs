namespace Mango.Web.RestService.IRestService
{
    public interface IRestService<T> where T : class
    {
        Task<T> UpdateAsync(string url, T data);
        Task PostAsync(string url, T data);
        Task<List<T>> GetAsync(string url);
        Task Delete(string url);
    }
}
