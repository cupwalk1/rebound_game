using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDotIndicator : MonoBehaviour
{
   [SerializeField] private float worldSize;
   [SerializeField]
   private IPlayer thisPlayer
   {
      get
      {
         switch (player)
         {
            case playertype.Player1:
               return Player.Player1;
            case playertype.Player2:
               return Player.Player2;
            default:
               return Player.None;
         }
      }
      set { }
   }

   bool lastIsActiveState;
   bool currentIsActiveState;
   
   [SerializeField] private playertype player;
   Game _g;
   bool isTwoPlayerGame;

   private enum playertype
   {
      Player1,
      Player2
   }

   private async void Awake()
   {
      gameObject.SetActive(false);
      
      
      thisPlayer = player == playertype.Player1 ? Player.Player1 : Player.Player2;
      
      gameObject.GetComponent<Image>().color = thisPlayer.Color;
      isTwoPlayerGame = await CheckIfTwoPlayerGame();
      
      
      worldSize = thisPlayer.LastDot.Instance.GetComponent<CircleCollider2D>().radius/4;  
      gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2((Camera.main.WorldToViewportPoint(new Vector3(worldSize, 0, 0)).x-.5f)*Camera.main.aspect*600, (Camera.main.WorldToViewportPoint(new Vector3(worldSize, worldSize, 0)).y-.5f)*600);
      
      
      currentIsActiveState = Player.CurrentPlayer == thisPlayer;
      lastIsActiveState = currentIsActiveState;
      if(!currentIsActiveState)
      {
         if (!isTwoPlayerGame)
         {
            gameObject.GetComponent<Image>().enabled = false;
            gameObject.SetActive(true);
            return;
         }
         gameObject.GetComponent<Image>().color = Color.gray;
         transform.localScale = new Vector3(0.75f, 0.75f, 0.5f);
      }
      gameObject.SetActive(true);
   }

   private async Task<bool> CheckIfTwoPlayerGame()
   {
      while (GameController.Instance?.CurrentGame == null) await Task.Delay(10);
      _g = GameController.Instance.CurrentGame;
      bool t = _g.GetType().IsSubclassOf(typeof(TwoLineGame));
      return t;
   }

   private void ToggleActive()
   {
      if(!isTwoPlayerGame)
      {
         if (!currentIsActiveState)
         {
            gameObject.GetComponent<Image>().enabled = false;
            
         }
         else
         {
            gameObject.GetComponent<Image>().enabled = true;
         }
         return;
      }

      if (!currentIsActiveState)
      {
         gameObject.GetComponent<Image>().color = Color.gray;
         transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
      }
      else
      {
         gameObject.GetComponent<Image>().color = thisPlayer.Color;
         transform.localScale = new Vector3(1, 1, 1);
      }
   }
   
   private void Update()
   {
      
      currentIsActiveState = Player.CurrentPlayer == thisPlayer;
      
      if (lastIsActiveState != currentIsActiveState)
      {
         ToggleActive();
         lastIsActiveState = currentIsActiveState;
      }

      if (_g.CurrentLine! == null)
      {
         if (!isTwoPlayerGame) transform.position = _g.CurrentDot.Instance.transform.position;
         else transform.position = thisPlayer.LastDot.Instance.transform.position;
      }
      else if (_g.CurrentLine.GetEndDot() == null && currentIsActiveState)
      {
            transform.position = _g.CurrentLine.GetRendererInstance().GetPosition(1);
      }
      else
      {
         if (currentIsActiveState) transform.position = _g.CurrentLine.GetEndDot().Instance.transform.position;
      }
   }
}