using UnityEngine;

public class None : IPlayer
{
   public LineBehavior PlayerLine { get; set; }
   public Color Color { get; set; } = Color.white;
   public Dot LastDot { get; set; }
   public string Name { get; set; } = "Nobody";
}