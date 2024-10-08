using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;



public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public BoardManager boardManager;
    public Game game;
    public Game.GameType GameMode;

    public GameObject SoccerBackground;
    public GameObject SumoBackground;
    public GameObject GolfBackground;
    public GameObject PoolBackground;
    public GameObject SoccerBlitzBackground;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void StartGame(Game.GameType GameMode)
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            Debug.Log("Game Already Started");
            return;
        }
        SceneManager.LoadScene("GameScene");
        game = new Game(GameMode);

    }


}

public class Game
{
    public GameType GameMode;
    public IGameType CurrentGame;
    public TurnState CurrentTurnState;
    public Dot StartGameDot;

    public Game(GameType gameType)
    {
        GameMode = gameType;
        switch (GameMode)
        {
            case GameType.Soccer:
                CurrentGame = new Soccer();
                break;
            case GameType.Sumo:
                CurrentGame = new Sumo();
                break;
            case GameType.Golf:
                CurrentGame = new Golf();
                break;
            case GameType.Pool:
                CurrentGame = new Pool();
                break;
            case GameType.SoccerBlitz:
                CurrentGame = new SoccerBlitz();
                break;
        }
        StartGameDot = CurrentGame.StartOfGameDot;
    }

    public void OnVictory(LineBehavior.Player winner)
    {
        LineBehavior lb = GameObject.Find("Controller").GetComponent<LineBehavior>();
        GameObject[] particles = GameObject.FindGameObjectsWithTag("WinnerCoriandoli");
        lb.gameInProgress = false;
        lb.WinnerText.SetActive(true);
        GameObject.Find("UI/backgroundWin").GetComponent<Image>().enabled = true;
        lb.WinnerText.GetComponent<TMP_Text>().text = winner + " Wins!";
        if (winner == LineBehavior.Player.P1)
        {
            GameObject.Find("Controller").GetComponent<LineBehavior>().WinnerText.GetComponent<TMP_Text>().color = PlayerPrefs.GetColor(LineBehavior.Player.P1);
        }
        else
        {
            GameObject.Find("Controller").GetComponent<LineBehavior>().WinnerText.GetComponent<TMP_Text>().color = PlayerPrefs.GetColor(LineBehavior.Player.P2);
        }

        foreach (GameObject particle in particles)
        {
            particle.GetComponent<ParticleSystem>().Play();
        }
    }

    public enum GameType
    {
        Soccer,
        Sumo,
        Golf,
        Pool,
        SoccerBlitz
    }
    public enum TurnState
    {
        AwaitTouch,
        Dragging,
        Snapped
    }

    public interface IGameType
    {
        Dot StartOfGameDot { get; set; }
        int BoardHeight { get; set; }
        int BoardWidth { get; set; }
        GameObject Background { get; set; }
        void CustomBoardSetup(int BoxesX, int BoxesY);
        void CustomRules();
        void CheckForWin();
        GameObject CurrentDotCover { get; set; }
    }

    public class Soccer : IGameType
    {
        public Soccer()
        {
            gameController = UnityEngine.GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>();
            BoardHeight = 13;
            BoardWidth = 7;
            CurrentDotCover = Resources.Load<GameObject>("Prefabs/SoccerBall");
            Background = Resources.Load<GameObject>("Prefabs/SoccerBackground");
        }
        public GameController gameController { get; set; }
        public GameObject Background { get; set; }
        public List<Dot> P1GoalDots = new();
        public List<Dot> P2GoalDots = new();
        public Dot StartOfGameDot { get; set; }
        public int BoardHeight { get; set; }
        public int BoardWidth { get; set; }
        public GameObject CurrentDotCover { get; set; }

        public void CustomBoardSetup(int boxesX, int boxesY)
        {
            int goalBoxes = Mathf.CeilToInt((boxesX / 3f) / 2) * 2;
            for (int i = 0; i < (boxesX - goalBoxes) / 2; i++)
            {
                UnityEngine.Object.Destroy(Dot.Board[i, 0].Instance);
                UnityEngine.Object.Destroy(Dot.Board[i, boxesY].Instance);
                UnityEngine.Object.Destroy(Dot.Board[boxesX - i, 0].Instance);
                UnityEngine.Object.Destroy(Dot.Board[boxesX - i, boxesY].Instance);
                Dot.Board[i, 0] = null;
                Dot.Board[i, boxesY] = null;
                Dot.Board[boxesX - i, 0] = null;
                Dot.Board[boxesX - i, boxesY] = null;
            }
            StartOfGameDot = Dot.Board[(boxesX / 2), boxesY / 2];

            for (int i = 0; i <= goalBoxes; i++)
            {
                P1GoalDots.Add(Dot.Board[(boxesX - goalBoxes) / 2 + i, boxesY - 1]);
                P2GoalDots.Add(Dot.Board[(boxesX - goalBoxes) / 2 + i, 1]);
            }
            for (int i = 0; i < P1GoalDots.Count; i++)
            {
                P1GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = PlayerPrefs.GetColor(LineBehavior.Player.P1);
                if (i == 0)
                {
                    P1GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Color.white;
                }
                if (i == P1GoalDots.Count - 1)
                {
                    P1GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Color.white;
                    continue;
                }
                Line l = new(P1GoalDots[i], P1GoalDots[i + 1]);
                l.SetColor(PlayerPrefs.GetColor(LineBehavior.Player.P1));

            }
            for (int i = 0; i < P2GoalDots.Count; i++)
            {
                P2GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = PlayerPrefs.GetColor(LineBehavior.Player.P2);
                if (i == 0)
                {
                    P2GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Color.white;
                }
                if (i == P2GoalDots.Count - 1)
                {
                    P2GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Color.white;
                    continue;
                }
                Line l = new(P2GoalDots[i], P2GoalDots[i + 1]);
                l.SetColor(PlayerPrefs.GetColor(LineBehavior.Player.P2));
                l.Instance.GetComponent<LineRenderer>().sortingOrder = 8;
            }
            //

        }

