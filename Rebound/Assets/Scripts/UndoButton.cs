using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UndoButton : MonoBehaviour
{
   private Game g;
   public bool hasMoved = false;
   public LineBehavior linePath;
   public Dot CurrentDot;

   // Start is called before the first frame update
   void Start()
   {
      GetComponent<Button>().onClick.AddListener(CheckUndo);
   }

   private void CheckUndo()
   {
      g = GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>().CurrentGame;
      linePath = g.currentLinePath;
      if (Line.LineHistory.Count == 0) return;

      Line lastLine = Line.LineHistory.Last();


      if (lastLine.EndDot == g.startOfTurnDot && lastLine.LinePlayer != Player.CurrentPlayer)
      {
         return;
      }
      else if (lastLine.LinePlayer != Player.CurrentPlayer)
      {
         g.SwitchPlayer();
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
      g.SetCurrentDot(Line.LineHistory.Last().GetStartDot());
      g.DestroyLine(Line.LineHistory.Last());
      if (Line.LineHistory.Count == 0)
      {
         g.SetCurrentDot(g.StartOfGameDot);
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