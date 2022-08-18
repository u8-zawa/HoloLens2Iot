using System.Text;

namespace CustomVision.Value
{
    public class BoundingBox
    {
        public double Left { get; set; }

        public double Top { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        // デバック用
        override
        public string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("BoundingBox");
            stringBuilder.AppendLine($"left = {this.Left}");
            stringBuilder.AppendLine($"top = {this.Top}");
            stringBuilder.AppendLine($"width = {this.Width}");
            stringBuilder.AppendLine($"height = {this.Height}");

            return stringBuilder.ToString();
        }
    }
}
