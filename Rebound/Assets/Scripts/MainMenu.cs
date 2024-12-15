using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject SettingsGear;
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject CreditsPanel;
    [SerializeField] private GameObject CreditsMenu;
    [SerializeField] private float x;
    [SerializeField] private float y;
    
    
    public float RotateSpeed = 5;

    void Update()
    {
        transform.Rotate(Vector3.right, Time.deltaTime * RotateSpeed);
    }
    
    public void SetLocale(string locale)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(locale);
        PlayerPrefs.SetString("locale", locale);
        PlayerPrefs.Save();
    }
    
    public void Start()
    {
        
        SetLocale(PlayerPrefs.GetString("locale"));
        if (PlayerPrefs.GetString("locale") == null)
        {
            SetLocale("en");
        }
        if (PlayerPrefs.GetFloat("volume") == null)
        {
            PlayerPrefs.SetFloat("volume", 1f);
        }
        if (PlayerPrefs.GetInt("vibration") == null)
        {
            PlayerPrefs.SetInt("vibration", 1);
        }
        PlayerPrefs.Save();
        SettingsPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        x = Camera.main.ScreenToWorldPoint(new Vector3(Screen.height, 0, 0)).x;
        y = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.width, 0)).y;
        SettingsMenu.transform.SetPositionAndRotation(new Vector3(0, -x-2, 0), new Quaternion(0, 0, 0, 0));
        CreditsMenu.transform.SetPositionAndRotation(new Vector3(-y-10, 0, 0), new Quaternion(0, 0, 0, 0));
    }
    
    public void OnTutorialClick()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.click);
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.StartGame(Game.GameType.Tutorial);
         
    }

    public void OnSoccerClick()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.click);
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.StartGame(Game.GameType.Soccer);
         
    }

    public void OnSumoClick()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.click);
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.StartGame(Game.GameType.Sumo);
    }

    public void OnHockeyClick()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.click);
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.StartGame(Game.GameType.Hockey);
    }

    public void OnFencingClick()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.click);
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.StartGame(Game.GameType.Fencing);
    }

    public void OnSoccerBlitzClick()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.click);
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.StartGame(Game.GameType.SoccerBlitz);
    }
    
    public void OnSettingsOpen()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.click);
        SettingsPanel.SetActive(true);
        SettingsMenu.transform.LeanMoveY(0, 1f).setEaseOutElastic().period = 1.2f;
    }
    
    public void OnSettingsClose()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.click);
        PlayerPrefs.Save();
        SettingsMenu.transform.LeanMoveY(-x-2, 1f).setEaseInQuad().setOnComplete(() => SettingsPanel.SetActive(false));
    }
    
    public void OnCreditsOpen()
    {
        OnSettingsClose();
        SoundManager.Instance.PlaySFX(SoundManager.Instance.click);
        CreditsPanel.SetActive(true);
        CreditsMenu.transform.LeanMoveX(0, 1f).setEaseOutElastic().period = 1.2f;
    }
    
    public void OnCreditsClose()
    {
        OnSettingsOpen();
        SoundManager.Instance.PlaySFX(SoundManager.Instance.click);
        PlayerPrefs.Save();
        CreditsMenu.transform.LeanMoveX(-y-10, 1f).setEaseOutQuad().setOnComplete(() => CreditsPanel.SetActive(false));
    }

    public void OnDonateClick()
    {
        Application.OpenURL("https://buymeacoffee.com/cupwalk1");
    }

}