using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

}
