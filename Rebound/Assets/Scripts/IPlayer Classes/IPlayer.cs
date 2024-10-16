using UnityEngine;

public interface IPlayer
{
   public Color color { get; set; }
   public Dot lastDot { get; set; }
   string name { get; set; }
}