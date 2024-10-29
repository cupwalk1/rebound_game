using UnityEngine;

public interface IPlayer
{
   public Color Color { get; set; }
   public Dot LastDot { get; set; }
   string Name { get; set; }
}