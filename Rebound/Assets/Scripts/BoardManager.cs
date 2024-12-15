using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BoardManager : Singleton<BoardManager>
{
    public float spacing;
    public List<Dot> OrderedList;
    private GameObject _dotPrefab;
    public int coulumns;
    public int rows;
    public int boxesX;
    public int boxesY; 


    private void Awake()
    {
        _dotPrefab = Resources.Load<GameObject>("Prefabs/Dot");

    }

    public void GenerateBoard()
    {
    
        Game currentGame = GameController.Instance.CurrentGame.GetComponent<Game>();
        
        _ = Instantiate(currentGame.Background, new Vector3(0, 0, 0), Quaternion.identity);
        coulumns = currentGame.BoardWidth;
        rows = currentGame.BoardHeight;
        //Create Centered Array of Dots
        Dot.Board = new Dot[coulumns, rows];
        float width = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x * 2;
        float height = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y * 2;
        float spacingX = width / (coulumns + 1);
        float spacingY = height / (rows + 1);
        spacing = Mathf.Min(spacingX, spacingY);


        float offsetX = Mathf.FloorToInt(coulumns / 2) * spacing;
        float offsetY = Mathf.FloorToInt(rows / 2) * spacing;
        boxesX = coulumns - 1;
        boxesY = rows - 1;



        for (int x = 0; x < coulumns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector2 worldPos = new((x * spacing) - offsetX, (y * spacing) - offsetY);
                GameObject instance = Instantiate(_dotPrefab, new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
                Dot.Board[x, y] = new Dot(worldPos.x, worldPos.y, x, y, instance);
                Dot.Board[x, y].Instance.name = "Dot (" + x + ", " + y + ")";
                Dot.Board[x, y].Instance.GetComponent<CircleCollider2D>().radius = spacing / 2.2f / Dot.Board[x, y].Instance.transform.localScale.x;
            }
        }

        currentGame.CustomBoardSetup(boxesX, boxesY);
        List<Dot> outerDots = new();
        // Find Dots that are not adjacent to 8 dots and put them in list
        for (int x = 0; x < coulumns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (Dot.Board[x, y] == null) continue;
                if (Dot.Board[x, y].GetNeighbours().Count < 8)
                {
                    outerDots.Add(Dot.Board[x, y]);
                }
            }
        }

        // Order list finding neighbor outer dots one after another
        OrderedList = new() { outerDots[0] };
        Dot currentDot = outerDots[0];
        for (int i = 0; i < outerDots.Count; i++)
        {
            // Made list of adjacent dots to CurrentDot
            List<Dot> neighbours = currentDot.GetNeighbours();
            List<Dot> adjacentList = new();
            foreach (Dot dot in neighbours)
            {
                if (dot.X == currentDot.X || dot.Y == currentDot.Y)
                {
                    adjacentList.Add(dot);
                }
            }
            adjacentList = adjacentList.Distinct().ToList();

            foreach (Dot dot in adjacentList)
            {
                if (outerDots.Contains(dot) && !OrderedList.Contains(dot))
                {
                    OrderedList.Add(dot);
                    currentDot = dot;
                    break;
                }
            }
        }
        DrawOuterDiagonalDots(OrderedList);

        // Connect the dots in the ordered list
        ConnectBorderDots(OrderedList, Color.white);
        
        GameController.Instance.OnBoardGenerated.Invoke();
    }

    private void DrawOuterDiagonalDots(List<Dot> orderedList)
    {
        for (int i = 0; i < orderedList.Count; i++)
        {
            Dot prevDot;
            Dot nextDot;

            Dot currentDot = orderedList[i];
            if (i == 0) prevDot = orderedList[^1];
            else prevDot = orderedList[i - 1];
            if (i == orderedList.Count - 1) nextDot = orderedList[0];
            else nextDot = orderedList[i + 1];

            Vector2 direction = new(currentDot.BoardX - prevDot.BoardX, currentDot.BoardY - prevDot.BoardY);
            Vector2 direction2 = new(nextDot.BoardX - currentDot.BoardX, nextDot.BoardY - currentDot.BoardY);
            bool inturn = direction2.x == -direction.y && direction2.y == direction.x;
            if (inturn)
            {
                Line l = new (Player.None, prevDot, nextDot);
                l.SetColor(Color.clear);
            }
        }
    }

    public void ConnectBorderDots(List<Dot> dots, Color color)
    {
        for (int i = 0; i < dots.Count; i++)
        {
            dots[i].Instance.GetComponent<SpriteRenderer>().sortingOrder = 9;
            Line line;
            if (i == dots.Count - 1)
            {
                line = new Line(Player.None, dots[i], dots[0]);
                line.SetColor(color);
                break;
            }

            Dot currentDot = dots[i];
            Dot nextDot = dots[i + 1];

            // Check for outward turn and draw diagonal line
            line = new Line(Player.None, currentDot, nextDot);
            line.SetColor(color);
        }
    }

    public List<Dot> GetOuterDots()
    {
        return OrderedList;
    }

}