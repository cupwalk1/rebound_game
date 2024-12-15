using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.Serialization;


public class GameController : MonoBehaviour
{
   public static GameController Instance;
   public GameObject CurrentGame;
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

   public GameObject Soccer;
   public GameObject Sumo;
   public GameObject Fencing;
   public GameObject Hockey;
   public GameObject Tutorial;
   private Game game {
      get
      {
         return CurrentGame.GetComponent<Game>();
      }
      set{}}
   
   [SerializeField] private GameObject sceneChangeGameObject;
   
   public UnityEvent OnGameStart;
   public UnityEvent OnBoardGenerated;

   private void Awake()
   {
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
      CurrentGame = null;
      await sceneChangeGameObject.GetComponent<SceneTransition>().StartSceneChange();
      
      switch (gameMode)
      {
         case Game.GameType.Soccer:
            CurrentGame = Instantiate(Soccer);
            break;
         case Game.GameType.Sumo:
            CurrentGame = Instantiate(Sumo);
            break;
         case Game.GameType.Fencing:
            CurrentGame = Instantiate(Fencing);
            break;
         case Game.GameType.Hockey: 
            CurrentGame = Instantiate(Hockey);
            break;   
         case Game.GameType.Tutorial:  
            CurrentGame = Instantiate(Tutorial);
            break;
      }
      OnGameStart.AddListener(game.SetupBoard);
      
      homeButton = GameObject.Find("homeButton");
      homeButton.GetComponent<Button>().interactable = false;
      homeButton.GetComponent<Button>().onClick.AddListener(OnhomeButtonPressed);
      OnGameStart.Invoke();
      homeButton.GetComponent<Button>().interactable = true;
   }


   
   public  void OnhomeButtonPressed()
   {
      homeButton.GetComponent<Button>().interactable = false;
      sceneChangeGameObject.GetComponent<SceneTransition>().StartSceneChange();
   }
}