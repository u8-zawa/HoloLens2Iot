using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorDataManager : SingletonMonoBehaviorInScene<SensorDataManager>
{
    private Dictionary<string, SensorData> datas = new Dictionary<string, SensorData> ();

    // �Z���T�[���̍X�V
    public void UpdateData(SensorData data)
    {
        datas[data.name] = data;
    }

    // �Z���T�[���̎擾
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
