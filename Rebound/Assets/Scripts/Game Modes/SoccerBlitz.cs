using System;
using UnityEngine;

public class SoccerBlitz : Game
{
   public sealed override GameObject Background { get; protected set; }
   public override Dot StartOfGameDot { get; protected set; }
   public sealed override int BoardHeight { get; protected set; }
   public sealed override int BoardWidth { get; protected set; }
   public override GameObject CurrentDotCover { get; protected set; }

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

   public override void CustomRules()
   {
      throw new NotImplementedException();
   }

   public override void CheckForWin()
   {
      throw new NotImplementedException();
   }


}