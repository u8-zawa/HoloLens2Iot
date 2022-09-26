using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SensorData
{
    public string Name { get; set; }

    public List<float> Datas { get; set; }

    public List<long> Times { get; set; }

    public Statistics Stat { get; set; }

    public static SensorData FromJson(string json)
    {
        var settings = new JsonSerializerSettings
        {
            // メンバが存在しない場合、例外を出力する(デシリアライズ時)
            MissingMemberHandling = MissingMemberHandling.Error
        };
        return JsonConvert.DeserializeObject<SensorData>(json, settings);
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    // 正しい形式のJsonであるかどうか
    public static bool IsSensorDataJson(string json)
    {
        try
        {
            FromJson(json);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
        return true;
    } 
}