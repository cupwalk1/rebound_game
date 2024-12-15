using System.Linq;
using System.Threading.Tasks;
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
      _g = Game.Instance;
      _lastLine = Line.LineHistory.Last();
      //The cases for which the back button should be disabled.
      //If game's not in progress or (the StartOfTurnDot has been set to current dot by OnBeginLine() and the last line was of the opponent) or the player is dragging the line, or the tutorial is currently running.... Phew, that was LOOONNNGGGGG.
      if (!_g.InProgress || (_g.CurrentDot == _g.StartOfTurnDot && _lastLine.LinePlayer != Player.CurrentPlayer) || (_g.CurrentLine != null &&  _g.CurrentLine?.GetEndDot() == null) || (TutorialController.Instance != null && TutorialController.Instance.CurrentTextIndex <= 6) )
      {
         
         
         if (GetComponent<Button>().interactable == true)  GetComponent<Button>().interactable = false;
      }
      else
      {
         if (GetComponent<Button>().interactable == false) GetComponent<Button>().interactable = true;
      }
   }
   
   private async Task WaitForGameLoad()
   {
      while (GameController.Instance?.CurrentGame == null) await Task.Delay(10);
      _g = Game.Instance;
   }
   
   void Start()
   {
      gameObject.SetActive(false);
      GetComponent<Button>().onClick.AddListener(CheckUndo);
      GameController.Instance.OnGameStart.AddListener(() => gameObject.SetActive(true));
   }

   private void CheckUndo()
   {
      _g = Game.Instance;
      
      
      
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
      
      SoundManager.Instance.PlaySFX(SoundManager.Instance.backBtn);
      
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