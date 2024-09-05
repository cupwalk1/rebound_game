using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public static class PlayerPrefs
{
    public static string player1Name;
    public static Color player1Color = Color.red;
    public static string player2Name;
    public static Color player2Color = Color.blue;
    public static Color GetColor(string name)
    {
        if (name == "P1")
        {
            return player1Color;
        }
        else if (name == "P2")
        {
            return player2Color;
        }
        else
        {
            return Color.white;
        }
    }
    public static void SetColor(string name, Color color)
    {
        if (name == "P1")
        {
            player1Color = color;
        }
        else if (name == "P2")
        {
            player2Color = color;
        }
    }

}
public class MainMenu : MonoBehaviour
{
    public Text text;
    public GameObject startMenu;
    public GameObject P1Setup;
    public GameObject P2Setup;
    public Button startButton;
    public Slider colorSlider;
    public Image colorDisplay;
    public TMP_InputField inputField;
    private bool isP1 = false;
    private bool isP2 = false;
    [SerializeField] private Button readyP1;
    [SerializeField] private Button readyP2;
    [SerializeField] private GameObject Red;
    [SerializeField] private GameObject Yellow;
    [SerializeField] private GameObject Green;
    [SerializeField] private GameObject Blue;
    [SerializeField] private GameObject Purple;


    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Red.GetComponent<Image>().color = new Color(235 / 255f, 125 / 255f, 91 / 255f);
        Yellow.GetComponent<Image>().color = new Color(254 / 255f, 210 / 255f, 63 / 255f);
        Green.GetComponent<Image>().color = new Color(181 / 255f, 211 / 255f, 61 / 255f);
        Blue.GetComponent<Image>().color = new Color(108 / 255f, 162 / 255f, 234 / 255f);
        Purple.GetComponent<Image>().color = new Color(68 / 255f, 34 / 255f, 136 / 255f);
        readyP1.onClick.AddListener(OKP1);
        readyP2.onClick.AddListener(OKP2);
        startButton.onClick.AddListener(StartGame);
        P2Setup.SetActive(false);
        P1Setup.SetActive(false);
    }



    // This method will be called when the start button is clicked
    void StartGame()
    {
        // Load the game scene
        // Replace "GameScene" with the name of your game scene
        startMenu.SetActive(false);
        P1Setup.SetActive(true);
        P2Setup.SetActive(true);
    }

    public void OKP1()
    {
        if (!isP1)
        {
            isP1 = true;
            readyP1.GetComponent<Image>().color = Color.green;
            if (isP2)
            {
                Debug.Log("Both players are ready");
                OnOK();
            }

        }
        else
        {
            readyP1.GetComponent<Image>().color = Color.white;
            isP1 = false;
        }
    }

    public void OKP2()
    {
        if (!isP2)
        {
            readyP2.GetComponent<Image>().color = Color.green;
            isP2 = true;
            if (isP1)
            {
                Debug.Log("Both players are ready");
                OnOK();
            }
        }
        else
        {
            readyP2.GetComponent<Image>().color = Color.white;
            isP2 = false;
        }
    }

    public void OnOK()
    {
        SceneManager.LoadScene("GameScene");
    }
}