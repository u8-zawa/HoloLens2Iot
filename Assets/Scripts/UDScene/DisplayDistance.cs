using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayDistance : MonoBehaviour
{
    [SerializeField] private TextMeshPro UIText = null;
    private float UDValue = 0;


    public void UpdateText()
    {
        SensorData sensorData = SensorDataManager.Instance.GetSensorData("ud");
        if (sensorData != null && sensorData.Stat.Latest != UDValue)
        {
        // 現在のセンサの測定値
            UDValue = sensorData.Stat.Latest;
            if(UDValue < 100) {
                UIText.text = string.Format("{0 :#.##}cm", UDValue);
            } else {
                UIText.text = string.Format("{0 :#.##}m", UDValue / 100);            
            }
        }
        
    }
}
