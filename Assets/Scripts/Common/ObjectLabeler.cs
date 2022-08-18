using Microsoft.MixedReality.Toolkit.UI;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLabeler : MonoBehaviour
{
    [SerializeField]
    private GameObject labelObject; // ���x���Ɏg�p����I�u�W�F�N�g
    
    private List<GameObject> labelObjects = new List<GameObject>();  // �\�����Ă��郉�x���̃��X�g

    // Label�𐶐�����
    public void CreateLabel(string tagName, Vector3 position)
    {
        var newLabelObject = Instantiate(labelObject, position, Quaternion.identity, transform.parent);

        var toolTip = newLabelObject.GetComponent<ToolTip>();
        var toolTipText = tagName;
        switch (tagName)
        {
            case "co2_sensor":
                toolTipText = "��_���Y�f�Z���T�[";
                break;
            case "gsr_sensor":
                toolTipText = "����������";
                break;
            case "ud_sensor":
                toolTipText = "�����g�����Z���T�[";
                break;
        }
        toolTip.ToolTipText = toolTipText;
        var connector = toolTip.GetComponent<ToolTipConnector>();
        connector.Target = newLabelObject;

        labelObjects.Add(newLabelObject);
    }

    // Label��S�č폜����
    public void DestroyLabels()
    {
        foreach (var label in labelObjects)
        {
            Destroy(label);
        }
        labelObjects.Clear();
    }
}
