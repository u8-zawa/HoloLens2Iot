using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SensorData
{
    public string Name { get; set; }

    public List<int> Datas { get; set; }

    public List<long> Times { get; set; }

    public Statistics Stat { get; set; }

    public static SensorData FromJson(string json)
    {
        var settings = new JsonSerializerSettings
        {
            // �����o�����݂��Ȃ��ꍇ�A��O���o�͂���(�f�V���A���C�Y��)
            MissingMemberHandling = MissingMemberHandling.Error
        };
        return JsonConvert.DeserializeObject<SensorData>(json, settings);
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    // �������`����Json�ł��邩�ǂ���
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