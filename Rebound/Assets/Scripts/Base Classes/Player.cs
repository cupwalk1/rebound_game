using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Diagnostics;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;


public static class Player
{
   public static IPlayer Player1;
   public static IPlayer Player2;
   public static IPlayer None;
   public static IPlayer CurrentPlayer;
   public static IPlayer Opponent => CurrentPlayer == Player1 ? Player2 : Player1;
   public static readonly Color Player1Color = Color.red;
   public static readonly Color Player2Color = Color.blue; 
   
   static Player()
   {
      Player1 = new P1();
      Player2 = new P2();
      None = new None();
      CurrentPlayer = Player1;
   }
   
}