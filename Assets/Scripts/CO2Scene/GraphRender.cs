using System;
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

    private long FromTicks;

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
        serie.ClearData();

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
            xAxis.type = Axis.AxisType.Category;
            xAxis.minMaxType = Axis.AxisMinMaxType.MinMax;
        }
        else
        {
            xAxis.type = Axis.AxisType.Category;
        }
        // 文字列の設定
        xAxis.axisLabel.textLimit.enable = false;
        xAxis.axisLabel.textStyle.fontSize = 15;
        xAxis.axisLabel.textPadding.top = 4;

        // 基準となる1970年1月1日のタイマ刻み秒を取得
        DateTime dt = new DateTime(1970, 1, 1);
        FromTicks = dt.Ticks;
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
            datas.Add(UnityEngine.Random.Range(600, 700));
        }
        WaitForSeconds wait = new WaitForSeconds(5);
        while (true)
        {
            datas.Add(UnityEngine.Random.Range(600, 700));
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
        UpdateYDataWithName(sensorData.Times, sensorData.Datas);
 //       UpdateDataXY(sensorData.Times, sensorData.Datas);
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

    // 名前と共にY軸のデータを更新する（X軸方向は横に一定間隔で並ぶ）
    void UpdateYDataWithName(List<long> times, List<int> datas)
    {
        serie.ClearData();
        XAxis xAxis = chart.GetChartComponent<XAxis>();
        xAxis.ClearData();

        for (int i = 0; i < datas.Count; i++)
        {
            chart.AddXAxisData(ToTimeString(times[i]));
            chart.AddData(0, datas[i]);
            //chart.AddData(0, datas[i], ToTimeString(times[i]));
        }
    }

    // X軸Y軸セットのデータを更新する
    void UpdateDataXY(List<long> dataX, List<int> dataY)
    {
        serie.ClearData();

        for (int i = 0; i < dataX.Count; i++)
        {
            serie.AddXYData(dataX[i], dataY[i]);
            Debug.Log("日付：" + ToTimeString(dataX[i]));
        }
    }

    // Jsonate式でのミリ秒（1970年1月1日からの経過ミリ秒）を時間に直す
    private string ToTimeString(long milliSec)
    {
        DateTime dt = new DateTime(FromTicks + milliSec * TimeSpan.TicksPerMillisecond).ToLocalTime();
        return dt.ToString("MM/dd\nHH:mm:ss");
    }
}
