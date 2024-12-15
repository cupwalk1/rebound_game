using UnityEngine;
using UnityEngine.UI;

public class GameInfo : MonoBehaviour
{
    [SerializeField] private Button _iButton;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Image _gameImage;
    [SerializeField] private GameObject _gameInfoPanel;
    [SerializeField] private Image _iicon;
    [SerializeField] private Image _xicon;
    private bool _isInfoPanelActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    
    public void ShowInfo()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.backBtn);
        if (!_isInfoPanelActive)
        {
            _gameImage.gameObject.GetComponent<Button>().interactable = false;
            _isInfoPanelActive = true;
            transform.LeanRotateY(180, .5f).setEaseLinear();
            Invoke("at90deg", .25f);
        }
        else
        {
            _isInfoPanelActive = false;
            transform.LeanRotateY(0, 0.5f).setEaseLinear();
            Invoke("at90deg", .25f);
        }
    }

    public void at90deg()
    {
        if (_isInfoPanelActive)
        {
            _iButton.transform.localRotation = Quaternion.Euler(180, 0, 0);
            _buttonImage.color = new Color32(88, 255, 122, 255);
            _iicon.enabled = false;
            _xicon.enabled = true;
            _gameInfoPanel.SetActive(true);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            _iButton.transform.localRotation = Quaternion.Euler(0, 0, 0);
            _buttonImage.color = Color.white;
            _iicon.enabled = true;
            _xicon.enabled = false;
            _gameInfoPanel.SetActive(false);
            transform.localScale = new Vector3(1, 1, 1);
            _gameImage.gameObject.GetComponent<Button>().interactable = true;
        }
    }
}
