using System.Collections.Generic;
using UnityEngine;

public class Soccer : Game
{
   public override GameObject Background { get; protected set; }
   public override Dot StartOfGameDot { get; protected set; }
   public sealed override int BoardHeight { get; protected set; }
   public override int BoardWidth { get; protected set; }
   readonly List<Dot> _p1GoalDots = new();
   readonly List<Dot> _p2GoalDots = new();
   public Soccer()
   {

      BoardHeight = 13;
      BoardWidth = 7;
      Background = Resources.Load<GameObject>("Prefabs/SoccerBackground");
   }

   public override void CustomBoardSetup(int boxesX, int boxesY)
   {
      int goalBoxes = Mathf.CeilToInt((boxesX / 3f) / 2) * 2;
      for (int i = 0; i < (boxesX - goalBoxes) / 2; i++)
      {
         UnityEngine.Object.Destroy(Dot.Board[i, 0].Instance);
         UnityEngine.Object.Destroy(Dot.Board[i, boxesY].Instance);
         UnityEngine.Object.Destroy(Dot.Board[boxesX - i, 0].Instance);
         UnityEngine.Object.Destroy(Dot.Board[boxesX - i, boxesY].Instance);
         Dot.Board[i, 0] = null;
         Dot.Board[i, boxesY] = null;
         Dot.Board[boxesX - i, 0] = null;
         Dot.Board[boxesX - i, boxesY] = null;
      }
      StartOfGameDot = Dot.Board[(boxesX / 2), boxesY / 2];
      Player.Player1.LastDot = StartOfGameDot;
      Player.Player2.LastDot = StartOfGameDot;

      for (int i = 0; i <= goalBoxes; i++)
      {
         _p1GoalDots.Add(Dot.Board[(boxesX - goalBoxes) / 2 + i, boxesY - 1]);
         _p2GoalDots.Add(Dot.Board[(boxesX - goalBoxes) / 2 + i, 1]);
      }
      for (int i = 0; i < _p1GoalDots.Count; i++)
      {
         _p1GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Player.Player1.Color;
         if (i == 0)
         {
            _p1GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Color.white;
         }
         if (i == _p1GoalDots.Count - 1)
         {
            _p1GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Color.white;
            continue;
         }
         Line l = new(Player.None, _p1GoalDots[i], _p1GoalDots[i + 1]);
         l.SetColor(Player.Player1.Color);

      }
      for (int i = 0; i < _p2GoalDots.Count; i++)
      {
         _p2GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Player.Player2.Color;
         if (i == 0)
         {
            _p2GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Color.white;
         }
         if (i == _p2GoalDots.Count - 1)
         {
            _p2GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Color.white;
            continue;
         }
         Line l = new(Player.None, _p2GoalDots[i], _p2GoalDots[i + 1]);
         l.SetColor(Player.Player2.Color);
         l.Instance.GetComponent<LineRenderer>().sortingOrder = 8;
      }
      //

   }

   protected override void CheckForWin()
   {
      IPlayer winner;
      if (_p1GoalDots.Contains(CurrentLine.EndDot) && Player.Player1 == CurrentLine.LinePlayer)
      {
         OnVictory(Player.Player1);
      }
      if (_p2GoalDots.Contains(CurrentLine.EndDot) && Player.Player2 == CurrentLine.LinePlayer)
      {
         OnVictory(Player.Player2);
      }
   }
}