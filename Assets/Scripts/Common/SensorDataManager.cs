using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorDataManager : SingletonMonoBehaviorInScene<SensorDataManager>
{
    private Dictionary<string, SensorData> datas = new Dictionary<string, SensorData> ();

    // センサー情報の更新
    public void UpdateData(SensorData data)
    {
        datas[data.name] = data;
    }

    // センサー情報の取得
    public SensorData GetSensorData(string name)
    {
        if (datas.ContainsKey(name))
        {
            return datas[name];
        }
        else
        {
            return null;
        }
    }
}
