using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// �I�u�W�F�N�g�̏Փˎ��ɓ���̃��\�b�h���Ăяo���N���X
[RequireComponent(typeof(Collider))]
public class CollisionDetector : MonoBehaviour
{
    [SerializeField] private TriggerEvent onTriggerEnter = new TriggerEvent();
    [SerializeField] private TriggerEvent onTriggerStay = new TriggerEvent();
    [SerializeField] private TriggerEvent onTriggerExit = new TriggerEvent();

    //Is Trigger��ON�ő���Collider�ƂԂ������Ƃ��́A���̃��\�b�h���R�[�������
    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter.Invoke(other);
    }

    //Is Trigger��ON�ő���Collider�Əd�Ȃ��Ă���Ƃ��́A���̃��\�b�h����ɃR�[�������
    private void OnTriggerStay(Collider other)
    {
        onTriggerStay.Invoke(other);
    }

    //Is Trigger��ON�ő���Collider���痣�ꂽ�Ƃ��́A���̃��\�b�h���R�[�������
    private void OnTriggerExit(Collider other)
    {
        onTriggerExit.Invoke(other);
    }

    //UnityEvent���p�������N���X��[Serialize]������t�^���邱�ƂŁA
    //Inspector�E�B���h�E��ɕ\���ł���悤�ɂȂ�
    [Serializable]
    public class TriggerEvent : UnityEvent<Collider>
    {
    }
}
