using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Json;
using System.Threading.Tasks;
namespace WPF_Project_1
{
    internal class ChordRecognitionService
    {
        
        private static readonly HttpClient _httpClient = new HttpClient();

       
        private const string ServerUrl = "http://127.0.0.1:8000/predict";

        public async Task<ChordResponse> PredictChordAsync(float[] audioSamples)
        {
            try
            {
                var requestData = new AudioDataRequest { samples = audioSamples };
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(ServerUrl, requestData);
                response.EnsureSuccessStatusCode();

        
                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ChordResponse>(json);
            }
            catch
            {
            
                return new ChordResponse { chord = "Помилка", confidence = 0 };
            }
        }
    }

    public class ChordResponse
    {
        public string chord { get; set; }
        public double confidence { get; set; }
    }
}
