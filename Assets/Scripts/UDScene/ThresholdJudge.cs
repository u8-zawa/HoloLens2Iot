using UnityEngine;

public class ThresholdJudge : MonoBehaviour
{
    [SerializeField] private int threshold = 200;    // 閾値 (単位:cm)
    public int nowValue = 0;    //　現在の超音波距離センサーの測定値
    public bool IsOver { get; private set; }    // 現在のセンサーの測定値が閾値以上か以下かのフラグ

    
    // センサーの測定値と閾値の大小判定
    public void Judge()
    {   
        // センサーの測定値を取得
        SensorDataManager sdm = SensorDataManager.Instance;
        if (sdm == null || sdm.GetSensorData("ud") == null)
        {
            return;
        }

        // 現在のセンサーの測定値
        float nowValue = sdm.GetSensorData("ud").Stat.Latest;
        
        // 現在のセンサーの測定値と閾値の大小を比較
        if (nowValue < threshold) {
            IsOver = false;
            // Debug.Log("less than 2m");
        }
        else {
            IsOver = true;
            // Debug.Log("greater than 2m");
        }
    }
}

        