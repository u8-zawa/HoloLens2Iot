using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatMove : MonoBehaviour
{
    // ������͂̑傫���͈̔͐ݒ�
    [SerializeField] public float minForcePower = 0.1f;
    [SerializeField] public float maxForcePower = 1f;
    // �͂�������Ԋu�͈̔͐ݒ�
    [SerializeField] public float minMoveDuration = 0.2f;
    [SerializeField] public float maxMoveDuration = 3f;

    // �ړ��̌p���t���O
    private bool isFloating = true;

    // Rigidbody�̃L���b�V��
    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        // Start()�Ɠ����Ɉړ����J�n����
        StartCoroutine(Floating());
    }

    // ���b�����ɃI�u�W�F�N�g�ɑ΂��ė͂�������
    private IEnumerator Floating() {
        isFloating = true;
        while (true) {
            // isFloating��False�Ȃ�͂�������̂��~�߂�
            if (!isFloating)
            {
                yield break;
            }
            // �����_���ȕ����փ����_���ȑ傫���̗͂�������
            float Length = Random.Range(minForcePower, maxForcePower);
            Vector3 force = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)) * Vector3.up;
            _rb.AddForce(force * Length, ForceMode.Impulse);
            _rb.AddTorque(force, ForceMode.Impulse);
            // �P�`�R�b���x�ҋ@����
            float waitTime = Random.Range(minMoveDuration, maxMoveDuration);
            yield return new WaitForSeconds(waitTime);
        }
    }

    // �ړ��̊J�n
    public void startFloat()
    {
        if( !isFloating )
        {
            StartCoroutine(Floating());
        }
    }

    // �ړ��̏I��
    public void endFloat()
    {
        isFloating = false;
    }

    // ���f����͂񂾂Ƃ��̏���
    public void startGrab()
    {
        // �ړ�����U�~�߂�
        endFloat();
    }

    // ���f���𗣂����Ƃ��̏���
    public void endGrab()
    {
        // �ړ����ĊJ����
        startFloat();
    }
}
