using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogMessageUI : MonoBehaviour
{
    TextMeshPro UIText = null;

    string messages = "";

    void Start()
    {
        UIText = GameObject.Find("LogMessageText").GetComponent<TextMeshPro>();

        Application.logMessageReceived += logMessageReceived;
    }

    private void logMessageReceived(string condition, string stackTrace, LogType type)
    {
        messages = condition + "\n" + messages;

        UIText.text = messages;

        switch (type)
        {
            case LogType.Error:
                break;
            case LogType.Assert:
                break;
            case LogType.Warning:
                break;
            case LogType.Log:
                break;
            case LogType.Exception:
                break;
        }
    }
}
