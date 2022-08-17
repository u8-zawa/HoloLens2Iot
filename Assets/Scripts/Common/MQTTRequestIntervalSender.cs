using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MQTTRequestIntervalSender : MonoBehaviour
{
    [SerializeField] private MqttController mqttController;     // MQTTController
    [SerializeField] private List<string> sensorNames;          // �擾����Z���T�[���̃��X�g
    [SerializeField] private float SensorRequestInterval = 5;   // Sensor�����擾����C���^�[�o��

    public bool endFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RequestInterval());
    }

    // ����I�ɃZ���T�[�����擾����
    private IEnumerator RequestInterval()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(SensorRequestInterval);
        while (true)
        {
            if (endFlag)
            {
                yield break;
            }
            // �Z���T�[�̏����擾����
            foreach (string sensorName in sensorNames)
            {
                mqttController.GetSensorData(sensorName);
            }
            yield return waitForSeconds;
        }
    }
}
