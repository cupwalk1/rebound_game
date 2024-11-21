using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class VictoryAnimation : MonoBehaviour
{
   private StringTable table;
   CanvasGroup canvasGroup;
   [SerializeField] GameObject homeButton;
   [SerializeField] GameObject victoryText;
   private void Start()
   {
      homeButton = transform.GetChild(1).gameObject;
      canvasGroup = gameObject.GetComponent<CanvasGroup>();
      canvasGroup.alpha = 0;
      transform.localScale = Vector3.zero;
      homeButton.transform.localScale = Vector3.zero;
   }

   public async Task ShowVictoryUI(IPlayer winner)
   {
      if (winner == Player.Player1)
      {
         victoryText.GetComponent<LocalizeStringEvent>().SetEntry("p1wins_key");
      }
      else if (winner == Player.Player2)
      {
         victoryText.GetComponent<LocalizeStringEvent>().SetEntry("p2wins_key");
      }
      else
      {
         victoryText.GetComponent<LocalizeStringEvent>().SetEntry("tie_key");
      }
      
      victoryText.GetComponent<TMP_Text>().color = winner.Color;
      
      GameObject[] particles = GameObject.FindGameObjectsWithTag("WinnerCoriandoli");
      foreach (GameObject particle in particles)
      {
         particle.GetComponent<ParticleSystem>().Play();
      }
      
      //hide all buttons
      foreach (GameObject button in GameObject.FindGameObjectsWithTag("Button"))
      {
         button.LeanScale(Vector3.zero, 0.5f).setEaseInBack();
      }

      canvasGroup.LeanAlpha(1, 1).setEaseOutQuad();
      transform.LeanScale(Vector3.one, 1).setEaseOutBack();
      await Task.Delay(2000);
      homeButton.LeanScale(Vector3.one, 1f).setEaseOutElastic();
      
   }

   public void AttachButtonListener()
   {
      SceneTransition.Instance.GetComponent<SceneTransition>().StartSceneChange();
   }
}
