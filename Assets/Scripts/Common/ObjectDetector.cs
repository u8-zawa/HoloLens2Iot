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

    // �J�����ŎB�e�����摜�Ő��_���J�n����
    public async void StartDetect()
    {
        // ���x����S�č폜����
        objectLabeler.DestroyLabels();

        // �ʐ^���B��
        cameraCapture.StartPhotoCapture();

        // �x��������
        await Task.Delay((int)delaySeconds * 1000);

        // ���̌��o������
        customVisionResult = await customVisionService.DetectAsync(cameraCapture.image, threshold);

        Debug.Log(customVisionResult.ToString());

        // ���̌��o�̌��ʂɊ�Â��āA���x���𐶐�����
        var predictions = customVisionResult.Predictions;
        foreach (var prediction in predictions)
        {
            var tagName = prediction.TagName;
            var position = new Vector3(0, 0, 0.6f);// BoundingBox���g���Ăǂ̂悤�ɐݒ肵����ǂ����A�킩��Ȃ����ߌŒ�
            objectLabeler.CreateLabel(tagName, position);
        }
    }
}
