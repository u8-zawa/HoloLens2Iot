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

    // ���b�����ɃI�u�W�F�N�g�ɑ΂��ė͂�������
    private IEnumerator Floating() {
        while (true) {
            // �����_���ȕ����փ����_���ȑ傫���̗͂�������
            float Length = Random.Range(0.1f, 1f);
            Vector3 force = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)) * Vector3.up;
            _rb.AddForce(force * Length, ForceMode.Impulse);
            _rb.AddTorque(force, ForceMode.Impulse);
            // isFloating��False�Ȃ�͂�������̂��~�߂�
            if (!isFloating) {
                yield break;
            }
            // �P�`�R�b���x�ҋ@����
            float waitTime = Random.Range(0.2f, 3f);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
