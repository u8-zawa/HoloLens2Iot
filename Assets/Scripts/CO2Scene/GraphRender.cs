using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

[RequireComponent(typeof(LineChart))]
public class GraphRender : MonoBehaviour
{
    [SerializeField] private string title = "CO2濃度";
    [SerializeField] private int maxValue = 3000;
    [SerializeField] private int minValue = 300;
    [SerializeField] private bool setXValue = true;

    private LineChart chart;
    private Serie serie;

    // Start is called before the first frame update
    void Start()
    {
        // LineChartとSerieを取得
        chart = GetComponent<LineChart>();
        if(chart.series.Count > 0)
        {
            serie = chart.series[0];
        }
        else
        {
            serie = chart.AddSerie<Serie>();
        }

        // タイトルを設定
        Title title = chart.GetChartComponent<Title>();
        title.text = this.title;

        // Y軸の最大値と最小値を設定
        YAxis axis = chart.GetChartComponent<YAxis>();
        axis.minMaxType = Axis.AxisMinMaxType.Custom;
        axis.min = minValue;
        axis.max = maxValue;

        // X軸のタイプを値に変更
        XAxis xAxis = chart.GetChartComponent<XAxis>();
        if( setXValue )
        {
            xAxis.type = Axis.AxisType.Value;
        }
        else
        {
            xAxis.type = Axis.AxisType.Category;
        }
    }

    // 5秒毎にランダムな値にグラフを更新してデバッグする
    [ContextMenu("Debug/GraphUpdateTest")]
    private void GraphUpdateTest()
    {
        StartCoroutine(UpdateRandomValue());
    }

    private IEnumerator UpdateRandomValue()
    {
        List<long> datax = new List<long>() { 0, 1, 2, 3, 5};
        List<int> datas = new List<int>();
        for(int i = 0; i < 5; i++)
        {
            datas.Add(Random.Range(600, 700));
        }
        WaitForSeconds wait = new WaitForSeconds(5);
        while (true)
        {
            datas.Add(Random.Range(600, 700));
            datas.RemoveAt(0);
            UpdateDataXY(datax, datas);
            //UpdateData(datas);
            yield return wait;
        }
    }

    // センサーの取得情報に従ってグラフを更新する
    public void UpdateSensorValue(string sensorName)
    {
        SensorDataManager sdm = SensorDataManager.Instance;
        SensorData sensorData;
        if( sdm == null || (sensorData = sdm.GetSensorData(sensorName)) == null )
        {
            return;
        }
        UpdateDataXY(sensorData.times, sensorData.datas);
    }

    // Y軸のデータを更新する（X軸方向は横に一定間隔に並ぶ）
    void UpdateYData(List<int> datas)
    {
        serie.ClearData();

        for(int i = 0; i < datas.Count; i++)
        {
            SerieData addData = new SerieData();
            addData.data = new List<double> { i, datas[i] };
            serie.AddSerieData(addData);
        }
    }

    // X軸Y軸セットのデータを更新する
    void UpdateDataXY(List<long> dataX, List<int> dataY)
    {
        serie.ClearData();

        for (int i = 0; i < dataX.Count; i++)
        {
            serie.AddXYData(dataX[i], dataY[i]);
        }
    }
}
