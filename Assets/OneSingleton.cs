using System;
using System.Diagnostics;
using UnityEngine;

[DebuggerStepThrough]
public abstract class OneSingleton<T> : MonoBehaviour where T : new()
{
    #region Fields
    private static Lazy<T> _instance = new Lazy<T>(() => new T());
    #endregion

    #region Properties
    public static T Instance
    {
        get => _instance.Value;
        set
        {
            _instance = new Lazy<T>(() => value);
        }
    }
    #endregion
}
