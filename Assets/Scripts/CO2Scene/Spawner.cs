using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int MaxNum = 30;
    [SerializeField] private int nowNum = 0;

    private List<GameObject> objects = new List<GameObject>();
    private bool isSpawnable => objects.Count < MaxNum;

    public int Num => objects.Count;

    private void Update()
    {
        if( nowNum < Num ) {
            Sub(Num - nowNum);
        }
        else if(nowNum > Num)
        {
            Add(nowNum - Num);
        }
    }

    private void Add(int n = 1)
    {
        if (n <= 0 || n + Num > MaxNum) return;
        for(int i = 0; i < n; i++)
        {
            CreateObject();
        }
    }
    private void Sub(int n = 0)
    {
        if (n <= 0 || n > Num) return;
        for(int i = 0; i < n; i++)
        {
            DestroyObject(objects[0]);
        }
    }


    // 新しくオブジェクトを生み出す
    private void CreateObject()
    {
        if (isSpawnable)
        {
            // 新しくオブジェクトを生み出す
            GameObject newObject = Instantiate(prefab);
            objects.Add(newObject);
        }
    }

    // オブジェクトを消す
    private void DestroyObject(GameObject targetObject)
    {
        if(objects.Contains(targetObject))
        {
            objects.Remove(targetObject);
            Destroy(targetObject);
        }
    }
}
