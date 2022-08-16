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
        var imageData = image.sprite.texture.EncodeToJPG(); // texture�Ƃ����摜(.jpg)��byte�z��ɕϊ�

        customVisionResult = await DetectImageAsync(imageData, 0.8);

        // Predictions�̐擪�����O�ɏo��
        Debug.Log(customVisionResult.ToString());

        // Label�𐶐�
        var predictions = customVisionResult.Predictions;
        foreach (var prediction in predictions)
        {
            var tagName = prediction.TagName;
            var position = new Vector3(0, 0, 0.6f);// BoundingBox���g���Ăǂ̂悤�ɐݒ肵����ǂ����A�킩��Ȃ����ߌŒ�
            CreateLabel(tagName, position);
        }
    }

    void Update()
    {
        
    }

    // imageData���畨�̌��o������
    private async Task<CustomVisionResult> DetectImageAsync(byte[] imageData, double threshold)
    {
        var requestUrl = $"{resourceBaseEndpoint}/customvision/v3.0/Prediction/{projectId}/detect/iterations/{publishedName}/image";
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey); // Header��Prediction-Key��ݒ�

        HttpResponseMessage response;
        using (var content = new ByteArrayContent(imageData))
        {
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream"); // Header��Content-Type��ݒ�
            response = await client.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

        }

        var resultJson = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<CustomVisionResult>(resultJson);
        result.Predictions = result.Predictions.FindAll(p => p.Probability > threshold); // �\����臒l���傫�����m�݂̂Ƃ���
        return result;
    }

    // Label�𐶐�����
    private void CreateLabel(string tagName, Vector3 position)
    {
        var newLabel = Instantiate(labelObject, position, Quaternion.identity, transform.parent);
        var toolTip = newLabel.GetComponent<ToolTip>();
        var toolTipText = tagName;
        switch (tagName)
        {
            case "co2_sensor":
                toolTipText = "��_���Y�f�Z���T�[";
                break;
            case "gsr_sensor":
                toolTipText = "����������";
                break;
            case "ud_sensor":
                toolTipText = "�����g�����Z���T�[";
                break;
        }
        toolTip.ToolTipText = toolTipText;
        var connector = toolTip.GetComponent<ToolTipConnector>();
        connector.Target = imageObject;
        createdLabels.Add(newLabel);
    }
}
