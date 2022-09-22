using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ThresholdJudgeUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro UIText = null;
    [SerializeField] private ThresholdJudge thresholdJudge;

    public void UpdateText() 
    {

        if (thresholdJudge.IsOver)
        {
           UIText.text = "greater than 2m";
        } 
        else 
        {
           UIText.text = "less than 2m";
        }
    }
}
