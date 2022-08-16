using CustomVision.Value;
using Microsoft.MixedReality.Toolkit.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField] private GameObject imageObject;
    [SerializeField] private GameObject labelObject;
    [SerializeField] private string predictionKey;
    [SerializeField] private string resourceBaseEndpoint;
    [SerializeField] private string projectId;
    [SerializeField] private string publishedName;

    private CustomVisionResult customVisionResult;
    private List<GameObject> createdLabels = new List<GameObject>();

    async void Start()
    {
        var image = imageObject.GetComponent<Image>();
        var imageData = image.sprite.texture.EncodeToJPG(); // textureとした画像(.jpg)をbyte配列に変換

        customVisionResult = await DetectImageAsync(imageData, 0.8);

        // Predictionsの先頭をログに出力
        Debug.Log(customVisionResult.ToString());

        // Labelを生成
        var predictions = customVisionResult.Predictions;
        foreach (var prediction in predictions)
        {
            var tagName = prediction.TagName;
            var position = new Vector3(0, 0, 0.6f);// BoundingBoxを使ってどのように設定したら良いか、わからないため固定
            CreateLabel(tagName, position);
        }
    }

    void Update()
    {
        
    }

    // imageDataから物体検出をする
    private async Task<CustomVisionResult> DetectImageAsync(byte[] imageData, double threshold)
    {
        var requestUrl = $"{resourceBaseEndpoint}/customvision/v3.0/Prediction/{projectId}/detect/iterations/{publishedName}/image";
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey); // HeaderにPrediction-Keyを設定

        HttpResponseMessage response;
        using (var content = new ByteArrayContent(imageData))
        {
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream"); // HeaderにContent-Typeを設定
            response = await client.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

        }

        var resultJson = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<CustomVisionResult>(resultJson);
        result.Predictions = result.Predictions.FindAll(p => p.Probability > threshold); // 予測を閾値より大きいモノのみとする
        return result;
    }

    // Labelを生成する
    private void CreateLabel(string tagName, Vector3 position)
    {
        var newLabel = Instantiate(labelObject, position, Quaternion.identity, transform.parent);
        var toolTip = newLabel.GetComponent<ToolTip>();
        var toolTipText = tagName;
        switch (tagName)
        {
            case "co2_sensor":
                toolTipText = "二酸化炭素センサー";
                break;
            case "gsr_sensor":
                toolTipText = "うそ発見器";
                break;
            case "ud_sensor":
                toolTipText = "超音波距離センサー";
                break;
        }
        toolTip.ToolTipText = toolTipText;
        var connector = toolTip.GetComponent<ToolTipConnector>();
        connector.Target = imageObject;
        createdLabels.Add(newLabel);
    }
}
