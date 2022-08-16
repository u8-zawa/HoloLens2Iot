using System.Text;

namespace CustomVision.Value
{
    public class Prediction
    {
        public double Probability { get; set; }

        public string TagId { get; set; }

        public string TagName { get; set; }

        public BoundingBox BoundingBox { get; set; }

        // デバック用
        override
        public string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Prediction");
            stringBuilder.AppendLine($"probability = {this.Probability}");
            stringBuilder.AppendLine($"tagId = {this.TagId}");
            stringBuilder.AppendLine($"tagName = {this.TagName}");
            stringBuilder.AppendLine($"{this.BoundingBox.ToString()}");

            return stringBuilder.ToString();
        }
    }
}
