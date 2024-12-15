using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public sealed class Fencing : TwoLineGame
{
   public Fencing()
   {
      BoardHeight = 9;
      BoardWidth = 7;
   }

   public GameObject _background;
   public override GameObject Background
   {
      get
      {
         return _background;
      }
      protected set
      {
         _background = value;
      }
   }
   public override Dot StartOfGameDot { get; protected set; }
   public override int BoardHeight { get; protected set; }
   public override int BoardWidth { get; protected set; }



   public override void CustomBoardSetup(int boxesX, int boxesY)
   {
      StartOfGameDot = Player.Player1.LastDot = Dot.Board[3, 3];
      Player.Player2.LastDot = Dot.Board[3, 5];
   }


   protected sealed override void ExtraWinConditions()
   {
      Game g = Game.Instance;
      IPlayer otherPlayer = Player.CurrentPlayer == Player.Player1 ? Player.Player2 : Player.Player1;
      if (Line.LineHistory[^1].GetEndDot() == otherPlayer.LastDot)
      {
         OnVictory(Player.CurrentPlayer);
         return;
      }
   }
}