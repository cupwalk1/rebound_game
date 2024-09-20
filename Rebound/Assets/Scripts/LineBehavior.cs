using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;






public class LineBehavior : MonoBehaviour
{
    private GameController gc;
    private BoardManager boardManager;
    private Dot LastDot;
    public List<Dot> validDots = new();
    public bool gameInProgress = true;
    private Dot[,] board; // The game board
    public Camera mainCamera; // Assign this in the inspector
    public Dot startDot;
    public Dot currentDot;
    public int moveNo = 0;
    public Line currentLine;
    public Color playercolor;
    public Player CurrentPlayer;
    public GameObject WinnerText;
    public List<Dot> availibleDots;
    public bool canMoveAgain;
    private Game.IGameType game;
    [SerializeField]
    private GameObject Button;
    private bool hasMoved = false;



    void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        BoardManager.Instance.GenerateBoard();
        game = GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>().game.CurrentGame;
        Button.GetComponent<Button>().onClick.AddListener(Undo);
    }

    void Start()
    {
        boardManager = UnityEngine.GameObject.Find("BoardManager").GetComponent<BoardManager>();
        SetCurrentDot(GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>().game.CurrentGame.StartOfGameDot);
        CurrentPlayer = Player.P2;
        ChangePlayer();

    }

    void Update()
    {
        if (!gameInProgress)
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
                OnTouchBegan(touch, hit);
                break;
            case TouchPhase.Moved:
                OnTouchMoved(touch, hit);
                break;
            case TouchPhase.Ended:
                OnTouchEnded(touch, hit);
                break;
            case TouchPhase.Canceled:
                OnTouchEnded(touch, hit);
                break;
        }
    }

    private void Undo()
    {
        bool canUndo;
        Line lastLine = Line.LineHistory.Last();

        if (lastLine.GetEndDot() == null)
        {
            DestroyLine(lastLine);
            return;
        }

        if (Line.LineHistory.Count == 1)
        {
            canUndo = true;
        }
        
        else if (Line.LineHistory.Count == 0)
        {
            canUndo = false;
        }

        else if (Line.LineHistory[^2].RendererInstance.startColor == lastLine.RendererInstance.startColor && hasMoved)
        {
            if (lastLine.GetEndDot().AttachedLines.Count() == 1)
            {
                ChangePlayer();
            }
            canUndo = true;
        }

        else if (hasMoved)
        {
            canUndo = true;
            hasMoved = false;
        }
        
        else if (currentDot.AttachedLines.Count > 1)
        {
            canUndo = true;
        }
        else canUndo = false;
        if (canUndo)
        {
            DestroyLine(lastLine);
            if (Line.LineHistory.Count == 0)
            {
                SetCurrentDot(game.StartOfGameDot);
                return;
            }
            SetCurrentDot(Line.LineHistory.Last().GetEndDot());
        }
    }

    private void OnTouchBegan(Touch touch, RaycastHit2D hit)
    {
        if (hit.collider == null)
        {
            return;
        }
        if (hit.collider.gameObject == currentDot.Instance)
        {
            currentLine = new Line(currentDot, isGameLine: true);
            hasMoved = false;
        }
    }

    private void OnTouchMoved(Touch touch, RaycastHit2D hit)
    {
        if (currentLine == null)
        {
            return;
        }
        Dot touchedDot = null;
        if (hit.collider != null)
        {
            touchedDot = availibleDots.FirstOrDefault(d => d.Instance == hit.collider.gameObject);
        }
        if (touchedDot == null)
        {

            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            touchPos.z = 0;
            currentLine.SetEndDot(null, true);
            if (currentLine.RendererInstance != null) currentLine.RendererInstance.SetPosition(1, touchPos);
            return;
        }
        else if (touchedDot.AttachedLines.Count > 0)
        {
            hasMoved = true;
            currentLine.SetEndDot(touchedDot);
            game.CheckForWin();
            SetCurrentDot(touchedDot);
            if (BoardManager.Instance.GetOuterDots().Contains(currentDot))
            {
                currentDot.SetColor(playercolor);
                currentDot.Instance.GetComponent<SpriteRenderer>().sortingOrder = 10;
                currentLine.Instance.GetComponent<LineRenderer>().sortingOrder = 9;
            }
            currentLine = new Line(currentDot, isGameLine: true);
        }
        else
        {
            currentLine.SetEndDot(touchedDot, true);
        }

    }

    private void OnTouchEnded(Touch touch, RaycastHit2D hit)
    {
        if (currentLine == null)
        {
            return;
        }

        if (currentLine.GetEndDot() == null)
        {
            DestroyLine(currentLine);
            return;
        }

        if (currentLine.GetEndDot().AttachedLines.Count == 0)
        {
            hasMoved = true;
            currentLine.SetEndDot(currentLine.GetEndDot());
            SetCurrentDot(currentLine.GetEndDot());
            currentLine = null;
            ChangePlayer();
        }
    }


    void DestroyLine(Line line)
    {
        Line.LineHistory.Remove(line);
        line.GetStartDot().AttachedLines.Remove(line);
        line.GetEndDot()?.AttachedLines.Remove(line);
        Destroy(line.Instance);

    }

    private void SetCurrentDot(Dot dot)
    {
        currentDot = dot;
        availibleDots = currentDot.AvailibleDots();
    }

    public enum Player
    {
        P1,
        P2
    }

    public void ChangePlayer()
    {
        CurrentPlayer = CurrentPlayer == Player.P1 ? Player.P2 : Player.P1;
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("PlayerIndicator"))
        {
            i.GetComponent<Image>().color = PlayerPrefs.GetColor(CurrentPlayer);
        }
        playercolor = PlayerPrefs.GetColor(CurrentPlayer);

    }


}
public class Dot
{
    public static Dot[,] Board;
    public List<Line> AttachedLines { get; set; }
    public GameObject Instance { get; private set; }
    public float X { get; private set; }
    public float Y { get; private set; }
    public int BoardX { get; private set; }
    public int BoardY { get; private set; }
    public Color Color { get; set; }

