using UnityEngine;

public class None : IPlayer
{
   public LineBehavior playerLine { get; set; }
   public Color color { get; set; } = Color.white;
   public Dot lastDot { get; set; }
   public string name { get; set; } = "Nobody";
}