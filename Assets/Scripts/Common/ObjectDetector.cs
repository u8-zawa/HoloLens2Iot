using CustomVision.Value;
using System.Threading.Tasks;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private CustomVisionService customVisionService;

    [SerializeField]
    private CameraCapture cameraCapture;
    
    [SerializeField]
    private ObjectLabeler objectLabeler;

    [SerializeField]
    [Range(0.01f, 1f)]
    private double threshold = 0.5;

    [SerializeField]
    private double delaySeconds = 3;

    private CustomVisionResult customVisionResult;

    // カメラで撮影した画像で推論を開始する
    public async void StartDetect()
    {
        // ラベルを全て削除する
        objectLabeler.DestroyLabels();

        // 写真を撮る
        cameraCapture.StartPhotoCapture();

        // 遅延させる
        await Task.Delay((int)delaySeconds * 1000);

        // 物体検出をする
        customVisionResult = await customVisionService.DetectAsync(cameraCapture.image, threshold);

        Debug.Log(customVisionResult.ToString());

        // 物体検出の結果に基づいて、ラベルを生成する
        var predictions = customVisionResult.Predictions;
        foreach (var prediction in predictions)
        {
            var tagName = prediction.TagName;
            var position = new Vector3(0, 0, 0.6f);// BoundingBoxを使ってどのように設定したら良いか、わからないため固定
            objectLabeler.CreateLabel(tagName, position);
        }
    }
}
