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
        var data = image.sprite.texture.EncodeToJPG(); // texture�Ƃ����摜(.jpg)��byte�z��ɕϊ�

        // CustomVisionResult���擾����
        using (var client = new HttpClient())
        {
            var requestUri = $"{resourceBaseEndpoint}/customvision/v3.0/Prediction/{projectId}/detect/iterations/{publishedName}/image";
            var content = new ByteArrayContent(data);
            client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey); // Header��Prediction-Key��ݒ�
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType); // Header��Content-Type��ݒ�

            var result = await client.PostAsync(requestUri, content);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(result.ReasonPhrase);
            }

            var body = await result.Content.ReadAsStringAsync();
            customVisionResult = CustomVisionResult.FromJson(body);
        }

        // Predictions�̐擪�����O�ɏo��
        Debug.Log(customVisionResult.ToString());

        // Label�𐶐�
        var predictions = customVisionResult.predictions;
        foreach (var prediction in predictions)
        {
            // �m����0.8�ȏ�̗\������Label�𐶐�����
            if (prediction.probability > 0.8)
            {
                var tagName = prediction.tagName;
                var position = new Vector3(0, 0, 0.6f);// BoundingBox���g���Ăǂ̂悤�ɐݒ肵����ǂ����A�킩��Ȃ����ߌŒ�
                CreateLabel(tagName, position);
            }
        }
    }

    void Update()
    {
        
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
