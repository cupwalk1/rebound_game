using System.Collections.Generic;
using UnityEngine;

public class DotChain
{
   IPlayer _player;
   List<Dot> _dots;
   bool _isLoop;
   public DotChain(List<Dot> dots, IPlayer player, bool isLoop = false)
   {
      _player = player;
      _dots = dots;
      _isLoop = isLoop;
   }


   public void CreateChain(bool isInvisible = false)
   {

      for (int i = 0; i < _dots.Count; i++)
      {
         _dots[i].Instance.GetComponent<SpriteRenderer>().sortingOrder = 9;
         Line line;
         if (i == _dots.Count - 1)
         {
            if(_isLoop)
            {
               line = new Line(_player, _dots[i], _dots[0]);
               if (isInvisible)
               {
                  line.SetColor(Color.clear);
               }
               else
               {
                  line.SetColor(_player.Color);
               }

            }
            break;
         }

         Dot currentDot = _dots[i];
         Dot nextDot = _dots[i + 1];

         // Check for outward turn and draw diagonal line
         line = new Line(Player.None, currentDot, nextDot);
         if (isInvisible)
         {
            line.SetColor(Color.clear);
         }
         else
         {
            line.SetColor(_player.Color);
         }
      }
   }
}