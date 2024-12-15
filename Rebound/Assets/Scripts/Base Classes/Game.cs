using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CandyCoded.HapticFeedback;


public abstract class Game : MonoBehaviour
{
   public static Game Instance;
   public Dot StartOfTurnDot;
   public List<Dot> AvailableDots;
   private Dot _currentDot;
   public virtual Dot CurrentDot
   {
      get => _currentDot;
      set
      {
         _currentDot = value;
      }
   }
   public Line CurrentLine;
   private List<Dot> _outerDots;
   public bool InProgress = true;
   public GameController GameController;
   public abstract int BoardHeight { get; protected set; }
   public abstract int BoardWidth { get; protected set; }

   public abstract Dot StartOfGameDot { get; protected set; }
   public abstract GameObject Background { get; protected set; }

   public abstract void CustomBoardSetup(int boxesX, int boxesY);
   protected abstract void CheckForWin();

   public LineBehavior CurrentLinePath;
   private GameObject _winnerText;

   public enum GameType
   {
      Soccer,
      Sumo,
      Hockey,
      Fencing,
      SoccerBlitz,
      Tutorial
   }
   

   public Game()
   {
      Instance = this;
   }
   
   public void SetupBoard()
   {
      Debug.Log("Setting up board");
      Player.ResetPlayers();
      BoardManager.Instance.GenerateBoard();
      foreach (GameObject i in GameObject.FindGameObjectsWithTag("PlayerIndicator"))
      {
         i.GetComponent<Image>().color = Player.CurrentPlayer.Color;
      }

      _outerDots = BoardManager.Instance.GetOuterDots();

      StartOfTurnDot = StartOfGameDot;


      SetCurrentDot(StartOfGameDot);
   }

   public void DestroyLine(Line line)
   {
      Line.LineHistory.Remove(line);
      line.GetStartDot().AttachedLines.Remove(line);
      line.GetEndDot()?.AttachedLines.Remove(line);
      Object.Destroy(line.Instance);
      CurrentLine = null;
   }
   
   public void SetCurrentDot(Dot dot)
   {
      CurrentDot = dot;
      AvailableDots = CurrentDot.AvailibleDots();
   }
   
   protected void OnVictory(IPlayer winner)
   {
      InProgress = false;
      if (Vibration.CurrentValue)
      {
         Handheld.Vibrate();
      }
      SoundManager.Instance.PlaySFX(SoundManager.Instance.win);
      GameObject.FindGameObjectWithTag("Button").GetComponent<Button>().interactable = false;
      GameObject.Find("UI/VictoryUI").GetComponent<VictoryAnimation>().ShowVictoryUI(winner);
   }
   
   public virtual void OnBeginLine()
   {
      SetCurrentDot(CurrentDot);
      if (CurrentDot.AttachedLines.Count == 1) StartOfTurnDot = CurrentDot;
      CurrentLine = new Line(Player.CurrentPlayer, CurrentDot, isGameLine: true);
   }
   
   public virtual void OnDragLine(Touch touch)
   {
      var touchPos = Camera.main!.ScreenToWorldPoint(touch.position);
      touchPos.z = 0;
      CurrentLine.SetEndDot(null, true);
      if (CurrentLine.RendererInstance != null) CurrentLine.RendererInstance.SetPosition(1, touchPos);
   }
   
   public virtual void OnBounce(Dot touchedDot)
   {
      if (Vibration.CurrentValue)
      {
         HapticFeedback.MediumFeedback();
      }
      SoundManager.Instance.PlaySFX(SoundManager.Instance.pop);
      CurrentLine.SetEndDot(touchedDot);
      if (CurrentDot.LoseDots.Contains(touchedDot))
      {
         switch (Player.CurrentPlayer)
         {
            case P1:
               OnVictory(Player.Player2);
               break;
            case P2:
               OnVictory(Player.Player1);
               break;
         }
      }
      SetCurrentDot(touchedDot);
      CheckForWin();
      if (_outerDots.Contains(CurrentDot))
      {
         CurrentDot.SetColor(Player.CurrentPlayer.Color);
         CurrentDot.Instance.GetComponent<SpriteRenderer>().sortingOrder = 10;
         CurrentLine.Instance.GetComponent<LineRenderer>().sortingOrder = 9;
      }
      CurrentLine = new Line(Player.CurrentPlayer, CurrentDot, isGameLine: true);
   }

   public virtual void OnEndLine(Dot touchedDot)
   {
      if (CurrentLine.GetEndDot() != touchedDot)
      {
         SoundManager.Instance.PlaySFX(SoundManager.Instance.pop);
         if (Vibration.CurrentValue)
         {
            HapticFeedback.MediumFeedback();
         }
      }
      CurrentLine.SetEndDot(touchedDot, true);
   }

   public virtual void OnLineCanceled()
   {
      DestroyLine(CurrentLine);
   }

   public virtual void OnLineReleased()
   {
      CurrentLine.SetEndDot(CurrentLine.GetEndDot());
      SetCurrentDot(CurrentLine.GetEndDot());
      CurrentLine = null;
      SwitchPlayer();
   }

   public virtual void SwitchPlayer()
   {
      Player.CurrentPlayer.LastDot = CurrentDot;
      Player.CurrentPlayer = Player.CurrentPlayer == Player.Player1 ? Player.Player2 : Player.Player1;
      foreach (GameObject i in GameObject.FindGameObjectsWithTag("PlayerIndicator"))
      {
         i.GetComponent<Image>().color = Player.CurrentPlayer.Color;
      }
   }
   
   public virtual void ExtraUndoBehavior(){}
}