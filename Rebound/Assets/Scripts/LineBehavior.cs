using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Serialization;


public class LineBehavior : MonoBehaviour
{

    public List<Dot> ValidDots = new();
    private Game _g;
    [FormerlySerializedAs("Button")] [SerializeField]
    private GameObject button;
    private Vector3 _currentPos;

    void Start()
    {
        _g = GameObject.Find("GameController").GetComponent<GameController>().CurrentGame;
    }

    void Update()
    {

        if (!_g.InProgress)
        {
            return;
        }
        
        if (Input.touchCount == 0)
        {
            return;
        }

        Touch touch = Input.GetTouch(0);
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                OnTouchBegan(hit);
                break;
            case TouchPhase.Moved:
                OnTouchMoved(touch, hit);
                break;
            case TouchPhase.Ended:
                OnTouchEnded();
                break;
            case TouchPhase.Canceled:
                OnTouchEnded();
                break;
        }

    }



    private void OnTouchBegan(RaycastHit2D hit)
    {
        if (hit.collider == null)
        {
            return;
        }
        if (hit.collider.gameObject == _g.CurrentDot.Instance)
        {
            _g.OnBeginLine();
        }
    }

    private void OnTouchMoved(Touch touch, RaycastHit2D hit)
    {
        if (_g.CurrentLine == null)
        {
            return;
        }
        Dot touchedDot = null;
        if (hit.collider != null)
        {
            touchedDot = _g.AvailableDots.FirstOrDefault(d => d.Instance == hit.collider.gameObject);
        }
        if (touchedDot == null)
        {
            _g.OnDragLine(touch);
        }
        else if (touchedDot.AttachedLines.Count > 0)
        {
            _g.OnBounce(touchedDot);
        }
        else
        {
            _g.OnEndLine(touchedDot);
        }

    }

    private void OnTouchEnded()
    {

        if (_g.CurrentLine == null)
        {
            return;
        }

        if (_g.CurrentLine.GetEndDot() == null)
        {
            _g.OnLineCanceled();
            //If the line is not connected to a dot
        }
        else if (_g.CurrentLine.GetEndDot().AttachedLines.Count == 0)
        {
            //If the line is connected to a dot with no other lines
            _g.OnLineReleased();
        }
    }






}
