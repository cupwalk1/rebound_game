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


public class LineBehavior : MonoBehaviour
{
    public List<Dot> validDots = new();
    public float spacing;
    [SerializeField] private GameObject UndoButtonGO;
    public List<Line> AllGameLines = new();
    public Dot startOfTurnDot;
    public bool gameInProgress = true;
    public GameObject WinnerText;
    public Dictionary<GameObject, Dot> lineMap = new();
    private Line[] Lines = new Line[1000];
    public Dot center;
    public GameObject Background;
    private Dot[,] board; // The game board
    private int currentPlayer; // The current player
    public Camera mainCamera; // Assign this in the inspector
    public GameObject dotPrefab; // Assign this in the inspector
    public float widthf;
    public float heightf;
    public GameObject linePrefab;
    private LineRenderer outlineRenderer;
    private Vector2 worldPos;
    public GameObject dotParent;
    public Dot startDot;
    public Dot currentDot;
    public int moveNo = 0;
    public Line currentLine;
    public string player = null;
    public Color playercolor;
    public bool currentLineValid = true;
    public List<Dot> P1Goal = new();
    public List<Dot> P2Goal = new();
    public Dot endTurnDot;


    void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        UndoButtonGO.GetComponent<Button>().onClick.AddListener(UndoButton);
        WinnerText = GameObject.Find("WinnerText");
        WinnerText.SetActive(false);
        playercolor = PlayerPrefs.GetColor(player);
        GenerateBoard();
        startDot = center;
        currentDot = center;
    }

    void LateUpdate()
    {
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            touchPos.z = 0;

            if (touch.phase == TouchPhase.Began)
            {
                currentDot.Instance.GetComponent<CircleCollider2D>().radius = 3f;
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
                currentDot.Instance.GetComponent<CircleCollider2D>().radius = 1.5f;

                if (hit.collider != null && hit.collider.transform.gameObject == currentDot.Instance)
                {
                    
                    CreateLine(currentDot);
                    currentLineValid = true;

                }
            }

        }
    }

    public void GenerateBoard()
    {

        widthf = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x * 2;
        heightf = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y * 2;
        GameObject outline = Instantiate(linePrefab);
        outlineRenderer = outline.GetComponent<LineRenderer>();
        outlineRenderer.startWidth = 0.1f;
        outlineRenderer.endWidth = 0.1f;
        outlineRenderer.numCornerVertices = 10;
        int boardx = 0;
        int boardy = 0;
        float spacingX = widthf / (9 + 1);
        float spacingY = heightf / (19 + 1);
        board = new Dot[10, 20];
        spacing = Mathf.Min(spacingX, spacingY);

        for (int x = boardx; x < 9; x++)
        {
            for (int y = boardy; y < 19; y++) // Adjust the loop condition
            {
                worldPos = new Vector2(x * spacing, y * spacing);
                GameObject dotInstance = Instantiate(dotPrefab, new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
                dotInstance.transform.SetParent(dotParent.transform);
                board[x + 1, y + 1] = new Dot(dotInstance, worldPos.x, worldPos.y, x + 1, y + 1, new());
                lineMap.Add(dotInstance, board[x + 1, y + 1]);
                dotInstance.name = "Dot (" + (x + 1) + ", " + (y + 1) + ")";
            }
        }
        center = board[5, 10];
        currentDot = center;
        ChangePlayer();

        dotParent.transform.position = -center.Instance.transform.position;

        // Disable the dots that are not part of the game board
        board[1, 1].Instance.SetActive(false);
        board[2, 1].Instance.SetActive(false);
        board[8, 1].Instance.SetActive(false);
        board[9, 1].Instance.SetActive(false);
        board[1, 19].Instance.SetActive(false);
        board[2, 19].Instance.SetActive(false);
        board[8, 19].Instance.SetActive(false);
        board[9, 19].Instance.SetActive(false);

        // Create the border lines
        List<Dot> BorderDots = new();
        BorderDots.Add(board[1, 2]);
        BorderDots.Add(board[1, 3]);
        BorderDots.Add(board[1, 4]);
        BorderDots.Add(board[1, 5]);
        BorderDots.Add(board[1, 6]);
        BorderDots.Add(board[1, 7]);
        BorderDots.Add(board[1, 8]);
        BorderDots.Add(board[1, 9]);
        BorderDots.Add(board[1, 10]);
        BorderDots.Add(board[1, 11]);
        BorderDots.Add(board[1, 12]);
        BorderDots.Add(board[1, 13]);
        BorderDots.Add(board[1, 14]);
        BorderDots.Add(board[1, 15]);
        BorderDots.Add(board[1, 16]);
        BorderDots.Add(board[1, 17]);
        BorderDots.Add(board[1, 18]);
        BorderDots.Add(board[2, 18]);
        BorderDots.Add(board[3, 18]);
        BorderDots.Add(board[3, 19]);
        BorderDots.Add(board[4, 19]);
        BorderDots.Add(board[5, 19]);
        BorderDots.Add(board[6, 19]);
        BorderDots.Add(board[7, 19]);
        BorderDots.Add(board[7, 18]);
        BorderDots.Add(board[9, 18]);
        BorderDots.Add(board[9, 17]);
        BorderDots.Add(board[9, 16]);
        BorderDots.Add(board[9, 15]);
        BorderDots.Add(board[9, 14]);
        BorderDots.Add(board[9, 13]);
        BorderDots.Add(board[9, 12]);
        BorderDots.Add(board[9, 11]);
        BorderDots.Add(board[9, 10]);
        BorderDots.Add(board[9, 9]);
        BorderDots.Add(board[9, 8]);
        BorderDots.Add(board[9, 7]);
        BorderDots.Add(board[9, 6]);
        BorderDots.Add(board[9, 5]);
        BorderDots.Add(board[9, 4]);
        BorderDots.Add(board[9, 3]);
        BorderDots.Add(board[9, 2]);
        BorderDots.Add(board[8, 2]);
        BorderDots.Add(board[7, 2]);
        BorderDots.Add(board[7, 1]);
        BorderDots.Add(board[6, 1]);
        BorderDots.Add(board[5, 1]);
        BorderDots.Add(board[4, 1]);
        BorderDots.Add(board[3, 1]);
        BorderDots.Add(board[3, 2]);
        BorderDots.Add(board[2, 2]);
        BorderDots.Add(board[1, 2]);


        P1Goal.Add(board[3, 2]);
        P1Goal.Add(board[4, 2]);
        P1Goal.Add(board[5, 2]);
        P1Goal.Add(board[6, 2]);
        P1Goal.Add(board[7, 2]);


        P2Goal.Add(board[3, 18]);
        P2Goal.Add(board[4, 18]);
        P2Goal.Add(board[5, 18]);
        P2Goal.Add(board[6, 18]);
        P2Goal.Add(board[7, 18]);

        ConnectBorderDots(P1Goal, PlayerPrefs.GetColor("P1"));
        ConnectBorderDots(P2Goal, PlayerPrefs.GetColor("P2"));
        ConnectBorderDots(BorderDots, Color.white);

    }

    void DestroyLine(Line line)
    {
        Debug.Log("Destroying line");
        line.GetStartDot().AttachedLines.Remove(line);
        line.GetEndDot().AttachedLines.Remove(line);
        Destroy(line.Instance);
    }
    public void ConnectBorderDots(List<Dot> dots, Color color)
    {
        for (int i = 0; i < dots.Count; i++)
        {
            if (i == dots.Count - 1)
            {
                break;
            }
            GameObject newlineobj = Instantiate(linePrefab, dots[i].Instance.transform.position, Quaternion.identity);
            Line line = new(newlineobj, dots[i], dots[i]);
            line.GetRendererInstance().positionCount = 2;
            line.SetColor(color);
            line.SetStartDot(dots[i]);
            line.SetEndDot(dots[i + 1]);
            dots[i].GetAttachedLines().Add(line);
            dots[i + 1].GetAttachedLines().Add(line);
        }
    }
    void UndoButton()
    {
        Debug.Log("Pos: " + startOfTurnDot.GetBoardX() + " " + startOfTurnDot.GetBoardY());
        if (AllGameLines.Count < 1)
        {
            return;
        }
        Line lineToUndo = AllGameLines.Last();
        if (lineToUndo.GetEndDot() == startOfTurnDot)
        {
            return;
        }
        AllGameLines.Remove(lineToUndo);
        endTurnDot = AllGameLines.Last().GetEndDot();
        currentDot = endTurnDot;
        DestroyLine(lineToUndo);
        
        UpdateUndoButton();
        if (lineToUndo.GetStartDot() == startOfTurnDot){
        ChangePlayer();
        }



    }

    public void CreateLine(Dot start)
    {
        GameObject newlineobj = Instantiate(linePrefab, start.Instance.transform.position, Quaternion.identity);
        Line line = new(newlineobj, start, start);
        line.GetRendererInstance().loop = false;
        line.GetRendererInstance().startColor = playercolor;
        line.GetRendererInstance().endColor = playercolor;
        if (currentDot == startDot)
        {
            line.Instance.GetComponent<LineScript>().currentLineValid = true;
        }
        line.Instance.GetComponent<LineScript>().SetOrigin(line);
        currentLine = line;
    }
    public void UpdateUndoButton()
    {

        if (AllGameLines.Count < 1)
        {
            UndoButtonGO.GetComponent<Button>().interactable = false;
            return;
        }
        Line lineToUndo = AllGameLines.Last();
        if (lineToUndo.GetEndDot() == startOfTurnDot)
        {
            Debug.Log("Reached start of turn: " + startOfTurnDot.GetBoardX() + " " + startOfTurnDot.GetBoardY());
            UndoButtonGO.GetComponent<Button>().interactable = false;
            return;
        }
        Debug.Log("Undo button enabled");
        UndoButtonGO.GetComponent<Button>().interactable = true;

    }

    public (Dot, bool) CheckAdjecent(Dot center)
    {
        Touch touch = Input.GetTouch(0);
        List<Dot> adjecentDots = new();
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
        for (int x = 1; x < board.GetLength(0); x++)
        {
            for (int y = 1; y < board.GetLength(1); y++)
            {

                int xpos = center.GetBoardX();
                int ypos = center.GetBoardY();
                if (Math.Abs(xpos - x) <= 1 && Math.Abs(ypos - y) <= 1 && board[x, y] != currentDot)
                {
                    adjecentDots.Add(board[x, y]);
                }
            }
        }
        if (hit.collider != null && lineMap.ContainsKey(hit.collider.gameObject))
        {
            ResetDotColors();
            Dot hitDot = lineMap[hit.collider.gameObject];
            validDots = DotPlacementConditions(adjecentDots, center);
            foreach (Dot d in validDots)
            {
                d.Instance.GetComponent<SpriteRenderer>().color = playercolor;
            }
            if (validDots.Contains(hitDot))
            {
                return (hitDot, true);
            }
            else
            {
                return (null, false);
            }
        }

        return (null, false);
    }
    public void ChangePlayer()
    {
        if (player == "P1")
        {
            player = "P2";
        }
        else
        {
            player = "P1";
        }
        currentLineValid = false;
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("PlayerIndicator"))
        {
            i.GetComponent<Image>().color = PlayerPrefs.GetColor(player);
        }
        playercolor = PlayerPrefs.GetColor(player);
        currentDot.SetColor(playercolor);
    }


    List<Dot> DotPlacementConditions(List<Dot> dots, Dot center)
    {
        List<Dot> modifiedDots = dots;
        List<Dot> dotsToRemove = new List<Dot>();

        foreach (Dot d in dots)
        {
            if (d.GetAttachedLines().Count > 6)
            {
                dotsToRemove.Add(d);
            }
            foreach (Line l in d.GetAttachedLines())
            {
                if ((l.GetStartDot() == center && l.GetEndDot() == d) || (l.GetEndDot() == center && l.GetStartDot() == d))
                {
                    if (modifiedDots.Contains(d))
                    {

                        dotsToRemove.Add(d);
                    }
                }
            }
        }

        foreach (Dot d in dotsToRemove)
        {
            modifiedDots.Remove(d);
        }
        return modifiedDots;
    }
    public void ResetDotColors()
    {
        for (int x = 1; x < board.GetLength(0); x++)
        {
            for (int y = 1; y < board.GetLength(1); y++)
            {
                board[x, y].Instance.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

    }

}


public class Dot
{
    public List<Line> AttachedLines { get; set; }
    public GameObject Instance { get; private set; }
    public float X { get; private set; }
    public float Y { get; private set; }
    public int BoardX { get; private set; }
    public int BoardY { get; private set; }
    public Color Color { get; set; }

    public Dot(GameObject instance, float x, float y, int boardX, int boardY, List<Line> attachedlines)
    {
        Color = Color.white;
        Instance = instance;
        X = x;
        Y = y;
        BoardX = boardX;
        BoardY = boardY;
        AttachedLines = attachedlines;
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

    public Line(GameObject instance, Dot startDot, Dot endDot)
    {
        Instance = instance;
        RendererInstance = instance.GetComponent<LineRenderer>();
        StartDot = startDot;
        EndDot = endDot;

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
    public void SetEndDot(Dot dot)
    {
        EndDot = dot;
        Instance.GetComponent<LineRenderer>().SetPosition(1, dot.Instance.transform.position);
    }
    public void SetStartDot(Dot dot)
    {
        StartDot = dot;
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

