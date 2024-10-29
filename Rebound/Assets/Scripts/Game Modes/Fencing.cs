using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Fencing : TwoLineGame
{
   public Fencing()
   {
      BoardHeight = 9;
      BoardWidth = 7;
      Background = Resources.Load<GameObject>("Prefabs/SumoBackground");
      CurrentDotCover = Resources.Load<GameObject>("Prefabs/FencingDot1");
      SecondDotCover = Resources.Load<GameObject>("Prefabs/FencingDot2");
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
      StartOfGameDot = Player.player1.lastDot = Dot.Board[3, 3];
      Player.player2.lastDot = Dot.Board[3, 5];
   }


   protected sealed override void ExtraWinConditions()
   {
      Game g = GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>().CurrentGame;
      IPlayer otherPlayer = Player.CurrentPlayer == Player.player1 ? Player.player2 : Player.player1;
      if (Line.LineHistory[^1].GetEndDot() == otherPlayer.lastDot)
      {
         OnVictory(Player.CurrentPlayer);
         return;
      }
   }
}