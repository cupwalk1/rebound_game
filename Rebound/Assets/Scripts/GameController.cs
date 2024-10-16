using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;



public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public Game CurrentGame;
    public BoardManager boardManager;
    public Game.GameType GameMode;
    public GameObject WinnerText;
    public GameObject SoccerBackground;
    public GameObject SumoBackground;
    public GameObject GolfBackground;
    public GameObject PoolBackground;
    public GameObject SoccerBlitzBackground;
    public GameObject undoButton;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        undoButton = GameObject.Find("UI/UndoButton");
    }

public void StartGame(Game.GameType GameMode)
{
    if (SceneManager.GetActiveScene().name == "GameScene")
    {
        Debug.Log("Game Already Started");
        return;
    }
    StartCoroutine(LoadGameScene(GameMode));
}

private IEnumerator<object> LoadGameScene(Game.GameType GameMode)
{
    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");

    while (!asyncLoad.isDone)
    {
        yield return null;
    }

    switch (GameMode)
    {
        case Game.GameType.Soccer:
            CurrentGame = new Soccer();
            break;
        case Game.GameType.Sumo:
            CurrentGame = new Sumo();
            break;
        case Game.GameType.Golf:
            CurrentGame = new Golf();
            break;
        case Game.GameType.Pool:
            CurrentGame = new Pool();
            break;
        case Game.GameType.SoccerBlitz:
            CurrentGame = new SoccerBlitz();
            break;
    }
    CurrentGame.SetupBoard();
}

}
