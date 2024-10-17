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
   public static IPlayer player1;
   public static IPlayer player2;
   public static IPlayer none;
   public static IPlayer CurrentPlayer;

   public static readonly Color player2Color = Color.blue; 
   
   static Player()
   {
      player1 = new P1();
      player2 = new P2();
      none = new None();
      CurrentPlayer = player1;
   }

   public static void SwitchPlayer()
   {

   }
}