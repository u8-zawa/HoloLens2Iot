using CustomVision;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private GameObject imageObject = default;
    [SerializeField]
    private GameObject labelObject = default;
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

    private CustomVisionResult customVisionResult;
    private List<GameObject> createdLabels = new List<GameObject>();

    async void Start()
    {
        var image = imageObject.GetComponent<Image>();
        var data = image.sprite.texture.EncodeToJPG(); // textureとした画像(.jpg)をbyte配列に変換

        // CustomVisionResultを取得する
        using (var client = new HttpClient())
        {
            var requestUri = $"{resourceBaseEndpoint}/customvision/v3.0/Prediction/{projectId}/detect/iterations/{publishedName}/image";
            var content = new ByteArrayContent(data);
            client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey); // HeaderにPrediction-Keyを設定
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType); // HeaderにContent-Typeを設定

            var result = await client.PostAsync(requestUri, content);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(result.ReasonPhrase);
            }

            var body = await result.Content.ReadAsStringAsync();
            customVisionResult = CustomVisionResult.FromJson(body);
        }

        // Predictionsの先頭をログに出力
        Debug.Log(customVisionResult.ToString());

        // Labelを生成
        var predictions = customVisionResult.predictions;
        foreach (var prediction in predictions)
        {
            // 確率が0.8以上の予測だけLabelを生成する
            if (prediction.probability > 0.8)
            {
                var tagName = prediction.tagName;
                var position = new Vector3(0, 0, 0.6f);// BoundingBoxを使ってどのように設定したら良いか、わからないため固定
                CreateLabel(tagName, position);
            }
        }
    }

    void Update()
    {
        
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
