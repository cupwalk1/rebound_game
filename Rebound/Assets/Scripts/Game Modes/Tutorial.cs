using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : Game
{
   [SerializeField] private GameObject _tutorialTexts;
   private TutorialController _tutorialController;
   private LineBehavior _lineBehavior;
   public UnityEvent OnBounce_event = new UnityEvent();
   public UnityEvent OnEndLine_event = new UnityEvent();

   public GameObject _background;

   public override GameObject Background
   {
      get { return _background; }
      protected set { _background = value; }
   }

   public override Dot StartOfGameDot { get; protected set; }
   public sealed override int BoardHeight { get; protected set; }
   public override int BoardWidth { get; protected set; }
   readonly List<Dot> _p1GoalDots = new();
   readonly List<Dot> _p2GoalDots = new();

   protected Tutorial()
   {
      // GameController.Instance.homeButton.SetActive(false);
      // GameController.Instance.undoButton.SetActive(false);
      BoardHeight = 9;
      BoardWidth = 5;
   }

   public override void CustomBoardSetup(int boxesX, int boxesY)
   {
      
      
      _tutorialTexts = Instantiate(_tutorialTexts);
      _tutorialTexts.transform.SetParent(GameObject.Find("UI").transform);
      _tutorialController = _tutorialTexts.GetComponent<TutorialController>();

      int goalBoxes = Mathf.CeilToInt((boxesX / 3f) / 2) * 2;
      for (int i = 0; i < (boxesX - goalBoxes) / 2; i++)
      {
         UnityEngine.Object.Destroy(Dot.Board[i, 0].Instance);
         UnityEngine.Object.Destroy(Dot.Board[i, boxesY].Instance);
         UnityEngine.Object.Destroy(Dot.Board[boxesX - i, 0].Instance);
         UnityEngine.Object.Destroy(Dot.Board[boxesX - i, boxesY].Instance);
         Dot.Board[i, 0] = null;
         Dot.Board[i, boxesY] = null;
         Dot.Board[boxesX - i, 0] = null;
         Dot.Board[boxesX - i, boxesY] = null;
      }

      StartOfGameDot = Dot.Board[(boxesX / 2), boxesY / 2];
      Player.Player1.LastDot = StartOfGameDot;
      Player.Player2.LastDot = StartOfGameDot;

      for (int i = 0; i <= goalBoxes; i++)
      {
         _p1GoalDots.Add(Dot.Board[(boxesX - goalBoxes) / 2 + i, boxesY - 1]);
         _p2GoalDots.Add(Dot.Board[(boxesX - goalBoxes) / 2 + i, 1]);
      }

      for (int i = 0; i < _p1GoalDots.Count; i++)
      {
         _p1GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Player.Player1.Color;
         if (i == 0)
         {
            _p1GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Color.white;
         }

         if (i == _p1GoalDots.Count - 1)
         {
            _p1GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Color.white;
            continue;
         }

         Line l = new(Player.None, _p1GoalDots[i], _p1GoalDots[i + 1]);
         l.SetColor(Player.Player1.Color);
      }

      for (int i = 0; i < _p2GoalDots.Count; i++)
      {
         _p2GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Player.Player2.Color;
         if (i == 0)
         {
            _p2GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Color.white;
         }

         if (i == _p2GoalDots.Count - 1)
         {
            _p2GoalDots[i].Instance.GetComponent<SpriteRenderer>().color = Color.white;
            continue;
         }

         Line l = new(Player.None, _p2GoalDots[i], _p2GoalDots[i + 1]);
         l.SetColor(Player.Player2.Color);
         l.Instance.GetComponent<LineRenderer>().sortingOrder = 8;
      }
      //
   }

   protected override void CheckForWin()
   {
      IPlayer winner;
      if (_p1GoalDots.Contains(CurrentLine.EndDot) && Player.Player1 == CurrentLine.LinePlayer)
      {
         OnVictory(Player.Player1);
         _tutorialController.CurrentTextIndex++;
      }

      if (_p2GoalDots.Contains(CurrentLine.EndDot) && Player.Player2 == CurrentLine.LinePlayer)
      {
         OnVictory(Player.Player2);
         _tutorialController.CurrentTextIndex++;
      }
   }

   public override void OnLineReleased()
   {
      if (_tutorialController.CurrentTextIndex <= 6)
      {
         _tutorialController.CurrentTextIndex++;
      }
      base.OnLineReleased();
   }

   public override void OnBounce(Dot touchedDot)
   {
      if (_tutorialController.CurrentTextIndex == 5)
      {
         if(touchedDot != _tutorialController.dot5)
         {
            base.OnDragLine(Input.GetTouch(0));
            return;
         }
         base.OnBounce(touchedDot);
         return;
      }
      if (_tutorialController.CurrentTextIndex == 3)
      {
         if(touchedDot != _tutorialController.dot3)
         {
            base.OnDragLine(Input.GetTouch(0));
            return;
         }
         _tutorialController.CurrentTextIndex++;
         base.OnBounce(touchedDot);
         return;
      }
      //disable bounce on the second line
      if (_tutorialController.CurrentTextIndex == 2)
      {
         base.OnDragLine(Input.GetTouch(0));
         return;
      }
      OnBounce_event.Invoke();
      base.OnBounce(touchedDot);
   }

   public override void OnDragLine(Touch touch)
   {
      if (_tutorialController.CurrentTextIndex == 0)
         OnBounce_event.Invoke();
      base.OnDragLine(touch);
   }

   public override void OnEndLine(Dot touchedDot)
   {
      if (_tutorialController.CurrentTextIndex == 1 && touchedDot != _tutorialController.dot1)
      {
         base.OnDragLine(Input.GetTouch(0));
         return;
      }
      if (_tutorialController.CurrentTextIndex == 2 && touchedDot != _tutorialController.dot2)
      {
         base.OnDragLine(Input.GetTouch(0));
         return;
      }
      if (_tutorialController.CurrentTextIndex == 3 && touchedDot != _tutorialController.dot3)
      {
         base.OnDragLine(Input.GetTouch(0));
         return;
      }
      if (_tutorialController.CurrentTextIndex == 4 && touchedDot != _tutorialController.dot4)
      {
         base.OnDragLine(Input.GetTouch(0));
         return;
      }
      if (_tutorialController.CurrentTextIndex == 5 && touchedDot != _tutorialController.dot6)
      {
         base.OnDragLine(Input.GetTouch(0));
         return;
      }

      

      OnEndLine_event.Invoke();
      base.OnEndLine(touchedDot);
   }
}