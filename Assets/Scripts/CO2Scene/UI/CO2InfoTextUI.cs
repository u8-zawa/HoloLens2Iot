using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CO2InfoTextUI : MonoBehaviour
{
    private TextMeshProUGUI TextUI;

    [SerializeField] private Spawner spawner;
    [SerializeField, TextArea(3, 5)] private string textFormat = "二酸化炭素濃度：{0}、二酸化炭素個数：{1}";

    private int CO2ModelN = 0;
    private int CO2Value = 0;

    // Start is called before the first frame update
    void Start()
    {
        TextUI = GetComponent<TextMeshProUGUI>();

        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        // 値の取得
        bool flag = false;
        SensorData sensorData = SensorDataManager.Instance.GetSensorData("CO2");
        if (sensorData != null && sensorData.Stat.Latest != CO2Value)
        {
            CO2Value = sensorData.Stat.Latest;
            flag = true;
        }else if (sensorData == null)
        {
            CO2Value = -1;
        }
        if(spawner != null && spawner.Num != CO2ModelN)
        {
            CO2ModelN = spawner.Num;
            flag = true;
        }

        // テキストの更新
        if (flag)
        {
            UpdateText();
        }
    }

    // 表示内容の更新
    private void UpdateText()
    {
        string message;
        if (CO2Value < 0)
        {
            message = string.Format(textFormat, "?", CO2ModelN);
        }
        else
        {
            message = string.Format(textFormat, CO2Value, CO2ModelN);
        }

        TextUI.text = message;
    }
}
