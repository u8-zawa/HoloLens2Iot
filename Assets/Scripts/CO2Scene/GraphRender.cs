using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

[RequireComponent(typeof(LineChart))]
public class GraphRender : MonoBehaviour
{
    [SerializeField] private string title = "CO2�Z�x";
    [SerializeField] private int maxValue = 3000;
    [SerializeField] private int minValue = 300;
    [SerializeField] private bool setXValue = true;

    private LineChart chart;
    private Serie serie;

    // Start is called before the first frame update
    void Start()
    {
        // LineChart��Serie���擾
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

        // �^�C�g����ݒ�
        Title title = chart.GetChartComponent<Title>();
        title.text = this.title;

        // Y���̍ő�l�ƍŏ��l��ݒ�
        YAxis axis = chart.GetChartComponent<YAxis>();
        axis.minMaxType = Axis.AxisMinMaxType.Custom;
        axis.min = minValue;
        axis.max = maxValue;

        // X���̃^�C�v��l�ɕύX
        XAxis xAxis = chart.GetChartComponent<XAxis>();
        if( setXValue )
        {
            xAxis.type = Axis.AxisType.Value;
            xAxis.minMaxType = Axis.AxisMinMaxType.MinMax;
        }
        else
        {
            xAxis.type = Axis.AxisType.Category;
        }
    }

    // 5�b���Ƀ����_���Ȓl�ɃO���t���X�V���ăf�o�b�O����
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

    // �Z���T�[�̎擾���ɏ]���ăO���t���X�V����
    public void UpdateSensorValue(string sensorName)
    {
        SensorDataManager sdm = SensorDataManager.Instance;
        SensorData sensorData;
        if( sdm == null || (sensorData = sdm.GetSensorData(sensorName)) == null )
        {
            return;
        }
        UpdateDataXY(sensorData.Times, sensorData.Datas);
    }

    // Y���̃f�[�^���X�V����iX�������͉��Ɉ��Ԋu�ɕ��ԁj
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

    // X��Y���Z�b�g�̃f�[�^���X�V����
    void UpdateDataXY(List<long> dataX, List<int> dataY)
    {
        serie.ClearData();

        for (int i = 0; i < dataX.Count; i++)
        {
            serie.AddXYData(dataX[i], dataY[i]);
        }
    }
}
