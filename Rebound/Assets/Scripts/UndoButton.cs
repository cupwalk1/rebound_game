using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UndoButton : MonoBehaviour
{
   private Game _g;
   public bool hasMoved = false;
   public LineBehavior linePath;
   private Line _lastLine;
   
   void Update()
   {
      if (Line.LineHistory.Count == 0)
      {
         GetComponent<Button>().interactable = false;
         return;
      }
      _g = GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>().CurrentGame;
      _lastLine = Line.LineHistory.Last();
      //The cases for which the back button should be disabled.
      //If game's not in progress or (the StartOfTurnDot has been set to current dot by OnBeginLine() and the last line was of the opponent) or the player is dragging the line.... Phew, that was LOOONNNGGGGG.
      if (!_g.InProgress || (_g.CurrentDot == _g.StartOfTurnDot && _lastLine.LinePlayer != Player.CurrentPlayer) || (_g.CurrentLine != null &&  _g.CurrentLine?.GetEndDot() == null) )
      {
         if (GetComponent<Button>().interactable == true)  GetComponent<Button>().interactable = false;
      }
      else
      {
         if (GetComponent<Button>().interactable == false) GetComponent<Button>().interactable = true;
      }
   }
   
   void Start()
   {
      GetComponent<Button>().onClick.AddListener(CheckUndo);
      _g = GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>().CurrentGame;

   }

   private void CheckUndo()
   {
      _g = GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>().CurrentGame;
      
      
      
      linePath = _g.CurrentLinePath;
      if (Line.LineHistory.Count == 0) return;

      _lastLine = Line.LineHistory.Last();


      if (_lastLine.EndDot == _g.StartOfTurnDot && _lastLine.LinePlayer != Player.CurrentPlayer)
      {
         return;
      }
      else if (_lastLine.LinePlayer != Player.CurrentPlayer)
      {
         
         _g.SwitchPlayer();
         Undo();
         return;
      }
      else
      {
         Undo();
         return;
      }
   }

   private void Undo()
   {
      _g.ExtraUndoBehavior();
      _g.SetCurrentDot(Line.LineHistory.Last().GetStartDot());
      _g.DestroyLine(Line.LineHistory.Last());
      if (Line.LineHistory.Count == 0)
      {
         _g.SetCurrentDot(_g.StartOfGameDot);
         return;
      }
      

      if (BoardManager.Instance.GetOuterDots().Contains(Line.LineHistory.Last().GetStartDot()))
      {
         Line.LineHistory.Last().GetStartDot().SetColor(Color.white);
      }

      if (BoardManager.Instance.GetOuterDots().Contains(Line.LineHistory.Last().GetEndDot()))
      {
         Line.LineHistory.Last().GetEndDot().SetColor(Color.white);
      }
      
      
   }
}