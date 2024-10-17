using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fencing : Game
{
   public Fencing()
   {
      BoardHeight = 9;
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
      StartOfGameDot = Player.player1.lastDot = Dot.Board[3, 3];
      Player.player2.lastDot = Dot.Board[3, 5];
   }

   public override void CustomRules()
   {
      throw new NotImplementedException();
   }

   public override void CheckForWin()
   {
      Game g  = GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>().CurrentGame;
      IPlayer otherPlayer = Player.CurrentPlayer == Player.player1 ? Player.player2 : Player.player1;
      if (Line.LineHistory[^1].GetEndDot() == otherPlayer.lastDot)
      {
         OnVictory(Player.CurrentPlayer);
         return;
      }
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
      if (currentLine.GetStartDot() == StartOfGameDot) return;
      if (Line.LineHistory[^2]?.LinePlayer != Player.CurrentPlayer) startOfTurnDot = otherPlayer.lastDot;
      
   }
}