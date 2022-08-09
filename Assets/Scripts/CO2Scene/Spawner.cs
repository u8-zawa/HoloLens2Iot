using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab; // 生成するオブジェクト（Prefab）
    [SerializeField] private int MinNum = 3;    // オブジェクトの最小生成数
    [SerializeField] private int MaxNum = 30;   // オブジェクトの最大生成数
    public int nowNum = 0;                      // 現在のオブジェクトの設定数

    private List<GameObject> objects = new List<GameObject>();  // 管理しているオブジェクトのリスト
    private bool isSpawnable => objects.Count < MaxNum; // 生成可能かのフラグ

    public int Num => objects.Count;    // 現在管理しているオブジェクトの数

    private void Start()
    {
        if (MinNum > 0) Add(MinNum);
    }

    private void Update()
    {
        // オブジェクトの設定数と管理数に差がある場合、その分だけオブジェクトを追加・削除する
        if( nowNum < Num ) {
            Sub(Num - nowNum);
        }
        else if(nowNum > Num)
        {
            Add(nowNum - Num);
        }
    }

    // 数を指定して、その分オブジェクトを増やす
    private void Add(int n = 1)
    {
        if (n <= 0 || Num >= MaxNum) return;
        else if (n + Num >= MaxNum) n = MaxNum - Num;
        for(int i = 0; i < n; i++)
        {
            CreateObject();
        }
    }
    // 数を指定して、その分オブジェクトを減らす
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


    // 新しくオブジェクトを生み出す
    private void CreateObject()
    {
        if (isSpawnable)
        {
            // 新しくオブジェクトを生み出す
            GameObject newObject = Instantiate(prefab, transform.position, Quaternion.identity, transform.parent);
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
