using System;
using UnityEngine;

public class Golf : Game
{

   public override GameObject Background { get; protected set; }
   public override Dot StartOfGameDot { get; protected set; }
   public override int BoardHeight { get; protected set; }
   public override int BoardWidth { get; protected set; }
   public override GameObject CurrentDotCover { get; protected set; }

   public Golf()
   {
      BoardHeight = 15;
      BoardWidth = 7;
      Background = Resources.Load<GameObject>("Prefabs/GolfBackground");
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