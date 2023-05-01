using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T>: MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if(Instance == null)
        {
            Instance = this as T;
        }
        else
        {
            Debug.LogWarning($"More than one {typeof(T).ToString()} in the scene");
        }
    }
}