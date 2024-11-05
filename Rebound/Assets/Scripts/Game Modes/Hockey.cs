using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;



public sealed class Hockey : TwoLineGame
{
   bool _hasExtraMove;
   public override GameObject Background { get; protected set; }
   public override Dot StartOfGameDot { get; protected set; }
   public override int BoardHeight { get; protected set; }
   public override int BoardWidth { get; protected set; }
   List<Dot> _p1GoalDots = new();
   List<Dot> _p2GoalDots = new();
   List<Dot> _p1BackOfGoalDots = new();
   List<Dot> _p2BackOfGoalDots = new();
   private List<List<Dot>> invisibleDotChains = new();
   Puck _puck;


   public Hockey()
   {
      BoardHeight = 13;
      BoardWidth = 7;
      Background = Resources.Load<GameObject>("Prefabs/SoccerBackground");
   }
   
   public override void CustomBoardSetup(int boxesX, int boxesY)
   {
      int goalBoxes = Mathf.CeilToInt((boxesX / 4f) / 2) * 2;

      StartOfGameDot = Player.Player1.LastDot = Dot.Board[(boxesX / 2), boxesY / 2 - 1];
      Player.Player2.LastDot = Dot.Board[(boxesX / 2), boxesY / 2 + 1];
      _puck = new Puck(StartOfGameDot);
      

      for (int i = 0; i <= goalBoxes; i++)
      {
         _p1GoalDots.Add(Dot.Board[(boxesX - goalBoxes) / 2 + i, boxesY - 2]);
         _p2GoalDots.Add(Dot.Board[(boxesX - goalBoxes) / 2 + i, 2]);
      }
      _p1BackOfGoalDots.Add(_p1GoalDots[0]);
      _p2BackOfGoalDots.Add(_p2GoalDots[0]);
      for (int i = 0; i <= goalBoxes; i++)
      {
         _p1BackOfGoalDots.Add(Dot.Board[(boxesX - goalBoxes) / 2 + i, boxesY - 1]);
         _p2BackOfGoalDots.Add(Dot.Board[(boxesX - goalBoxes) / 2 + i, 1]);
      }
      _p1BackOfGoalDots.Add(_p1GoalDots[^1]);
      _p2BackOfGoalDots.Add(_p2GoalDots[^1]);


      new DotChain(_p1BackOfGoalDots, Player.None, false).CreateChain();
      new DotChain(_p2BackOfGoalDots, Player.None, false).CreateChain();
      new DotChain(_p2GoalDots, Player.Player2, false).CreateChain();
      new DotChain(_p1GoalDots, Player.Player1, false).CreateChain();
      
      
      invisibleDotChains = new List<List<Dot>>
      {
         new List<Dot>{_p1GoalDots[0], _p1BackOfGoalDots[2], _p1GoalDots[^1]},
         new List<Dot>{_p1BackOfGoalDots[1], _p1GoalDots[1], _p1BackOfGoalDots[^2]},
         new List<Dot>{_p2GoalDots[0], _p2BackOfGoalDots[2], _p2GoalDots[^1]},
         new List<Dot>{_p2BackOfGoalDots[1], _p2GoalDots[1], _p2BackOfGoalDots[^2]}
      };
      foreach (List<Dot> chain in invisibleDotChains)
      {
         new DotChain(chain, Player.None).CreateChain(true);
      }


   }

   public override void OnBeginLine()
   {  
      base.OnBeginLine();
      if (CurrentLine.GetStartDot() == StartOfTurnDot && _puck.PuckHolder != Player.CurrentPlayer)
      {
         _hasExtraMove = true;
      }
      _puck.LastStealDot = null;
      _puck.LastDoubleBounceDot = null;
   }

   protected override void ExtraWinConditions()
   {
      if (_p1GoalDots.Contains(Player.CurrentPlayer.LastDot) && Player.CurrentPlayer == Player.Player1 &&  _puck.PuckHolder == Player.Player1)
      {
         OnVictory(CurrentLine.LinePlayer);
      }

      if (_p2GoalDots.Contains(Player.CurrentPlayer.LastDot) && Player.CurrentPlayer == Player.Player2 &&  _puck.PuckHolder == Player.Player2)
      {
         OnVictory(Player.Player2);
      }
   }

   public override void OnDragLine(Touch touch)
   {
      base.OnDragLine(touch);
      if (Player.CurrentPlayer == _puck.PuckHolder) _puck.SetPosition(Camera.main!.ScreenToWorldPoint(touch.position));
   }

   public override void OnBounce(Dot touchedDot)
   {
      if (Player.CurrentPlayer == _puck.PuckHolder)
      {
         _puck.SetPosition(touchedDot);
      }
      if(touchedDot == _puck.PuckDot && _puck.PuckHolder != Player.CurrentPlayer)
      {
         _puck.LastStealDot = touchedDot;
         _puck.PuckHolder = Player.CurrentPlayer;
      }

      
      base.OnBounce(touchedDot);
   }

   public override void OnEndLine(Dot touchedDot)
   {
      if (_hasExtraMove && Player.CurrentPlayer != _puck.PuckHolder)
      {
         //can double bounce
         _hasExtraMove = false;
         _puck.LastDoubleBounceDot = touchedDot;
         base.OnBounce(touchedDot);
         return;
      }
      base.OnEndLine(touchedDot);
      //move puck with currentPlayer
      if (_puck.PuckHolder == Player.CurrentPlayer)
      {
         _puck.SetPosition(touchedDot);
      }
   }
   
   public override void OnLineCanceled()
   {
      base.OnLineCanceled();
      
      if (_puck.PuckHolder == Player.CurrentPlayer)
      {
         _puck.SetPosition(CurrentDot);
      }
   }

   public override void ExtraUndoBehavior()
   {
      if (_puck.LastStealDot == CurrentDot)
      {
         _puck.LastStealDot = null;
         _puck.PuckHolder = Player.Opponent;
         return;
      }
      if (_puck.PuckHolder == Player.CurrentPlayer)
      {
         _puck.SetPosition(Line.LineHistory.Last().GetStartDot());
         return;
      }
      if (_puck.LastDoubleBounceDot == CurrentDot)
      {
         _puck.LastDoubleBounceDot = null;
         _hasExtraMove = true;
         return;
      }
   }
}


public class Puck
{
   public Dot LastDoubleBounceDot;
   public Dot LastStealDot;
   private GameObject PuckPrefab;
   public Dot PuckDot;
   private IPlayer _puckHolder;

   public IPlayer PuckHolder
   {
      get => _puckHolder;
      set
      {
         SetPosition(value.LastDot);
         _puckHolder = value;
      }
   }

   public GameObject Instance;
   public Puck(Dot dot)
   {
      LastDoubleBounceDot = null;
      LastStealDot = null;
      _puckHolder = Player.CurrentPlayer;
      PuckPrefab = Resources.Load<GameObject>("Prefabs/HockeyPuck");
      Instance = Object.Instantiate(PuckPrefab, new Vector3(0, 0, 0), Quaternion.identity);
      Instance.transform.SetParent(GameObject.Find("UI").transform);
      Instance.transform.localScale = Vector3.one;
      SetPosition(dot);
   }


   public void SetPosition(Vector3 pos)
   {
      Vector3 fixedZVector = new Vector3(pos.x, pos.y, 0);
      Instance.transform.position = fixedZVector;
   }
   public void SetPosition(Dot dot)
   {
      Instance.transform.position = dot.Instance.transform.position;
      PuckDot = dot;
   }
}