using UnityEngine;

public abstract class TwoLineGame : Game
{
   public abstract GameObject SecondDotCover { get; protected set; }
   protected virtual void ExtraWinConditions(){}
   protected sealed override void CheckForWin()
   {
      ExtraWinConditions();
      for (int i = 0; i < BoardWidth; i++)
      {
         for (int j = 0; j < BoardHeight; j++)
         {
            if (Dot.Board[i, j] == null)
            {
               return;
            }
            if (Dot.Board[i, j].GetAttachedLines().Count == 0)
            {
               return;
            }
         }
      }
      OnVictory(Player.none);
   }
   
   public sealed override void SwitchPlayer()
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