using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public abstract class Game
{
   public Dot startOfTurnDot;
   public List<Dot> availibleDots;
   public Dot currentDot;
   public Line currentLine;
   private List<Dot> OuterDots;
   public bool inProgress = true;
   public GameController gameController;
   public abstract int BoardHeight { get; protected set; }
   public abstract int BoardWidth { get; protected set; }

   public abstract Dot StartOfGameDot { get; protected set; }
   GameObject DotCover { get; set; }
   public abstract GameObject Background { get; protected set; }
   public abstract GameObject CurrentDotCover { get; protected set; }

   public abstract void CustomBoardSetup(int BoxesX, int BoxesY);
   protected abstract void CheckForWin();

   public LineBehavior currentLinePath;
   private GameObject WinnerText;

   public enum GameType
   {
      Soccer,
      Sumo,
      Hockey,
      Fencing,
      SoccerBlitz
   }

   public void SetupBoard()
   {
      AddLineBehavior();

      WinnerText = GameObject.Find("UI/WinnerText");
      BoardManager.Instance.GenerateBoard();
      foreach (GameObject i in GameObject.FindGameObjectsWithTag("PlayerIndicator"))
      {
         i.GetComponent<Image>().color = Player.CurrentPlayer.color;
      }

      OuterDots = BoardManager.Instance.GetOuterDots();
      DotCover = Object.Instantiate(CurrentDotCover, new Vector3(0, 0, 0), Quaternion.identity);
      DotCover.transform.SetParent(GameObject.Find("UI").transform);
      DotCover.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

      startOfTurnDot = StartOfGameDot;


      SetCurrentDot(StartOfGameDot);
   }

   public void DestroyLine(Line line)
   {
      Line.LineHistory.Remove(line);
      line.GetStartDot().AttachedLines.Remove(line);
      line.GetEndDot()?.AttachedLines.Remove(line);
      Object.Destroy(line.Instance);
      currentLine = null;
   }

   public void SetCurrentDot(Dot dot)
   {
      currentDot = dot;
      availibleDots = currentDot.AvailibleDots();
      UpdateDotCoverPosition(dot.Instance.transform.position);
   }

   protected void OnVictory(IPlayer winner)
   {
      GameObject[] particles = GameObject.FindGameObjectsWithTag("WinnerCoriandoli");
      inProgress = false;
      WinnerText.SetActive(true);
      GameObject.FindGameObjectWithTag("Button").GetComponent<Button>().interactable = false;
      GameObject.Find("UI/backgroundWin").GetComponent<Image>().enabled = true;
      WinnerText.GetComponent<TMP_Text>().text = winner.name + " Wins!";
      WinnerText.GetComponent<TMP_Text>().color = winner.color;

      foreach (GameObject particle in particles)
      {
         particle.GetComponent<ParticleSystem>().Play();
      }
   }

   private void UpdateDotCoverPosition(Vector3 pos)
   {
      DotCover.transform.position = pos;
   }

   public virtual void OnBeginLine()
   {
      SetCurrentDot(currentDot);
      if (currentDot.AttachedLines.Count == 1) startOfTurnDot = currentDot;
      currentLine = new Line(Player.CurrentPlayer, currentDot, isGameLine: true);
   }

   public virtual void OnDragLine(Touch touch)
   {
      Vector3 touchPos = Camera.main!.ScreenToWorldPoint(touch.position);
      touchPos.z = 0;
      currentLine.SetEndDot(null, true);
      UpdateDotCoverPosition(touchPos);
      if (currentLine.RendererInstance != null) currentLine.RendererInstance.SetPosition(1, touchPos);
   }

   public virtual void OnBounce(Dot touchedDot)
   {
      currentLine.SetEndDot(touchedDot);
      CheckForWin();
      if (currentDot.LoseDots.Contains(touchedDot))
      {
         GameController gc = GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>();
         switch (Player.CurrentPlayer)
         {
            case P1:
               OnVictory(Player.player2);
               break;
            case P2:
               OnVictory(Player.player1);
               break;
         }
      }

      SetCurrentDot(touchedDot);
      if (OuterDots.Contains(currentDot))
      {
         currentDot.SetColor(Player.CurrentPlayer.color);
         currentDot.Instance.GetComponent<SpriteRenderer>().sortingOrder = 10;
         currentLine.Instance.GetComponent<LineRenderer>().sortingOrder = 9;
      }

      currentLine = new Line(Player.CurrentPlayer, currentDot, isGameLine: true);
   }

   public virtual void OnEndLine(Dot touchedDot)
   {
      UpdateDotCoverPosition(touchedDot.Instance.transform.position);
      currentLine.SetEndDot(touchedDot, true);
   }

   public virtual void OnLineCanceled()
   {
      UpdateDotCoverPosition(currentLine.GetStartDot().Instance.transform.position);
      DestroyLine(currentLine);
   }

   public virtual void OnLineReleased()
   {
      currentLine.SetEndDot(currentLine.GetEndDot());
      SetCurrentDot(currentLine.GetEndDot());
      currentLine = null;
      SwitchPlayer();
      UpdateDotCoverPosition(currentDot.Instance.transform.position);
   }

   public virtual void SwitchPlayer()
   {
      Player.CurrentPlayer.lastDot = currentDot;
      Player.CurrentPlayer = Player.CurrentPlayer == Player.player1 ? Player.player2 : Player.player1;
      foreach (GameObject i in GameObject.FindGameObjectsWithTag("PlayerIndicator"))
      {
         i.GetComponent<Image>().color = Player.CurrentPlayer.color;
      }
   }

   public void AddLineBehavior()
   {
      if (currentLinePath == null)
      {
         GameObject go = new("LineBehavior");
         go.AddComponent<LineBehavior>();
         currentLinePath = go.GetComponent<LineBehavior>();
      }
   }

   public virtual void ExtraUndoBehavior(){}
}