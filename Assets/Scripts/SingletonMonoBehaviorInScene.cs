using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �V�[����Ɉ���������I�u�W�F�N�g�Ƃ��ĐU�镑���R���|�[�l���g
// �p������ƁA�N���X��.Instance �ŃA�N�Z�X�\�ɂȂ�
public abstract class SingletonMonoBehaviorInScene<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            // �V�[����ɂQ�ȏ�C���X�^���X�����݂���ꍇ�̓G���[
            throw new Exception("�V�[�����ɑ��̃C���X�^���X�����݂��܂��I");
        }

        Instance = this as T;
    }
}
