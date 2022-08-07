using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab; // ��������I�u�W�F�N�g�iPrefab�j
    [SerializeField] private int MinNum = 3;    // �I�u�W�F�N�g�̍ŏ�������
    [SerializeField] private int MaxNum = 30;   // �I�u�W�F�N�g�̍ő吶����
    public int nowNum = 0;                      // ���݂̃I�u�W�F�N�g�̐ݒ萔

    private List<GameObject> objects = new List<GameObject>();  // �Ǘ����Ă���I�u�W�F�N�g�̃��X�g
    private bool isSpawnable => objects.Count < MaxNum; // �����\���̃t���O

    public int Num => objects.Count;    // ���݊Ǘ����Ă���I�u�W�F�N�g�̐�

    private void Start()
    {
        if (MinNum > 0) Add(MinNum);
    }

    private void Update()
    {
        // �I�u�W�F�N�g�̐ݒ萔�ƊǗ����ɍ�������ꍇ�A���̕������I�u�W�F�N�g��ǉ��E�폜����
        if( nowNum < Num ) {
            Sub(Num - nowNum);
        }
        else if(nowNum > Num)
        {
            Add(nowNum - Num);
        }
    }

    // �����w�肵�āA���̕��I�u�W�F�N�g�𑝂₷
    private void Add(int n = 1)
    {
        if (n <= 0 || Num >= MaxNum) return;
        else if (n + Num >= MaxNum) n = MaxNum - Num;
        for(int i = 0; i < n; i++)
        {
            CreateObject();
        }
    }
    // �����w�肵�āA���̕��I�u�W�F�N�g�����炷
    private void Sub(int n = 0)
    {
        if (n <= 0) return;
        else if (Num - n < MinNum) n = Num - MinNum;

        if (n > Num) n = Num;
        for(int i = 0; i < n; i++)
        {
            DestroyObject(objects[0]);
        }
    }


    // �V�����I�u�W�F�N�g�𐶂ݏo��
    private void CreateObject()
    {
        if (isSpawnable)
        {
            // �V�����I�u�W�F�N�g�𐶂ݏo��
            GameObject newObject = Instantiate(prefab);
            objects.Add(newObject);
        }
    }

    // �I�u�W�F�N�g������
    private void DestroyObject(GameObject targetObject)
    {
        if(objects.Contains(targetObject))
        {
            objects.Remove(targetObject);
            Destroy(targetObject);
        }
    }
}
