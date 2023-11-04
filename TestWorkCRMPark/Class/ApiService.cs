using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using TestWorkCRMPark.Interface;

namespace TestWorkCRMPark.Class
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _apiKey;

        public ApiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _baseUrl = config["ApiSettings:BaseUrl"];
            _apiKey = config["ApiSettings:ApiKey"];
        }

        public async Task<string> GetCompaniesByINN(string innList)
        {
            var url = $"{_baseUrl}?q={innList}&key={_apiKey}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(responseContent);

                var formattedResponse = "Результаты запроса:\n";
                foreach (var item in jObject["items"])
                {
                    if (item["ЮЛ"] != null)
                    {
                        var ulObject = item["ЮЛ"];
                        if (ulObject["ИНН"] != null)
                            formattedResponse += $"ИНН: {ulObject["ИНН"]}\n";
                        if (ulObject["НаимСокрЮЛ"] != null)
                            formattedResponse += $"Наименование: {ulObject["НаимСокрЮЛ"]}\n";
                        if (ulObject["АдресПолн"] != null)
                            formattedResponse += $"Адрес: {ulObject["АдресПолн"]}\n\n";
                    }
                }
                return formattedResponse;
            }
            else
            {
                return "Произошла ошибка при выполнении запроса к API.";
            }
        }
    }

}
