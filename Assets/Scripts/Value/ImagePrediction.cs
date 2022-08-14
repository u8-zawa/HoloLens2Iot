using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class ImagePrediction
{
    public string id;
    public string project;
    public string iteration;
    public string created;
    public List<Prediction> predictions;

    public static ImagePrediction FromJson(string json)
    {
        ImagePrediction imagePrediction = null;
        try
        {
            imagePrediction = JsonUtility.FromJson<ImagePrediction>(json);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        return imagePrediction;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    // デバック用
    override
    public string ToString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("ImagePrediction");
        stringBuilder.AppendLine($"id = {this.id}");
        stringBuilder.AppendLine($"project = {this.project}");
        stringBuilder.AppendLine($"iteration = {this.iteration}");
        stringBuilder.AppendLine($"created = {this.created}");
        stringBuilder.AppendLine($"{this.predictions[0].ToString()}");

        return stringBuilder.ToString();
    }
}
