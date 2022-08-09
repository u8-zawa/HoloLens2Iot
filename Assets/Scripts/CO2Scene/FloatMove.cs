using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatMove : MonoBehaviour
{
    // 加える力の大きさの範囲設定
    [SerializeField] public float minForcePower = 0.1f;
    [SerializeField] public float maxForcePower = 1f;
    // 力を加える間隔の範囲設定
    [SerializeField] public float minMoveDuration = 0.2f;
    [SerializeField] public float maxMoveDuration = 3f;

    // 移動の継続フラグ
    private bool isFloating = true;

    // Rigidbodyのキャッシュ
    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        // Start()と同時に移動を開始する
        StartCoroutine(Floating());
    }

    // 数秒おきにオブジェクトに対して力を加える
    private IEnumerator Floating() {
        isFloating = true;
        while (true) {
            // isFloatingがFalseなら力を加えるのを止める
            if (!isFloating)
            {
                yield break;
            }
            // ランダムな方向へランダムな大きさの力を加える
            float Length = Random.Range(minForcePower, maxForcePower);
            Vector3 force = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)) * Vector3.up;
            _rb.AddForce(force * Length, ForceMode.Impulse);
            _rb.AddTorque(force, ForceMode.Impulse);
            // １～３秒程度待機する
            float waitTime = Random.Range(minMoveDuration, maxMoveDuration);
            yield return new WaitForSeconds(waitTime);
        }
    }

    // 移動の開始
    public void startFloat()
    {
        if( !isFloating )
        {
            StartCoroutine(Floating());
        }
    }

    // 移動の終了
    public void endFloat()
    {
        isFloating = false;
    }

    // モデルを掴んだときの処理
    public void startGrab()
    {
        // 移動を一旦止める
        endFloat();
    }

    // モデルを離したときの処理
    public void endGrab()
    {
        // 移動を再開する
        startFloat();
    }
}
