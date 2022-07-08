using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatMove : MonoBehaviour
{
    private bool isFloating = true;

    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(Floating());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 数秒おきにオブジェクトに対して力を加える
    private IEnumerator Floating() {
        while (true) {
            // ランダムな方向へランダムな大きさの力を加える
            float Length = Random.Range(0.1f, 1f);
            Vector3 force = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)) * Vector3.up;
            _rb.AddForce(force * Length, ForceMode.Impulse);
            _rb.AddTorque(force, ForceMode.Impulse);
            // isFloatingがFalseなら力を加えるのを止める
            if (!isFloating) {
                yield break;
            }
            // １～３秒程度待機する
            float waitTime = Random.Range(0.2f, 3f);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
