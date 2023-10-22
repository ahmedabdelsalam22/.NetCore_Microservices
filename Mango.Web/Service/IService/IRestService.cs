using RestSharp;

namespace Mango.Web.RestService.IRestService
{
    public interface IRestService<T> where T : class
    {
        Task<T> UpdateAsync(string url, T data, bool withBearer = true);
        Task<RestResponse> PostAsync(string url, T data, bool withBearer = true);
        Task<List<T>> GetAsync(string url, bool withBearer = true);
        Task<T> GetByIdAsync(string url, bool withBearer = true);
        Task<RestResponse> Delete(string url, bool withBearer = true);
        Task<RestResponse> PostToDeleteCart(string url, int cartDetailsId, bool withBearer = true);
        
    }
}
