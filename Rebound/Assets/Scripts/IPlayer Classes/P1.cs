using UnityEngine;

public class P1 : IPlayer
{
   public LineBehavior playerLine { get; set; }
   public Color color { get; set; } = Color.red;
   public Dot lastDot { get; set; }
   public string name { get ; set; } = "Player 1";
}