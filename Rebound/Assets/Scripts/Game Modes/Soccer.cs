using System.Collections.Generic;
using UnityEngine;

public class Soccer : Game
{
   public override GameObject Background { get; protected set; }
   public override Dot StartOfGameDot { get; protected set; }
   public sealed override int BoardHeight { get; protected set; }
   public override int BoardWidth { get; protected set; }
   public override GameObject CurrentDotCover { get; protected set; }
   readonly List<Dot> P1GoalDots = new();
   readonly List<Dot> P2GoalDots = new();
   public Soccer()
   {

      BoardHeight = 13;
      BoardWidth = 7;
      CurrentDotCover = Resources.Load<GameObject>("Prefabs/SoccerBall");
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
      Player.player1.lastDot = StartOfGameDot;
      Player.player2.lastDot = StartOfGameDot;

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

   protected override void CheckForWin()
   {
      IPlayer winner;
      if (P1GoalDots.Contains(currentLine.EndDot))
      {
         OnVictory(currentLine.LinePlayer);
      }

   }
}