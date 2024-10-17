using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using static Player;

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
    public List<Dot> LoseDots = new();
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
        LoseDots = new();
        List<Dot> availibleDots = new();
        List<Dot> adjecentDots = GetNeighbours();
        foreach (Dot d in adjecentDots)
        {
            if (d.AttachedLines.Count > d.GetNeighbours().Count - 2)
            {
                Debug.Log("Lose dot");
                LoseDots.Add(d);
                continue;
            }

            bool isConnectedToCurrentDot = false;
            //lb.currentLine.SetEndDot(d);
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
        if (availibleDots.Count == 0)
        {
            return LoseDots;
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