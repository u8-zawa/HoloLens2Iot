using System;
using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDetector : MonoBehaviour
{
    [Header("Source")]
    [SerializeField]
    private Texture2D texture = default; // texture�Ɏw�肵���摜�ŕ��̌��o���s��

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
        var data = texture.EncodeToJPG(); // texture�Ƃ����摜(.jpg)��byte�z��ɕϊ�

        using (var client = new HttpClient())
        {
            var content = new ByteArrayContent(data);
            client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey); // Header��Prediction-Key��ݒ�
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType); // Header��Content-Type��ݒ�

            var result = await client.PostAsync(
                $"{resourceBaseEndpoint}/customvision/v3.0/Prediction/{projectId}/detect/iterations/{publishedName}/image",
                content);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(result.ReasonPhrase);
            }

            var body = await result.Content.ReadAsStringAsync();
            var imagePredictionResult = ImagePrediction.FromJson(body);

            // Predictions�̐擪�����O�ɏo��
            Debug.Log(imagePredictionResult.ToString());
        }

        // imageObject��texture��\��
        imageObject.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    void Update()
    {
        
    }
}
