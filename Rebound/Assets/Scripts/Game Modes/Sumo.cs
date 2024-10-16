using System;
using System.Linq;
using UnityEngine;

public class Sumo : Game
{
   public Sumo()
   {
      BoardHeight = 7;
      BoardWidth = 7;
      Background = Resources.Load<GameObject>("Prefabs/SumoBackground");
      CurrentDotCover = Resources.Load<GameObject>("Prefabs/SumoDot");
      AddLineBehavior();
   }

   public override GameObject Background { get; protected set; }
   public override Dot StartOfGameDot { get; protected set; }
   public override int BoardHeight { get; protected set; }
   public override int BoardWidth { get; protected set; }
   public override GameObject CurrentDotCover { get; protected set; }


   public override void CustomBoardSetup(int boxesX, int boxesY)
   {
      StartOfGameDot = Dot.Board[BoardWidth / 2, BoardHeight / 2];
      Player.player1.lastDot = StartOfGameDot;
      Player.player2.lastDot = StartOfGameDot;
   }

   public override void CustomRules()
   {
      throw new NotImplementedException();
   }

   public override void CheckForWin()
   {
      for (int i = 0; i < BoardWidth; i++)
      {
         for (int j = 0; j < BoardHeight; j++)
         {
            if (Dot.Board[i, j].GetAttachedLines().Count == 0)
            {
               return;
            }
         }
      }
      OnVictory(Player.none);
   }

   public override void SwitchPlayer()
   {
      base.SwitchPlayer();
      currentDot = Player.CurrentPlayer.lastDot;
   }

   public override void OnBeginLine()
   {
      base.OnBeginLine();
      IPlayer otherPlayer = Player.CurrentPlayer == Player.player1 ? Player.player2 : Player.player1;
      if (Line.LineHistory[^2]?.LinePlayer != Player.CurrentPlayer) startOfTurnDot = otherPlayer.lastDot;
      
   }
}