using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MQTTRequestIntervalSender : MonoBehaviour
{
    [SerializeField] private MqttController mqttController;     // MQTTController
    [SerializeField] private List<string> sensorNames;          // 取得するセンサー名のリスト
    [SerializeField] private float SensorRequestInterval = 5;   // Sensor情報を取得するインターバル

    public bool endFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RequestInterval());
    }

    // 定期的にセンサー情報を取得する
    private IEnumerator RequestInterval()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(SensorRequestInterval);
        while (true)
        {
            if (endFlag)
            {
                yield break;
            }
            // センサーの情報を取得する
            foreach (string sensorName in sensorNames)
            {
                mqttController.GetSensorData(sensorName);
            }
            yield return waitForSeconds;
        }
    }
}
