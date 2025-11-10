using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FS_Dynamic.Models;
using FS_Dynamic.Services;
using Newtonsoft.Json;

namespace FS_Dynamic.Services
{
    public class ResultService
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "http://localhost/fs-dynamic-web/api/";

        public ResultService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> SaveTrainigResultsAsync(TrainingResult result)
        {
            try
            {
                var json = JsonConvert.SerializeObject(result);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(ApiBaseUrl + "wpf_save_result.php", content);
                var responseJson = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var resultResponse = JsonConvert.DeserializeObject<SaveResultResponse>(responseJson);
                    return resultResponse?.Success ?? false;
                }
                return false;

            }
            catch (Exception ex)
            { 
                return false;
            }
        }
    }
}
