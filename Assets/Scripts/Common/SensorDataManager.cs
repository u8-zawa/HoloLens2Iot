using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SensorDataManager : SingletonMonoBehaviorInScene<SensorDataManager>
{
    [SerializeField] private UnityEvent OnUpdateDataEvent = new UnityEvent();

    private Dictionary<string, SensorData> datas = new Dictionary<string, SensorData> ();

    // �Z���T�[���̍X�V
    public void UpdateData(SensorData data)
    {
        if (data == null || data.Name == null) return;
        datas[data.Name] = data;
        Debug.Log("SaveData:" + data.ToJson());

        // �f�[�^���X�V�����ۂ̏��������s����
        OnUpdateDataEvent.Invoke();
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
