using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingBG : MonoBehaviour
{
    Rect rect;
    // Start is called before the first frame update
    void Start()
    {
rect = GetComponent<RawImage>().uvRect;
    }

    // Update is called once per frame
    void Update()
    {
        RawImage ri = GetComponent<RawImage>();
        rect.y -= Time.deltaTime / 15;
        if (rect.y < 0)
        {
            rect.y += 1;
        }
        ri.uvRect = rect;
        
    }
}
