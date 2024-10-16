using UnityEngine;

public class P2 : IPlayer
{
   public Color color { get; set; } = Color.blue;
   public Dot lastDot { get; set; }
   public string name { get; set; } = "Player 2";
}