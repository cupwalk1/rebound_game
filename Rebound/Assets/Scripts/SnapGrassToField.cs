using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapGrassToField : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BoardManager bm = GameObject.Find("BoardManager").GetComponent<BoardManager>();
        RawImage ri = GetComponent<RawImage>(); 
        RectTransform rt = GetComponent<RectTransform>();

        Rect rect = new();
        rect = ri.uvRect;
        float dotdistanceY = bm.spacing; 
        float H = (.125f*Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y)/(dotdistanceY);
        rect.height = H;
        ri.uvRect = rect;

    }
}