using System.Linq;
using UnityEngine;


public sealed class Sumo : TwoLineGame
{
   public Sumo()
   {
      BoardHeight = 7;
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
      StartOfGameDot = Dot.Board[BoardWidth / 2, BoardHeight / 2];
      Player.Player1.LastDot = StartOfGameDot;
      Player.Player2.LastDot = StartOfGameDot;
   }
   
}