using CustomVision.Value;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;

public class CustomVisionService : MonoBehaviour
{
    [SerializeField]
    private string endpoint;

    [SerializeField]
    private string projectId;

    [SerializeField]
    private string projectName;
    
    [SerializeField]
    private string predictionKey;

    public async Task<CustomVisionResult> DetectAsync(byte[] image, double threshold)
    {
        var requestUri = $"{endpoint}/customvision/v3.0/Prediction/{projectId}/detect/iterations/{projectName}/image";
        
        using (var client = new HttpClient())
        {
            var content = new ByteArrayContent(image);
            content.Headers.Add("Prediction-Key", predictionKey);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            var response = await client.PostAsync(requestUri, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

            var result = CustomVisionResult.FromJson(await response.Content.ReadAsStringAsync());
            result.Predictions = result.Predictions.FindAll(p => p.Probability > threshold);
            return result;
        }
    }
}