using System;
using System.Text;

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
        stringBuilder.AppendLine("Prediction = {");
        stringBuilder.AppendLine($"  probability = {this.probability}");
        stringBuilder.AppendLine($"  tagId = {this.tagId}");
        stringBuilder.AppendLine($"  tagName = {this.tagName}");
        stringBuilder.AppendLine($"  {this.boundingBox.ToString()}");
        stringBuilder.AppendLine("}");

        return stringBuilder.ToString();
    }
}
