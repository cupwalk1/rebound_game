using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocaleSelector : MonoBehaviour
{
   TMP_Dropdown dropdown;

   public void SetLocale(string locale)
   {
      LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(locale);
      PlayerPrefs.SetString("locale", locale);
      PlayerPrefs.Save();
   }

   private void Start()
   {
      SetLocale(PlayerPrefs.GetString("locale"));
      if (PlayerPrefs.GetString("locale") == null)
      {
         SetLocale("en");
      }
      dropdown = GetComponent<TMP_Dropdown>();
      switch (PlayerPrefs.GetString("locale"))
      {
         case "en":
            dropdown.value = 0;
            break;
         case "it":
            dropdown.value = 1;
            break;
      }

      dropdown.onValueChanged.AddListener(delegate
      {
         switch (dropdown.value)
         {
            case 0:
               SetLocale("en");
               break;
            case 1:
               SetLocale("it");
               break;
         }
      });
   }
}