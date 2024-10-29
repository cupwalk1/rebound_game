using System.Collections.Generic;
using UnityEngine;



public sealed class Hockey : TwoLineGame
{
   bool hasExtraMove;
   public override GameObject Background { get; protected set; }
   public override Dot StartOfGameDot { get; protected set; }
   public override int BoardHeight { get; protected set; }
   public override int BoardWidth { get; protected set; }
   public override GameObject CurrentDotCover { get; protected set; }
   public override GameObject SecondDotCover { get; protected set; }
   readonly List<Dot> P1GoalDots = new();
   readonly List<Dot> P2GoalDots = new();
   Puck puck;


   public Hockey()
   {
      BoardHeight = 13;
      BoardWidth = 7;
      Background = Resources.Load<GameObject>("Prefabs/SoccerBackground");
      CurrentDotCover = Resources.Load<GameObject>("Prefabs/HockeyDot1");
      SecondDotCover = Resources.Load<GameObject>("Prefabs/HockeyDot2");
   }
   
   public override void CustomBoardSetup(int boxesX, int boxesY)
   {
      int goalBoxes = Mathf.CeilToInt((boxesX / 4f) / 2) * 2;
      for (int i = 0; i < (boxesX - goalBoxes) / 2; i++)
      {
         Object.Destroy(Dot.Board[i, 0].Instance);
         Object.Destroy(Dot.Board[i, boxesY].Instance);
         Object.Destroy(Dot.Board[boxesX - i, 0].Instance);
         Object.Destroy(Dot.Board[boxesX - i, boxesY].Instance);
         Dot.Board[i, 0] = null;
         Dot.Board[i, boxesY] = null;
         Dot.Board[boxesX - i, 0] = null;
         Dot.Board[boxesX - i, boxesY] = null;
      }
      StartOfGameDot = Player.player1.lastDot = Dot.Board[(boxesX / 2), boxesY / 2 - 1];
      Player.player2.lastDot = Dot.Board[(boxesX / 2), boxesY / 2 + 1];
      puck = new Puck(StartOfGameDot);
      

      for (int i = 0; i <= goalBoxes; i++)
      {
         P1GoalDots.Add(Dot.Board[(boxesX - goalBoxes) / 2 + i, boxesY - 1]);
         P2GoalDots.Add(Dot.Board[(boxesX - goalBoxes) / 2 + i, 1]);
      }

      for (int i = 0; i < P1GoalDots.Count; i++)
      {
         P1GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Player.player1.color;
         if (i == 0)
         {
            P1GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Color.white;
         }

         if (i == P1GoalDots.Count - 1)
         {
            P1GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Color.white;
            continue;
         }

         Line l = new(Player.none, P1GoalDots[i], P1GoalDots[i + 1]);
         l.SetColor(Player.player1.color);
      }

      for (int i = 0; i < P2GoalDots.Count; i++)
      {
         P2GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Player.player2.color;
         if (i == 0)
         {
            P2GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Color.white;
         }

         if (i == P2GoalDots.Count - 1)
         {
            P2GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Color.white;
            continue;
         }

         Line l = new(Player.none, P2GoalDots[i], P2GoalDots[i + 1]);
         l.SetColor(Player.player2.color);
         l.Instance.GetComponent<LineRenderer>().sortingOrder = 8;
      }
      //
   }

   public override void OnBeginLine()
   {
      base.OnBeginLine();
      if (currentLine.GetStartDot() == Player.CurrentPlayer.lastDot && puck.PuckHolder != Player.CurrentPlayer)
      {
         hasExtraMove = true;
      }
      puck._lastStealDot = null;
      puck._lastDoubleBounceDot = null;
   }

   protected override void ExtraWinConditions()
   {
      IPlayer winner;
      if (P1GoalDots.Contains(puck._puckDot))
      {
         OnVictory(currentLine.LinePlayer);
      }
   }

   public override void OnDragLine(Touch touch)
   {
      base.OnDragLine(touch);
      if (Player.CurrentPlayer == puck.PuckHolder) puck.SetPosition(Camera.main!.ScreenToWorldPoint(touch.position));
   }

   public override void OnBounce(Dot touchedDot)
   {
      base.OnBounce(touchedDot);
      if (puck.PuckHolder == Player.CurrentPlayer)
      {
         
      }
      else if(touchedDot == puck._puckDot)
      {
         puck._lastStealDot = touchedDot;
         puck.PuckHolder = Player.CurrentPlayer;
      }
      
   }

   public override void OnEndLine(Dot touchedDot)
   {
      if (hasExtraMove && Player.CurrentPlayer != puck.PuckHolder)
      {
         //can double bounce
         hasExtraMove = false;
         puck._lastDoubleBounceDot = touchedDot;
         base.OnBounce(touchedDot);
         return;
      }
      base.OnEndLine(touchedDot);
      //move puck with currentPlayer
      if (puck.PuckHolder == Player.CurrentPlayer)
      {
         puck.SetPosition(touchedDot);
      }
   }

   public override void ExtraUndoBehavior()
   {
      if (puck._lastStealDot == currentDot)
      {
         puck._lastStealDot = null;
         puck.PuckHolder = Player.CurrentPlayer == Player.player1 ? Player.player2 : Player.player1;
         return;
      }
      if (puck._lastDoubleBounceDot == currentDot)
      {
         puck._lastDoubleBounceDot = null;
         hasExtraMove = true;
         return;
      }
      puck.PuckHolder = Player.CurrentPlayer;
      
   }
}


public class Puck
{
   public Dot _lastDoubleBounceDot;
   public Dot _lastStealDot;
   public GameObject _puckPrefab;
   public Dot _puckDot;
   private IPlayer _puckHolder;

   public IPlayer PuckHolder
   {
      get
      {
         return _puckHolder;
      }
      set
      {
         SetPosition(value.lastDot);
         _puckHolder = value;
      }
   }

   public GameObject _instance;
   public Puck(Dot dot)
   {
      _lastDoubleBounceDot = null;
      _lastStealDot = null;
      _puckHolder = Player.CurrentPlayer;
      _puckPrefab = Resources.Load<GameObject>("Prefabs/HockeyPuck");
      _instance = Object.Instantiate(_puckPrefab, new Vector3(0, 0, 0), Quaternion.identity);
      _instance.transform.SetParent(GameObject.Find("UI").transform);
      _instance.transform.localScale = Vector3.one;
      SetPosition(dot);
   }

   private void UpdatePuck()
   {
      SetPosition(GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>().CurrentGame.currentDot);
   }
   public void SetPosition(Vector3 pos)
   {
      Vector3 fixedZVector = new Vector3(pos.x, pos.y, 0);
      _instance.transform.position = pos;
   }
   public void SetPosition(Dot dot)
   {
      _instance.transform.position = dot.Instance.transform.position;
      _puckDot = dot;
   }
}