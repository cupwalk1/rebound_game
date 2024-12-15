using System.Threading.Tasks;
using UnityEngine;

public class TutoralPointer : MonoBehaviour
{
    [SerializeField] private GameObject _arrow;
    private GameObject _tutorialText;
    public float distance = 20;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {transform.rotation = Quaternion.Euler(0, 0, -30);
        transform.SetParent(GameObject.Find("UI").transform);
        transform.localScale = new Vector3(1, 1, 1);
        _tutorialText = TutorialController.Instance._currentText;
        InvokeRepeating("Check", 0, .2f);
        _arrow.transform.LeanMoveLocalY(transform.position.y + distance, 1).setEaseInOutSine().setLoopPingPong();
    }
    
    void Check()
    {
        if (_tutorialText != TutorialController.Instance._currentText)
        {
            Destroy(gameObject);
        }
    }
    
}
