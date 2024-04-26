using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 继承Mono的单例
/// </summary>
public class UnitySingleBase<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                instance = obj.AddComponent<T>();
            }
            return instance;
        }
    }
}