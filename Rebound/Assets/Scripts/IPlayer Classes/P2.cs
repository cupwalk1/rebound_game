using UnityEngine;

public class P2 : IPlayer
{
   public Color Color { get; set; } = Color.blue;
   public Dot LastDot { get; set; }
   public string Name { get; set; } = "Player 2";
}