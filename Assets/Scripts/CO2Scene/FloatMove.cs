using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatMove : MonoBehaviour
{
    static private string BoxTag = "CO2Box";

    // 加える力の大きさの範囲設定
    [SerializeField] public float minForcePower = 0.1f;
    [SerializeField] public float maxForcePower = 1f;
    // 力を加える間隔の範囲設定
    [SerializeField] public float minMoveDuration = 0.2f;
    [SerializeField] public float maxMoveDuration = 3f;

    // 移動の継続フラグ
    private bool isFloating = true;
    // 箱外フラグ
    private bool isOutOfBox = false;
    // 掴んでいるフラグ
    private bool isGrabbed = false;

    private Rigidbody _rb;  // Rigidbodyのキャッシュ
    private Vector3 _homePosition;  // 生成時の位置の記憶

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _homePosition = transform.position;
        // Start()と同時に移動を開始する
        StartCoroutine(Floating());
    }

    private void Update()
    {
        // 掴んでいないときに箱の外に出た際は、すぐに初期位置に戻す
        if(!isGrabbed & isOutOfBox)
        {
            transform.position = _homePosition;
        }
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

    // 箱から出たときの処理
    public void OnBoxExit(Collider other)
    {
        if (other.CompareTag(BoxTag))
        {
            isOutOfBox = true;
        }
    }

    // 箱に入ったときの処理
    public void OnBoxEnter(Collider other)
    {
        if (other.CompareTag(BoxTag))
        {
            isOutOfBox = false;
        }
    }

    // モデルを掴んだときの処理
    public void startGrab()
    {
        isGrabbed = true;
        // 移動を一旦止める
        endFloat();
    }

    // モデルを離したときの処理
    public void endGrab()
    {
        isGrabbed = false;
        // 箱の外にあるときは、初期位置に移動する
        if (isOutOfBox)
        {
            transform.position = _homePosition;
        }
        // 移動を再開する
        startFloat();
    }
}
