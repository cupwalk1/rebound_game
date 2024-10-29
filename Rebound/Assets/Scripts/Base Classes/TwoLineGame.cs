using UnityEngine;

public abstract class TwoLineGame : Game
{
   public override Dot CurrentDot
   {
      get => Player.CurrentPlayer.LastDot;
      set { Player.CurrentPlayer.LastDot = value; }
   }

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
      OnVictory(Player.None);
   }
   
   public sealed override void SwitchPlayer()
   {
      base.SwitchPlayer();
      CurrentDot = Player.CurrentPlayer.LastDot;
   }
   
   public override void OnBeginLine()
   {
      base.OnBeginLine();
      IPlayer otherPlayer = Player.CurrentPlayer == Player.Player1 ? Player.Player2 : Player.Player1;
      if (CurrentLine.GetStartDot() == StartOfGameDot) return;
      if (Line.LineHistory[^2]?.LinePlayer != Player.CurrentPlayer && CurrentLine.GetStartDot() != StartOfGameDot) StartOfTurnDot = Player.CurrentPlayer.LastDot;
      
      
   }
}