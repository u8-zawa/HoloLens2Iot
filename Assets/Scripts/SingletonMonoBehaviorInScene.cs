using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// シーン上に一つしか無いオブジェクトとして振る舞うコンポーネント
// 継承すると、クラス名.Instance でアクセス可能になる
public abstract class SingletonMonoBehaviorInScene<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            // シーン上に２つ以上インスタンスが存在する場合はエラー
            throw new Exception("シーン内に他のインスタンスが存在します！");
        }

        Instance = this as T;
    }
}
