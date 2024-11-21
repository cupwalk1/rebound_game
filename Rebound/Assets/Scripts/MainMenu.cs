using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private float x;
    public void Start()
    {
        SettingsPanel.SetActive(false);
        x = Camera.main.ScreenToWorldPoint(new Vector3(Screen.height, 0, 0)).x;
        SettingsMenu.transform.SetPositionAndRotation(new Vector3(0, -x-2, 0), new Quaternion(0, 0, 0, 0));
    }
    
    public void OnTutorialClick()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.StartGame(Game.GameType.Tutorial);
         
    }

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

    public void OnHockeyClick()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.StartGame(Game.GameType.Hockey);
    }

    public void OnFencingClick()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.StartGame(Game.GameType.Fencing);
    }

    public void OnSoccerBlitzClick()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.StartGame(Game.GameType.SoccerBlitz);
    }
    
    public void OnSettingsOpen()
    {
        SettingsPanel.SetActive(true);
        SettingsMenu.transform.LeanMoveY(0, 1f).setEaseOutElastic().period = 1.2f;
    }
    
    public void OnSettingsClose()
    {
        SettingsMenu.transform.LeanMoveY(-x-2, 1f).setEaseInQuad().setOnComplete(() => SettingsPanel.SetActive(false));
    }

}