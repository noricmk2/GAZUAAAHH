using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> 
{
    static bool shotdown = false;
    private static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                if (shotdown == false)
                {
                    T instance = GameObject.FindObjectOfType<T>() as T;
                    if (instance == null)
                    {
                        instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    }

                    InstanceInit(instance);
                    Debug.Assert(_instance != null, typeof(T).ToString() + "singleton create fail");
                }
            }
            return _instance;
        }
    }

    private static void InstanceInit(Object inst)
    {
        _instance = inst as T;
        _instance.Init();
    }

    public virtual void Init()
    {
        DontDestroyOnLoad(_instance);
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    private void OnApplicationQuit()
    {
        _instance = null;
        shotdown = true;
    }

    public virtual void CustomUpdate()
    {

    }
}
