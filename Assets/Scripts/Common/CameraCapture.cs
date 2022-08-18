using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.WebCam;

public class CameraCapture : MonoBehaviour
{
    [SerializeField]
    private Image imageObject; // 撮影した写真を表示させる用のオブジェクト

    [SerializeField]
    private Texture2D sampleTexture; // 撮影する写真の代わりとするテクスチャ

    [HideInInspector]
    public byte[] image; // 撮影した写真のbyte配列

    private PhotoCapture photoCapture;

    public void StartPhotoCapture()
    {
        // sampleTextureがnullだったら、撮影する
        if (sampleTexture == null)
        {
            PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
        }
        else
        {
            var sprite = Sprite.Create(sampleTexture, new Rect(0, 0, sampleTexture.width, sampleTexture.height), new Vector2(0.5f, 0.5f));

            // imageObjectが設定されていたら、指定した写真を表示させる
            if (imageObject != null)
            {
                imageObject.sprite = sprite;
            }

            // 指定した写真のbyte配列を代入する
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

            // imageObjectが設定されていたら、撮影した写真を表示させる
            if (imageObject != null)
            {
                imageObject.sprite = sprite;
            }

            // 撮影した写真のbyte配列を代入する
            image = sprite.texture.EncodeToJPG();
        }
        else
        {
            throw new Exception("Unable to capture to memory!");
        }
        photoCapture.StopPhotoModeAsync(onPhotoModeStopped);
    }
}
