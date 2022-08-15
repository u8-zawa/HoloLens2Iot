using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CustomVision
{
    [Serializable]
    public class CustomVisionResult
    {
        public string id;
        public string project;
        public string iteration;
        public string created;
        public List<Prediction> predictions;

        public static CustomVisionResult FromJson(string json)
        {
            CustomVisionResult customVisionResult = null;
            try
            {
                customVisionResult = JsonUtility.FromJson<CustomVisionResult>(json);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            return customVisionResult;
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
            stringBuilder.AppendLine("CustomVisionResult");
            stringBuilder.AppendLine($"id = {this.id}");
            stringBuilder.AppendLine($"project = {this.project}");
            stringBuilder.AppendLine($"iteration = {this.iteration}");
            stringBuilder.AppendLine($"created = {this.created}");
            // 確率が0.8以上の予測だけを表示する
            if (this.predictions.Any(p => p.probability > 0.8))
            {
                foreach (var prediction in this.predictions)
                {
                    if (prediction.probability > 0.8)
                    {
                        stringBuilder.AppendLine($"{prediction.ToString()}");
                    }
                }
            }
            else
            {
                stringBuilder.AppendLine("\nThe probability of all predictions is less than 0.8");
            }

            return stringBuilder.ToString();
        }
    }

    [Serializable]
    public class Prediction
    {
        public double probability;
        public string tagId;
        public string tagName;
        public BoundingBox boundingBox;

        // デバック用
        override
        public string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Prediction");
            stringBuilder.AppendLine($"probability = {this.probability}");
            stringBuilder.AppendLine($"tagId = {this.tagId}");
            stringBuilder.AppendLine($"tagName = {this.tagName}");
            stringBuilder.AppendLine($"{this.boundingBox.ToString()}");

            return stringBuilder.ToString();
        }
    }

    [Serializable]
    public class BoundingBox
    {
        public double left;
        public double top;
        public double width;
        public double height;

        // デバック用
        override
        public string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("BoundingBox");
            stringBuilder.AppendLine($"left = {this.left}");
            stringBuilder.AppendLine($"top = {this.top}");
            stringBuilder.AppendLine($"width = {this.width}");
            stringBuilder.AppendLine($"height = {this.height}");

            return stringBuilder.ToString();
        }
    }
}
