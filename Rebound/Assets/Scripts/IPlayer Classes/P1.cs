using UnityEngine;

public class P1 : IPlayer
{
   public LineBehavior PlayerLine { get; set; }
   public Color Color { get; set; } = Color.red;
   public Dot LastDot { get; set; }
   public string Name { get ; set; } = "Player 1";
}