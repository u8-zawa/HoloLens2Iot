using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class SensorData
{
    public string name;
    public List<int> datas;
    public List<long> times;
    public Statistics stat;

    public static SensorData FromJson(string json)
    {
        SensorData sensorData = null;
        try
        {
            sensorData = JsonUtility.FromJson<SensorData>(json);
        } catch (Exception e) {
            Debug.Log(e.Message);
        }
        return sensorData;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(times);
    }

    // デバック用
    override
    public string ToString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("SensorDataReceived");
        stringBuilder.AppendLine($"name = {this.name}");
        stringBuilder.AppendLine("datas = [");
        for (int i=0; i<this.datas.Count; i++)
        {
            stringBuilder.AppendLine($"  {this.datas[i]},");
        }
        stringBuilder.AppendLine("]");
        stringBuilder.AppendLine("times = [");
        for (int i=0; i<this.times.Count; i++)
        {
            stringBuilder.AppendLine($"  {this.times[i]},");
        }
        stringBuilder.AppendLine("]");
        stringBuilder.AppendLine(this.stat.ToString());

        return stringBuilder.ToString();
    }

}