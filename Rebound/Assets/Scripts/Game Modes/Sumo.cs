using System.Linq;
using UnityEngine;


public class Sumo : TwoLineGame
{
   public Sumo()
   {
      BoardHeight = 7;
      BoardWidth = 7;
      Background = Resources.Load<GameObject>("Prefabs/SumoBackground");
      CurrentDotCover = Resources.Load<GameObject>("Prefabs/SumoDot1");
      SecondDotCover = Resources.Load<GameObject>("Prefabs/SumoDot2");
      AddLineBehavior();
   }

   public override GameObject Background { get; protected set; }
   public override Dot StartOfGameDot { get; protected set; }
   public override int BoardHeight { get; protected set; }
   public override int BoardWidth { get; protected set; }
   public override GameObject CurrentDotCover { get; protected set; }
   public override GameObject SecondDotCover { get; protected set; }


   public override void CustomBoardSetup(int boxesX, int boxesY)
   {
      StartOfGameDot = Dot.Board[BoardWidth / 2, BoardHeight / 2];
      Player.player1.lastDot = StartOfGameDot;
      Player.player2.lastDot = StartOfGameDot;
   }


   protected sealed override void ExtraWinConditions()
   {
      if (Line.LineHistory[^1].GetEndDot() == Player.CurrentPlayer.lastDot)
      {
         OnVictory(Player.CurrentPlayer);
         return;
      }
   }
   
}