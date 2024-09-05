using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorController : MonoBehaviour
{

    public GameObject P1Setup;
    public GameObject P2Setup;
    public GameObject thisSetup;
    public string thisPlayer;
    [SerializeField] private GameObject Red;
    [SerializeField] private GameObject Yellow;
    [SerializeField] private GameObject Green;
    [SerializeField] private GameObject Blue;
    [SerializeField] private GameObject Purple;
    [SerializeField] private Text playerText;
    // Start is called before the first frame update
    void Start()
    {

        if (this.gameObject == GameObject.Find("PlayerSetup1"))
        {
            thisSetup = P1Setup;
            thisPlayer = "P1";
        }
        else
        {
            Debug.Log("P2");
            thisPlayer = "P2";
            thisSetup = P2Setup;
        }
        Red.GetComponent<Image>().color = new Color(235 / 255f, 125 / 255f, 91 / 255f);
        Yellow.GetComponent<Image>().color = new Color(254 / 255f, 210 / 255f, 63 / 255f);
        Green.GetComponent<Image>().color = new Color(181 / 255f, 211 / 255f, 61 / 255f);
        Blue.GetComponent<Image>().color = new Color(108 / 255f, 162 / 255f, 234 / 255f);
        Purple.GetComponent<Image>().color = new Color(68 / 255f, 34 / 255f, 136 / 255f);

        Red.GetComponent<Button>().onClick.AddListener(() => UpdateColor(Red.GetComponent<Image>().color));
        Yellow.GetComponent<Button>().onClick.AddListener(() => UpdateColor(Yellow.GetComponent<Image>().color));
        Green.GetComponent<Button>().onClick.AddListener(() => UpdateColor(Green.GetComponent<Image>().color));
        Blue.GetComponent<Button>().onClick.AddListener(() => UpdateColor(Blue.GetComponent<Image>().color));
        Purple.GetComponent<Button>().onClick.AddListener(() => UpdateColor(Purple.GetComponent<Image>().color));
    }

    void UpdateColor(Color color)
    {
        PlayerPrefs.SetColor(thisPlayer, color);
        playerText.color = color;
        // Sample a color from the gradient based on the slider's value
    }
    // Update is called once per frame

}
