using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Unity.VisualScripting;
using JetBrains.Annotations;
using System.Buffers;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using UnityEditor;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {

                GameObject obj = new()
                {
                    name = typeof(T).Name
                };
                _instance = obj.AddComponent<T>();

            }
            return _instance;
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}

