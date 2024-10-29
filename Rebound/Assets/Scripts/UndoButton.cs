using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UndoButton : MonoBehaviour
{
   private Game g;
   public bool hasMoved = false;
   public LineBehavior linePath;
   public Dot CurrentDot;
   private Line lastLine;
   
   void Update()
   {
      if (Line.LineHistory.Count == 0)
      {
         GetComponent<Button>().interactable = false;
         return;
      }
      g = GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>().CurrentGame;
      lastLine = Line.LineHistory.Last();
      if (!g.inProgress || (lastLine.EndDot == g.startOfTurnDot && lastLine.LinePlayer != Player.CurrentPlayer))
      {
         GetComponent<Button>().interactable = false;
      }
      else
      {
         GetComponent<Button>().interactable = true;
      }
   }
   
   void Start()
   {
      GetComponent<Button>().onClick.AddListener(CheckUndo);
      g = GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>().CurrentGame;

   }

   private void CheckUndo()
   {
      g = GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>().CurrentGame;
      
      
      
      linePath = g.currentLinePath;
      if (Line.LineHistory.Count == 0) return;

      lastLine = Line.LineHistory.Last();


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
      g.ExtraUndoBehavior();
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