using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapGrassToField : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject controller = GameObject.Find("Controller");
        LineBehavior lb = controller.GetComponent<LineBehavior>();
        RawImage ri = GetComponent<RawImage>(); 
        RectTransform rt = GetComponent<RectTransform>();
        Rect rect = new Rect(0, 0.125f, 1, 1);
        float dotdistanceY = lb.spacing; //x = z
        rect.height = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).y  / (2 * dotdistanceY);
        rect.width  = rect.height * rt.rect.width / rt.rect.height; 
        ri.uvRect = rect;
    }
}
