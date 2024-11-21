using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class Vibration : MonoBehaviour, IPointerClickHandler
{
   [Header("Slider setup")] [SerializeField, Range(0, 1f)]
   protected float sliderValue;

   public static bool CurrentValue { get; private set; }

   private bool _previousValue;
   private Slider _slider;

   [Header("Animation")] [SerializeField, Range(0, 1f)]
   private float animationDuration = 0.5f;

   [SerializeField] private AnimationCurve slideEase =
      AnimationCurve.EaseInOut(0, 0, 1, 1);
   

   [Header("Events")] [SerializeField] private UnityEvent onToggleOn;
   [SerializeField] private UnityEvent onToggleOff;



   protected Action transitionEffect;

   private void SetupToggleComponents()
   {
      if (_slider != null)
         return;

      SetupSliderComponent();
   }

   private void SetupSliderComponent()
   {
      if(PlayerPrefs.GetInt("vibration") != null)
      {
         Debug.Log("Vibration value: " + PlayerPrefs.GetInt("vibration"));
         CurrentValue = PlayerPrefs.GetInt("vibration") == 1;
      }
      else{
         CurrentValue = true;
      }
      
      _slider = GetComponent<Slider>();
      if (_slider == null)
      {
         Debug.Log("No slider found!", this);
         return;
      }

      SetStateAndStartAnimation(CurrentValue);
      _slider.interactable = false;
      var sliderColors = _slider.colors;
      sliderColors.disabledColor = Color.white;
      _slider.colors = sliderColors;
      _slider.transition = Selectable.Transition.None;
   }




   protected virtual void Awake()
   {
      SetupSliderComponent();
   }

   public void OnPointerClick(PointerEventData eventData)
   {
      Toggle();
   }


   private void Toggle()
   {
         SetStateAndStartAnimation(!CurrentValue);
   }


   private void SetStateAndStartAnimation(bool state)
   {
      _previousValue = CurrentValue;
      CurrentValue = state;

      if (_previousValue != CurrentValue)
      {
         if (CurrentValue)
            onToggleOn?.Invoke();
         else
            onToggleOff?.Invoke();
      }

      LeanTween.cancel(gameObject, false);
      float startValue = _slider.value;
      float endValue = CurrentValue ? 1 : 0;
      switch (endValue)
      {
         case 1:
            LeanTween.value(gameObject, setColorCallback, Color.red, Color.green, animationDuration).setEase(slideEase);
            break;
         case 0:
            LeanTween.value(gameObject, setColorCallback, Color.green, Color.red, animationDuration).setEase(slideEase);
            break;
      }
      sliderValue = LeanTween.value(startValue, endValue, animationDuration).setEase(slideEase)
         .setOnUpdate((float val) => _slider.value = val).id;
      PlayerPrefs.SetInt("vibration", CurrentValue ? 1 : 0);
      PlayerPrefs.Save();
   }

   private void setColorCallback( Color c )
   {
      var img = GetComponentInChildren<Image>();
      img.color = c;
   }
   
}