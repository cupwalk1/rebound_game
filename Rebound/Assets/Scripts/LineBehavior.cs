using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;



public class LineBehavior : MonoBehaviour
{

    public List<Dot> validDots = new();
    public IPlayer CurrentPlayer;
    private Game g;
    [SerializeField]
    private GameObject Button;
    private Vector3 currentPos;
    private IPlayer c {get => Player.CurrentPlayer; set => Player.CurrentPlayer = value; }

    void Start()
    {
        g = GameObject.Find("GameController").GetComponent<GameController>().CurrentGame;
    }

    void Update()
    {

        if (!g.inProgress)
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
        if (hit.collider.gameObject == g.currentDot.Instance)
        {
            g.OnBeginLine();
        }
    }

    private void OnTouchMoved(Touch touch, RaycastHit2D hit)
    {
        if (g.currentLine == null)
        {
            return;
        }
        Dot touchedDot = null;
        if (hit.collider != null)
        {
            touchedDot = g.availibleDots.FirstOrDefault(d => d.Instance == hit.collider.gameObject);
        }
        if (touchedDot == null)
        {
            g.OnDragLine(touch);
        }
        else if (touchedDot.AttachedLines.Count > 0)
        {
            g.OnBounce(touchedDot);
        }
        else
        {
            g.OnEndLine(touchedDot);
        }

    }

    private void OnTouchEnded()
    {

        if (g.currentLine == null)
        {
            return;
        }

        if (g.currentLine.GetEndDot() == null)
        {
            g.OnLineCanceled();
            //If the line is not connected to a dot
        }
        else if (g.currentLine.GetEndDot().AttachedLines.Count == 0)
        {
            //If the line is connected to a dot with no other lines
            g.OnLineReleased();
        }
    }






}
