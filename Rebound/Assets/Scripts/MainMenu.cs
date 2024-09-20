using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public static class PlayerPrefs
{
    public static Color player1Color = Color.red;
    public static Color player2Color = Color.blue;
    public static Color GetColor(LineBehavior.Player key)
    {
        if (key == LineBehavior.Player.P1)
        {
            return player1Color;
        }
        else if (key == LineBehavior.Player.P2)
        {
            return player2Color;
        }
        return Color.white;
    }
    public static void SetColor(LineBehavior.Player key, Color value)
    {
        if (key == LineBehavior.Player.P1)
        {
            player1Color = value;
        }
        else if (key == LineBehavior.Player.P2)
        {
            player2Color = value;
        }
    }
}
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