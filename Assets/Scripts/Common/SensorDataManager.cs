using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SensorDataManager : SingletonMonoBehaviorInScene<SensorDataManager>
{
    [SerializeField] private UnityEvent OnUpdateDataEvent = new UnityEvent();

    private Dictionary<string, SensorData> datas = new Dictionary<string, SensorData> ();

    // センサー情報の更新
    public void UpdateData(SensorData data)
    {
        if (data == null || data.name == null) return;
        datas[data.name] = data;
        Debug.Log("SaveData:" + data.ToString());

        // データを更新した際の処理を実行する
        OnUpdateDataEvent.Invoke();
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
