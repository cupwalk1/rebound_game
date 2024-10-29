using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingBg : MonoBehaviour
{
    Rect _rect;
    // Start is called before the first frame update
    void Start()
    {
_rect = GetComponent<RawImage>().uvRect;
    }

    // Update is called once per frame
    void Update()
    {
        RawImage ri = GetComponent<RawImage>();
        _rect.y -= Time.deltaTime / 15;
        if (_rect.y < 0)
        {
            _rect.y += 1;
        }
        ri.uvRect = _rect;
        
    }
}
