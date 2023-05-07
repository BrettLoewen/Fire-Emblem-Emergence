using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to define a static singleton instance for a given script
/// Note: the child class must class `base.Awake()` in its Awake method
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T>: MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// A static reference to the only instance of this class
    /// </summary>
    public static T Instance { get; private set; }

    // Awake is called before Start before the first frame update
    protected virtual void Awake()
    {
        // If there is currently no static instance, then this instance should be the static instance
        if(Instance == null)
        {
            Instance = this as T;
        }
        // If there is already a static instance, somthing has gone wrong; throw an error
        else
        {
            Debug.LogWarning($"More than one {typeof(T)} in the scene");
        }
    }//end Awake
}