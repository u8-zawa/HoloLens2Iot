using System;
using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDetector : MonoBehaviour
{
    [Header("Source")]
    [SerializeField]
    private Texture2D texture = default; // textureに指定した画像で物体検出を行う

    [Header("Setting")]
    [SerializeField]
    private Image imageObject = default;
    [SerializeField]
    private string predictionKey = default;
    [SerializeField]
    private string contentType = default;
    [SerializeField]
    private string resourceBaseEndpoint = default;
    [SerializeField]
    private string projectId = default;
    [SerializeField]
    private string publishedName = default;

    async void Start()
    {
        var data = texture.EncodeToJPG(); // textureとした画像(.jpg)をbyte配列に変換

        using (var client = new HttpClient())
        {
            var content = new ByteArrayContent(data);
            client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey); // HeaderにPrediction-Keyを設定
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType); // HeaderにContent-Typeを設定

            var result = await client.PostAsync(
                $"{resourceBaseEndpoint}/customvision/v3.0/Prediction/{projectId}/detect/iterations/{publishedName}/image",
                content);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(result.ReasonPhrase);
            }

            var body = await result.Content.ReadAsStringAsync();
            var imagePredictionResult = ImagePrediction.FromJson(body);

            // Predictionsの先頭をログに出力
            Debug.Log(imagePredictionResult.ToString());
        }

        // imageObjectにtextureを表示
        imageObject.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    void Update()
    {
        
    }
}
