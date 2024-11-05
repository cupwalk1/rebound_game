using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine.Serialization;


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
   public GameObject homeButton;
   [SerializeField] private GameObject sceneChangeGameObject;

   private void Awake()
   {
      Screen.orientation = ScreenOrientation.Portrait;
      if (Instance != null)
      {
         Destroy(this);
         return;
      }
      Instance = this;
      DontDestroyOnLoad(gameObject);
   }
   

   public async void StartGame(Game.GameType gameMode)
   {
      
      await sceneChangeGameObject.GetComponent<SceneTransition>().StartSceneChange();
      switch (gameMode)
      {
         case Game.GameType.Soccer:
            CurrentGame = new Soccer();
            break;
         case Game.GameType.Sumo:
            CurrentGame = new Sumo();
            break;
         case Game.GameType.Hockey:
            CurrentGame = new Hockey();
            break;
         case Game.GameType.Fencing:
            CurrentGame = new Fencing();
            break;
         case Game.GameType.SoccerBlitz:
            CurrentGame = new SoccerBlitz();
            break;
      }

      CurrentGame.SetupBoard();
      homeButton = GameObject.Find("UI/homeButton");
      homeButton.GetComponent<Button>().onClick.AddListener(OnhomeButtonPressed);
   }


   
   public async void OnhomeButtonPressed()
   {
      homeButton.GetComponent<Button>().interactable = false;
      sceneChangeGameObject.GetComponent<SceneTransition>().StartSceneChange();
   }
}