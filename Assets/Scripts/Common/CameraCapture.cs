using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.WebCam;

public class CameraCapture : MonoBehaviour
{
    [SerializeField]
    private Image imageObject; // �B�e�����ʐ^��\��������p�̃I�u�W�F�N�g

    [SerializeField]
    private Texture2D sampleTexture; // �B�e����ʐ^�̑���Ƃ���e�N�X�`��

    [HideInInspector]
    public byte[] image; // �B�e�����ʐ^��byte�z��

    private PhotoCapture photoCapture;

    public void StartPhotoCapture()
    {
        // sampleTexture��null��������A�B�e����
        if (sampleTexture == null)
        {
            PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
        }
        else
        {
            var sprite = Sprite.Create(sampleTexture, new Rect(0, 0, sampleTexture.width, sampleTexture.height), new Vector2(0.5f, 0.5f));

            // imageObject���ݒ肳��Ă�����A�w�肵���ʐ^��\��������
            if (imageObject != null)
            {
                imageObject.sprite = sprite;
            }

            // �w�肵���ʐ^��byte�z���������
            image = sprite.texture.EncodeToJPG();
        }
    }

    private void OnPhotoCaptureCreated(PhotoCapture captureObject)
    {
        photoCapture = captureObject;

        var cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

        var cameraParameters = new CameraParameters();
        cameraParameters.hologramOpacity = 0.0f;
        cameraParameters.cameraResolutionWidth = cameraResolution.width;
        cameraParameters.cameraResolutionHeight = cameraResolution.height;
        cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

        captureObject.StartPhotoModeAsync(cameraParameters, OnPhotoModeStarted);
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            Debug.Log("OnPhotoModeStarted");
            photoCapture.TakePhotoAsync(OnCapturedToMemory);
        }
        else
        {
            throw new Exception("Unable to start photo mode!");
        }
    }

    private void onPhotoModeStopped(PhotoCapture.PhotoCaptureResult result)
    {
        Debug.Log("onPhotoModeStopped");
        photoCapture.Dispose();
        photoCapture = null;
    }

    private void OnCapturedToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        if (result.success)
        {
            Debug.Log("OnCapturedToMemory");
            var cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
            var targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
            photoCaptureFrame.UploadImageDataToTexture(targetTexture);
            var sprite = Sprite.Create(targetTexture, new Rect(0, 0, targetTexture.width, targetTexture.height), new Vector2(0.5f, 0.5f));

            // imageObject���ݒ肳��Ă�����A�B�e�����ʐ^��\��������
            if (imageObject != null)
            {
                imageObject.sprite = sprite;
            }

            // �B�e�����ʐ^��byte�z���������
            image = sprite.texture.EncodeToJPG();
        }
        else
        {
            throw new Exception("Unable to capture to memory!");
        }
        photoCapture.StopPhotoModeAsync(onPhotoModeStopped);
    }
}
