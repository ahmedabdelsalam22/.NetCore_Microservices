using Mango.Web.Models.DTOS;
using Mango.Web.Service.IService;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResponseDTO?> SendAsync(RequestDTO requestDTO)
        {
            try
            {

                HttpClient client = _httpClientFactory.CreateClient("MangoAPI");

                // for request 
                HttpRequestMessage message = new HttpRequestMessage();

                message.Headers.Add("Accept", "application/json");

                message.RequestUri = new Uri(requestDTO.Url);

                if (requestDTO.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDTO.Data), Encoding.UTF8, "application/json");
                }

                switch (requestDTO.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;

                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                // for responseMessage 

                HttpResponseMessage responseMessage = null;

                responseMessage = await client.SendAsync(message);

                switch (responseMessage.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new ResponseDTO() { IsSuccess = false, ErrorMessage = "Not found" };
                    case HttpStatusCode.Forbidden:
                        return new ResponseDTO() { IsSuccess = false, ErrorMessage = "Forbidden" };
                    case HttpStatusCode.Unauthorized:
                        return new ResponseDTO() { IsSuccess = false, ErrorMessage = "Unauthorized" };
                    case HttpStatusCode.InternalServerError:
                        return new ResponseDTO() { IsSuccess = false, ErrorMessage = "InternalServerError" };

                    default:
                        var apiContent = await responseMessage.Content.ReadAsStringAsync();
                        ResponseDTO responseDto = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);

                        //so 
                        return new ResponseDTO()
                        {
                            IsSuccess = responseDto!.IsSuccess,
                            Result = responseDto.Result,
                            ErrorMessage = responseDto.ErrorMessage,
                        };
                }


            }
            catch (Exception ex)
            {
                var dto = new ResponseDTO()
                {
                    IsSuccess = false,
                    Result = null,
                    ErrorMessage = ex.Message,
                };
                return dto;
            }
        }
    }
}
