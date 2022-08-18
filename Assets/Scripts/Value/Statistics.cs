using System;
using System.Text;

[Serializable]
public class Statistics
{
    public int latest;
    public float avg;
    public int max;
    public int min;

    // デバック用
    override
    public string ToString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("stat = {");
        stringBuilder.AppendLine($"  latest = {this.latest}");
        stringBuilder.AppendLine($"  avg = {this.avg}");
        stringBuilder.AppendLine($"  max = {this.max}");
        stringBuilder.AppendLine($"  min = {this.min}");
        stringBuilder.AppendLine("}");

        return stringBuilder.ToString();
    }
}
