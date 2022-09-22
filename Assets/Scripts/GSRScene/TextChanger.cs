using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChanger : MonoBehaviour
{
    private float start;
    private float elapsedTime;
    private int countdown;
    private int countcheck = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (countcheck == 1)
        {
            elapsedTime = Time.time - start;
            countdown = 10 - (int)elapsedTime;
            this.gameObject.GetComponent<TextMesh>().text = countdown.ToString();
        }
        if (countdown == 1)
        {
            CountEnd();
        }
    }

    public void CountStart()
    {
        start = Time.time;
        countcheck = 1;
    }

    public void CountEnd()
    {
        countcheck = 0;
    }
}
