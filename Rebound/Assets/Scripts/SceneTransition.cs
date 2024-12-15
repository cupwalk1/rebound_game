using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        DontDestroyOnLoad(this);
        transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
        transform.position = new Vector3(15+Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x, 0, 0);
        GetComponent<SpriteRenderer>().color = Color.black;
    }

    private async void ShowAnimation1()
    {
        LeanTween.cancel(gameObject);
        transform.position = new Vector3(15+Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x, 0, 0);
        transform.LeanMoveX(0, .6f).setEaseInQuad();
    }    
    private async void ShowAnimation2()
    {
        LeanTween.cancel(gameObject);
        transform.LeanMoveX(-15-Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x, .6f).setEaseOutQuad();
    }
    
    public async Task StartSceneChange()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.click);
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("GameButton"))
        {
            i.GetComponent<Button>().interactable = false;
        }
        ShowAnimation1();
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        await Task.Delay(600);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0) ? 1 : 0);
        asyncLoad.priority = 0;
        await asyncLoad; 
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("GameButton"))
        {
            i.GetComponent<Button>().interactable = false;
        }
        await Task.Delay(250);
        ShowAnimation2();
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("GameButton"))
        {
            i.GetComponent<Button>().interactable = true;
        }
    }

}
