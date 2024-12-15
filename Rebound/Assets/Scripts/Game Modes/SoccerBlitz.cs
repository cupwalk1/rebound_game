using System;

using UnityEngine;

public sealed class SoccerBlitz : Game
{
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


   public SoccerBlitz()
   {
      Background = Resources.Load<GameObject>("Prefabs/SoccerBlitzBackground");
      BoardHeight = 15;
      BoardWidth = 7;
   }

   public override void CustomBoardSetup(int boxesX, int boxesY)
   {
      throw new NotImplementedException();
   }

   protected override void CheckForWin()
   {
      throw new NotImplementedException();
   }


}