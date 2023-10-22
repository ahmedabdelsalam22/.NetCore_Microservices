using RestSharp;
using System.Text.Json.Nodes;
using System.Text.Json;
using Mango.Web.Utility;
using Mango.Web.RestService.IRestService;
using RestSharp.Authenticators;
using Mango.Web.Service.IService;

namespace RestCharpCourse.Services
{
    public class RestService<T> : IRestService<T> where T : class
    {
        private readonly RestClient _restClient;
        private readonly ITokenProvider _tokenProvider;

        public RestService(ITokenProvider tokenProvider)
        {
            _restClient = new RestClient();
            _tokenProvider = tokenProvider;
        }


        public async Task<RestResponse> Delete(string url,bool withBearer = true)
        {
            var request = new RestRequest(url, Method.Delete);

            if (withBearer)
            {
                request.AddHeader("Authorization", $"Bearer {_tokenProvider.GetToken()}"); // read token from method parameter
            }

            // var response = await _restClient.DeleteAsync(request);
            var response = await _restClient.ExecuteAsync(request); // "ExecuteAsync" handling error default if occured

            if (!response.IsSuccessful)
            {
                Console.WriteLine($"ERROR: {response.ErrorException?.Message}");
            }
            return response;
        }

        public async Task<List<T>> GetAsync(string url, bool withBearer = true)
        {

            var request = new RestRequest(url, Method.Get);


            if (withBearer)
            {
                request.AddHeader("Authorization", $"Bearer {_tokenProvider.GetToken()}"); // read token from method parameter
            }



            //// one way 
            var response = await _restClient.ExecuteGetAsync<List<T>>(request);

            if (response.Data == null)
            {
                Console.WriteLine($"ERROR: {response.ErrorException?.Message}");
            }

             return response.Data!;
        }

        public async Task<T> GetByIdAsync(string url, bool withBearer = true)
        {
            var request = new RestRequest(url, Method.Get);


            if (withBearer)
            {
                request.AddHeader("Authorization", $"Bearer {_tokenProvider.GetToken()}"); // read token from method parameter
            }



            //// one way 
            var response = await _restClient.ExecuteGetAsync<T>(request);

            if (response.Data == null)
            {
                Console.WriteLine($"ERROR: {response.ErrorException?.Message}");
            }

            return response.Data!;
        }

        public async Task<RestResponse> PostAsync(string url, T data, bool withBearer = true)
        {
            var request = new RestRequest(url, Method.Post);

            request.AddJsonBody(data);

            request.AddHeader("Accept", "application/json");
            if (withBearer)
            {
                request.AddHeader("Authorization", $"Bearer {_tokenProvider.GetToken()}"); // read token from method parameter
            }

           return await _restClient.ExecutePostAsync(request);
        }

        public async Task<RestResponse> PostToDeleteCart(string url, int cartDetailsId, bool withBearer = true)
        {
            var request = new RestRequest(url, Method.Post);

            request.AddBody(cartDetailsId);

            request.AddHeader("Accept", "application/json");
            if (withBearer)
            {
                request.AddHeader("Authorization", $"Bearer {_tokenProvider.GetToken()}"); // read token from method parameter
            }

            return await _restClient.ExecutePostAsync(request);
        }

        public async Task<T> UpdateAsync(string url, T data, bool withBearer = true)
        {
            var request = new RestRequest(url, Method.Put);

            request.AddJsonBody(data);
            request.AddHeader("Accept", "application/json");

            if (withBearer)
            {
                request.AddHeader("Authorization", $"Bearer {_tokenProvider.GetToken()}"); // read token from method parameter
            }


            var response = await _restClient.ExecutePutAsync<T>(request);

            if (!response.IsSuccessful)
            {
                Console.WriteLine($"ERROR: {response.ErrorException?.Message}");
            }


            return response.Data!;
        }
    }
}
