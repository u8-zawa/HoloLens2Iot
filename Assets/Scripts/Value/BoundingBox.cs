using System;
using System.Text;

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
        stringBuilder.AppendLine("BoundingBox = {");
        stringBuilder.AppendLine($"    left = {this.left}");
        stringBuilder.AppendLine($"    top = {this.top}");
        stringBuilder.AppendLine($"    width = {this.width}");
        stringBuilder.AppendLine($"    height = {this.height}");
        stringBuilder.AppendLine("  }");

        return stringBuilder.ToString();
    }
}