        public void CustomRules()
        {
            throw new NotImplementedException();
        }

        public void CheckForWin()
        {
            LineBehavior lb = UnityEngine.GameObject.Find("Controller").GetComponent<LineBehavior>();
            LineBehavior.Player winner;
            if (P1GoalDots.Contains(lb.currentLine.EndDot) && lb.currentLine.RendererInstance.startColor == PlayerPrefs.GetColor(LineBehavior.Player.P1))
            {
                winner = LineBehavior.Player.P1;
            }
            else if (P2GoalDots.Contains(lb.currentLine.EndDot) && lb.currentLine.RendererInstance.startColor == PlayerPrefs.GetColor(LineBehavior.Player.P2))
            {
                winner = LineBehavior.Player.P2;
            }
            else
            {
                return;
            }
            gameController.game.OnVictory(winner);

        }
    }

    public class Sumo : IGameType
    {
        public Sumo()
        {
            BoardHeight = 7;
            BoardWidth = 7;
            Background = Resources.Load<GameObject>("Prefabs/SumoBackground");
            CurrentDotCover = Resources.Load<GameObject>("Prefabs/SumoDot");

        }
        public GameObject Background { get; set; }
        public Dot StartOfGameDot { get; set; }
        public int BoardHeight { get; set; }
        public int BoardWidth { get; set; }
        public GameObject CurrentDotCover { get; set; }

        public void CustomBoardSetup(int boxesX, int boxesY)
        {
            StartOfGameDot = Dot.Board[BoardWidth / 2, BoardHeight / 2];
        }

        public void CustomRules()
        {
            throw new NotImplementedException();
        }

        public void CheckForWin()
        {
            return;
        }
    }

    public class Golf : IGameType
    {
        public Golf()
        {
            BoardHeight = 15;
            BoardWidth = 7;
            Background = Resources.Load<GameObject>("Prefabs/GolfBackground");
        }
        public GameObject Background { get; set; }
        public Dot StartOfGameDot { get; set; }
        public int BoardHeight { get; set; }
        public int BoardWidth { get; set; }
        public GameObject CurrentDotCover { get; set; }

        public void CustomBoardSetup(int boxesX, int boxesY)
        {
            throw new NotImplementedException();
        }

        public void CustomRules()
        {
            throw new NotImplementedException();
        }

        public void CheckForWin()
        {
            throw new NotImplementedException();
        }
    }

    public class Pool : IGameType
    {
        public Pool()
        {
            gameController = UnityEngine.GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>();
            BoardHeight = 15;
            BoardWidth = 7;
            Background = Resources.Load<GameObject>("Prefabs/PoolBackground");
        }
        public GameController gameController { get; set; }
        public GameObject Background { get; set; }
        public Dot StartOfGameDot { get; set; }
        public int BoardHeight { get; set; }
        public int BoardWidth { get; set; }
        public List<Dot> PocketDots = new();

        public GameObject CurrentDotCover { get; set; }
        public void CustomBoardSetup(int boxesX, int boxesY)
        {

            StartOfGameDot = Dot.Board[boxesX / 2, boxesY / 2];

            //add 6 pockets
            PocketDots.Add(Dot.Board[0, 0]);
            PocketDots.Add(Dot.Board[0, boxesY]);
            PocketDots.Add(Dot.Board[boxesX, 0]);
            PocketDots.Add(Dot.Board[boxesX, boxesY]);
            PocketDots.Add(Dot.Board[0, boxesY / 2]);
            PocketDots.Add(Dot.Board[boxesX, boxesY / 2]);

            foreach (Dot pocket in PocketDots)
            {
                pocket.Instance.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }

        public void CustomRules()
        {
            throw new NotImplementedException();
        }

        public void CheckForWin()
        {
            LineBehavior lb = UnityEngine.GameObject.Find("Controller").GetComponent<LineBehavior>();
            LineBehavior.Player winner;
            if (PocketDots.Contains(lb.currentLine.EndDot))
            {
                if (lb.currentLine.RendererInstance.startColor == PlayerPrefs.GetColor(LineBehavior.Player.P1))
                {
                    winner = LineBehavior.Player.P1;
                }
                else if (lb.currentLine.RendererInstance.startColor == PlayerPrefs.GetColor(LineBehavior.Player.P2))
                {
                    winner = LineBehavior.Player.P2;
                }
                else
                {
                    Debug.Log("Error: No Player Color Found");
                    return;
                }
            }
            else
            {
                return;
            }
            gameController.game.OnVictory(winner);
        }
        
    }

    public class SoccerBlitz : IGameType
    {
        public SoccerBlitz()
        {
            Background = Resources.Load<GameObject>("Prefabs/SoccerBlitzBackground");
            BoardHeight = 15;
            BoardWidth = 7;
        }
        public GameObject Background { get; set; }
        public Dot StartOfGameDot { get; set; }
        public int BoardHeight { get; set; }
        public int BoardWidth { get; set; }
        public GameObject CurrentDotCover { get; set; }

        public void CustomBoardSetup(int boxesX, int boxesY)
        {
            throw new NotImplementedException();
        }

        public void CustomRules()
        {
            throw new NotImplementedException();
        }

        public void CheckForWin()
        {
            throw new NotImplementedException();
        }
    }
}