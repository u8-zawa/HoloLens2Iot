using Microsoft.MixedReality.Toolkit.UI;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLabeler : MonoBehaviour
{
    [SerializeField]
    private GameObject labelObject; // ラベルに使用するオブジェクト
    
    private List<GameObject> labelObjects = new List<GameObject>();  // 表示しているラベルのリスト

    // Labelを生成する
    public void CreateLabel(string tagName, Vector3 position)
    {
        var newLabelObject = Instantiate(labelObject, position, Quaternion.identity, transform.parent);

        var toolTip = newLabelObject.GetComponent<ToolTip>();
        var toolTipText = tagName;
        switch (tagName)
        {
            case "co2_sensor":
                toolTipText = "二酸化炭素センサー";
                break;
            case "gsr_sensor":
                toolTipText = "うそ発見器";
                break;
            case "ud_sensor":
                toolTipText = "超音波距離センサー";
                break;
        }
        toolTip.ToolTipText = toolTipText;
        var connector = toolTip.GetComponent<ToolTipConnector>();
        connector.Target = newLabelObject;

        labelObjects.Add(newLabelObject);
    }

    // Labelを全て削除する
    public void DestroyLabels()
    {
        foreach (var label in labelObjects)
        {
            Destroy(label);
        }
        labelObjects.Clear();
    }
}
