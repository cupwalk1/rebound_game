using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    AsyncOperation asyncLoad;

    static SceneTransition Instance;
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

    private async void ShowAnimation()
    {
        LeanTween.cancel(gameObject);
        transform.position = new Vector3(15+Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x, 0, 0);
        transform.LeanMoveX(-15-Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x, 1.5f).setEaseInOutQuad();
    }
    
    public async Task StartSceneChange()
    {
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("GameButton"))
        {
            i.GetComponent<Button>().interactable = false;
        }
        ShowAnimation();
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        await Task.Delay(750);
        asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0) ? 1 : 0);
        asyncLoad.priority = 0;
        await asyncLoad;
    }

}
