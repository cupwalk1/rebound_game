using System;
using UnityEngine;
using Screen = UnityEngine.Device.Screen;

public class TutorialController : MonoBehaviour
{
   static public TutorialController Instance;
   [SerializeField] private GameObject[] _tutorialTexts;
   [SerializeField] private GameObject ArrowPoint;
   Vector3 popupPos = new Vector3(Screen.width / 2, Screen.height / 4, 1);
   public int _currentTextIndex;
   public int CurrentTextIndex
   {
      get{ return _currentTextIndex; }
      set
      {
         _currentTextIndex = value;
         UpdateTutorialText();
      }
   }

   private void Awake()
   {
      Instance = this;
   }

   private Tutorial _g;

   public Dot dot1
   {
      get { return Dot.Board[3, 5]; }
   }
   
   public Dot dot2
   {
      get { return Dot.Board[3, 4]; }
   }
   
   public Dot dot3
   {
      get { return Dot.Board[2, 4]; }
   }
   
   public Dot dot4
   {
      get { return Dot.Board[1, 5]; }
   }
   
   public Dot dot5
   {
      get { return Dot.Board[0, 4]; }
   }
   
   public Dot dot6
   {
      get { return Dot.Board[1, 3]; }
   }
   
   

   public GameObject _currentText
   {
      get
      {
         if (_currentTextIndex < 0 || _currentTextIndex >= _tutorialTexts.Length)
         {
            return null;
         }
         return _tutorialTexts[_currentTextIndex];
      }
      set { }
   }

   private void UpdateTutorialText()
   {
      if (_currentText == null)
      {
         return;
      }
      float y = Screen.width / Camera.main.aspect;
      foreach (GameObject i in _tutorialTexts)
      {
         if (_currentText == i)
         {
            i.transform.LeanMove(Camera.main.ScreenToWorldPoint(popupPos), 0.5f).setEaseOutBack();
         }
         else
         {
            i.transform.LeanMove(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, -y / 3, 0)), .5f);
         }
      }
      _currentText.transform.localScale = Vector3.zero;
      _currentText.SetActive(true);
      _currentText.transform.LeanScale(Vector3.one, 0.5f).setEaseOutBack();
      if (CurrentTextIndex == 0)
      {
         Invoke("NextTutorialText", 2);
      }
      if(CurrentTextIndex == 6)
      {
         Invoke("NextTutorialText", 3);
      }
      if (CurrentTextIndex == 1) Instantiate(ArrowPoint, dot1.Instance.transform.position, Quaternion.identity);
      if (CurrentTextIndex == 2) Instantiate(ArrowPoint, dot2.Instance.transform.position, Quaternion.identity);
      if (CurrentTextIndex == 3) Instantiate(ArrowPoint, dot3.Instance.transform.position, Quaternion.identity);
      if (CurrentTextIndex == 4) Instantiate(ArrowPoint, dot4.Instance.transform.position, Quaternion.identity);
      if (CurrentTextIndex == 5) Instantiate(ArrowPoint, dot6.Instance.transform.position, Quaternion.identity);
      
   }
   private void NextTutorialText()
   {
      CurrentTextIndex++;
   }

   void Start()
   {
      CurrentTextIndex = 0;
      _g = (Tutorial)Game.Instance;
      gameObject.transform.localScale = Vector3.one;

      _currentText = _tutorialTexts[0];
      _currentText.transform.localScale = Vector3.zero;
      _currentText.SetActive(true);
      _currentText.transform.LeanScale(Vector3.one, 0.5f).setEaseOutBack();

      float y = Screen.width / Camera.main.aspect;

      foreach (GameObject i in _tutorialTexts)
      {
         i.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, -y / 3, 0));
      }
   }
}