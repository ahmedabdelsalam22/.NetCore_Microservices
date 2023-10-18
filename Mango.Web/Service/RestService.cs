using RestSharp;
using System.Text.Json.Nodes;
using System.Text.Json;
using Mango.Web.Utility;
using Mango.Web.RestService.IRestService;
using RestSharp.Authenticators;

namespace RestCharpCourse.Services
{
    public class RestService<T> : IRestService<T> where T : class
    {
        private readonly RestClient _restClient;

        public RestService()
        {
            _restClient = new RestClient($"{SD.CouponAPIBase}");
        }



        public async Task Delete(string url)
        {
            var request = new RestRequest(url, Method.Delete);

            //request.AddHeader("Authorization", $"Bearer {token}"); // read token from method parameter

            // var response = await _restClient.DeleteAsync(request);
            var response = await _restClient.ExecuteAsync(request); // "ExecuteAsync" handling error default if occured

            if (!response.IsSuccessful)
            {
                Console.WriteLine($"ERROR: {response.ErrorException?.Message}");
            }
        }

        public async Task<List<T>> GetAsync(string url)
        {

            var authenticator = new JwtAuthenticator(pureToken);



            var request = new RestRequest(url, Method.Get);

             
           // request.AddHeader("Authorization", $"Bearer {token}");


            //// one way 
            var response = await _restClient.ExecuteGetAsync<List<T>>(request);

            if (response.Data == null)
            {
                Console.WriteLine($"ERROR: {response.ErrorException?.Message}");
            }

             return response.Data!;
        }
        public async Task PostAsync(string url, T data)
        {
            var request = new RestRequest(url, Method.Post);

            request.AddJsonBody(data);

            request.AddHeader("Accept", "application/json");
            // request.AddHeader("Authorization", $"Bearer {token}");

            await _restClient.ExecutePostAsync(request);
        }

        public async Task<T> UpdateAsync(string url, T data)
        {
            var request = new RestRequest(url, Method.Put);

            request.AddJsonBody(data);
            request.AddHeader("Accept", "application/json");
            // request.AddHeader("Authorization", $"Bearer {token}");

            var response = await _restClient.ExecutePutAsync<T>(request);

            if (!response.IsSuccessful)
            {
                Console.WriteLine($"ERROR: {response.ErrorException?.Message}");
            }


            return response.Data!;
        }
    }
}
