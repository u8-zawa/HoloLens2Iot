using System;
using System.Collections.Generic;
using System.Text;

namespace CustomVision.Value
{
    public class CustomVisionResult
    {
        public string Id { get; set; }

        public string Project { get; set; }

        public string Iteration { get; set; }

        public DateTime Created { get; set; }

        public List<Prediction> Predictions { get; set; }

        // デバック用
        override
        public string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("CustomVisionResult");
            stringBuilder.AppendLine($"id = {this.Id}");
            stringBuilder.AppendLine($"project = {this.Project}");
            stringBuilder.AppendLine($"iteration = {this.Iteration}");
            stringBuilder.AppendLine($"created = {this.Created}");
            foreach (var prediction in this.Predictions)
            {
                stringBuilder.AppendLine($"{prediction.ToString()}");
            }

            return stringBuilder.ToString();
        }
    }
}
