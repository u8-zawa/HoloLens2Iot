using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ThresholdJudgeUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro UIText = null;
    [SerializeField] private ThresholdJudge thresholdJudge;

    public AudioClip sound1;
    public AudioClip sound2;
    public AudioSource audiosource;

    public void UpdateText() 
    {

        if (thresholdJudge.IsOver)
        {
        //    UIText.text = "greater than 2m: 周囲2m以内に人はいません";
           UIText.text = "周囲2m以内に人はいません";

           audiosource.PlayOneShot(sound1);
        } 
        else 
        {
        //    UIText.text = "less than 2m: 密です。人と2m以上距離をとってください";
           UIText.text = "密です。人と2m以上距離をとってください";
           audiosource.PlayOneShot(sound2);
        }
    }
}
