using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// オブジェクトの衝突時に特定のメソッドを呼び出すクラス
[RequireComponent(typeof(Collider))]
public class CollisionDetector : MonoBehaviour
{
    [SerializeField] private TriggerEvent onTriggerEnter = new TriggerEvent();
    [SerializeField] private TriggerEvent onTriggerStay = new TriggerEvent();
    [SerializeField] private TriggerEvent onTriggerExit = new TriggerEvent();

    //Is TriggerがONで他のColliderとぶつかったときは、このメソッドがコールされる
    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter.Invoke(other);
    }

    //Is TriggerがONで他のColliderと重なっているときは、このメソッドが常にコールされる
    private void OnTriggerStay(Collider other)
    {
        onTriggerStay.Invoke(other);
    }

    //Is TriggerがONで他のColliderから離れたときは、このメソッドがコールされる
    private void OnTriggerExit(Collider other)
    {
        onTriggerExit.Invoke(other);
    }

    //UnityEventを継承したクラスに[Serialize]属性を付与することで、
    //Inspectorウィンドウ上に表示できるようになる
    [Serializable]
    public class TriggerEvent : UnityEvent<Collider>
    {
    }
}
