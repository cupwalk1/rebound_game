using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    public void OnSoccerClick()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.StartGame(Game.GameType.Soccer);
         
    }

    public void OnSumoClick()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.StartGame(Game.GameType.Sumo);
    }

    public void OnGolfClick()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.StartGame(Game.GameType.Golf);
    }

    public void OnPoolClick()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.StartGame(Game.GameType.Pool);
    }

    public void OnSoccerBlitzClick()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.StartGame(Game.GameType.SoccerBlitz);
    }

}