    public Dot(float x, float y, int boardX, int boardY, GameObject instance)
    {
        AttachedLines = new List<Line>();
        Color = Color.white;
        X = x;
        Y = y;
        BoardX = boardX;
        BoardY = boardY;
        Instance = instance;
    }


    public List<Dot> GetNeighbours()
    {
        List<Dot> neighbours = new();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                int checkX = BoardX + x;
                int checkY = BoardY + y;
                if (checkX >= 0 && checkX < Board.GetLength(0) && checkY >= 0 && checkY < Board.GetLength(1) && Board[checkX, checkY] != null)
                {
                    neighbours.Add(Board[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }
    public List<Dot> AvailibleDots()
    {
        LineBehavior lb = UnityEngine.GameObject.Find("Controller").GetComponent<LineBehavior>();
        List<Dot> availibleDots = new();
        List<Dot> adjecentDots = GetNeighbours();
        foreach (Dot d in adjecentDots)
        {
            bool isConnectedToCurrentDot = false;
            //lb.currentLine.SetEndDot(d);
            if (d.AttachedLines.Count > d.GetNeighbours().Count - 2)
            {
                continue;
            }
            //if there is no line between dot and current dot

            foreach (Line l in d.AttachedLines)
            {
                if (l.GetStartDot() == this || l.GetEndDot() == this)
                {
                    isConnectedToCurrentDot = true;
                }
            }
            if (!isConnectedToCurrentDot)
            {
                availibleDots.Add(d);
            }

            //lb.currentLine.SetEndDot(null);
        }
        return availibleDots;
    }
    public int GetBoardX()
    {

        return BoardX;
    }
    public int GetBoardY()
    {
        return BoardY;
    }
    public List<Line> GetAttachedLines()
    {
        return AttachedLines;
    }

    public void SetColor(Color color)
    {
        Color = color;
        Instance.GetComponent<SpriteRenderer>().color = color;
    }



}

public class Line
{

    public GameObject Instance { get; private set; }
    public LineRenderer RendererInstance { get; private set; }
    public Dot StartDot { get; private set; }
    public Dot EndDot { get; set; }
    public static List<Line> LineHistory = new();

    public Line(Dot startDot, Dot endDot = null, bool isGameLine = false)
    {
        if (isGameLine)
        {
            LineHistory.Add(this);
        }
        Instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Line"), startDot.Instance.transform.position, Quaternion.identity);
        RendererInstance = Instance.GetComponent<LineRenderer>();
        RendererInstance.loop = false;
        RendererInstance.positionCount = 2;
        LineBehavior lb = UnityEngine.GameObject.Find("Controller").GetComponent<LineBehavior>();
        RendererInstance.startColor = lb.playercolor;
        RendererInstance.endColor = lb.playercolor;
        SetStartDot(startDot);
        SetEndDot(endDot);
        if (GetEndDot() == null) RendererInstance.SetPosition(1, GetStartDot().Instance.transform.position);
    }
    public void SetColor(Color color)
    {
        RendererInstance.startColor = color;
        RendererInstance.endColor = color;
    }
    public Dot GetStartDot()
    {
        return StartDot;
    }
    public Dot GetEndDot()
    {
        return EndDot;
    }
    public void SetEndDot(Dot dot, bool NoAttachment = false)
    {
        EndDot = dot;
        if (dot == null) return;
        if (!NoAttachment) dot.AttachedLines.Add(this);

        Instance.GetComponent<LineRenderer>().SetPosition(1, dot.Instance.transform.position);
    }
    public void SetStartDot(Dot dot)
    {
        StartDot = dot;
        dot.AttachedLines.Add(this);
        Instance.GetComponent<LineRenderer>().SetPosition(0, dot.Instance.transform.position);
    }
    public GameObject GetInstance()
    {
        return Instance;
    }
    public LineRenderer GetRendererInstance()
    {
        return RendererInstance;
    }

}
