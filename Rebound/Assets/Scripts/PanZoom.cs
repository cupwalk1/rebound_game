using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoom : MonoBehaviour
{
    Vector3 touchStart;
    public float zoomOutMin = 2;
    public float zoomOutMax = 5;
    private float boundsLeft;
    private float boundsRight;
    private float boundsTop;
    private float boundsBottom;
    [SerializeField] float height;
    [SerializeField] float width;
    public GameObject bounds;
    private RectTransform rt;
    private bool isZoom;
    private Touch touchZero;
    private Touch touchOne;
    private bool touchingGameObjects;
    // Update is called once per frame

private void Start()
{
    bounds = GameObject.Find("Background");
    RectTransform rt = GetComponent<RectTransform>();
    rt = bounds.GetComponent<RectTransform>();
    touchingGameObjects = true;
    float newWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
    float newHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
}

    void Update()
    {
        rt = bounds.GetComponent<Canvas>().GetComponent<RectTransform>();
        if (Input.touchCount == 0)
        {
            touchingGameObjects = false;
        }
        width = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        height = (Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y - Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y);
        boundsLeft = -(rt.rect.width / 2 * rt.localScale.x - width / 2);
        boundsRight = -boundsLeft;
        boundsBottom = -(rt.rect.height / 2 * rt.localScale.y - height / 2);
        boundsTop = -boundsBottom;
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

            RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

            if (hitInformation.collider != null)
            {
                GameObject touchedObject = hitInformation.transform.gameObject;
            
                foreach (var d in GameObject.Find("Controller").GetComponent<LineBehavior>().validDots)
                {
                    if (touchedObject == d.Instance)
                    {
                        Debug.Log(GameObject.Find("Controller").GetComponent<LineBehavior>().validDots.Count);
                        touchingGameObjects = true;
                    }
                    else
                    {
                        touchingGameObjects = false;
                        touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
                    }
                }
                
            }
        }
        if (Input.touchCount == 2)
        {
            isZoom = true;
            touchZero = Input.GetTouch(0);
            touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            zoom(difference * 0.002f);
        }
        else if ((touchZero.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Ended) && isZoom && !touchingGameObjects)
        {
            isZoom = false;
            //touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0) && !isZoom && !touchingGameObjects)
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
            Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x, boundsLeft, boundsRight), Mathf.Clamp(Camera.main.transform.position.y, boundsBottom, boundsTop), Camera.main.transform.position.z);
        }
        zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
        Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x, boundsLeft, boundsRight), Mathf.Clamp(Camera.main.transform.position.y, boundsBottom, boundsTop), Camera.main.transform.position.z);
    }
}