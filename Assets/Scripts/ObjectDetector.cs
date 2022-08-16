using CustomVision.Value;
using Microsoft.MixedReality.Toolkit.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.WebCam;

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

    private PhotoCapture photoCapture;
    private Resolution cameraResolution;

    void Start()
    {
        cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending(res => res.width * res.height).First();
    }

    // �J�����ŎB�e�����摜�Ő��_���J�n����
    public async void StartDetect()
    {
        // Label���폜
        ClearLabels();

        TakePicture();
        await Task.Delay(3000);

        var image = imageObject.GetComponent<Image>();
        var imageData = image.sprite.texture.EncodeToJPG(); // texture�Ƃ����摜(.jpg)��byte�z��ɕϊ�

        customVisionResult = await DetectImageAsync(imageData, 0.5);

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

    // �ʐ^���B��
    private void TakePicture()
    {
        PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject)
        {
            photoCapture = captureObject;
            CameraParameters cameraParameters = new CameraParameters
            {
                hologramOpacity = 0.0f,
                cameraResolutionWidth = cameraResolution.width,
                cameraResolutionHeight = cameraResolution.height,
                pixelFormat = CapturePixelFormat.BGRA32
            };

            photoCapture.StartPhotoModeAsync(cameraParameters, p =>
            {
                photoCapture.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        });

    }

    // imageObject�ɎB�����ʐ^����������
    private void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        var targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
        photoCaptureFrame.UploadImageDataToTexture(targetTexture);
        imageObject.GetComponent<Image>().sprite = Sprite.Create(targetTexture, new Rect(0, 0, targetTexture.width, targetTexture.height), new Vector2(0.5f, 0.5f));

        photoCapture.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    // PhotoMode���I������
    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        photoCapture.Dispose();
        photoCapture = null;
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

    // Label���폜����
    private void ClearLabels()
    {
        foreach (var label in createdLabels)
        {
            Destroy(label);
        }
        createdLabels.Clear();
    }
}
