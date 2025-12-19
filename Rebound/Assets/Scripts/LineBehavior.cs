using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;

public class LineBehavior : MonoBehaviour
{
    private InputAction pointerPress;
    private InputAction pointerPosition;

    private Game _g;
    private bool isPressed;

    void Awake()
    {
        pointerPress = new InputAction(
            "PointerPress",
            binding: "<Pointer>/press"
        );

        pointerPosition = new InputAction(
            "PointerPosition",
            binding: "<Pointer>/position"
        );

        pointerPress.Enable();
        pointerPosition.Enable();
    }

    void Start()
    {
        gameObject.SetActive(false);

        if (GameController.Instance != null)
        {
            GameController.Instance.OnBoardGenerated.AddListener(() =>
            {
                gameObject.SetActive(true);
            });
        }
    }

    void Update()
    {
        _g = Game.Instance;
        if (!_g.InProgress) return;

        if (pointerPress.WasPressedThisFrame())
        {
            isPressed = true;
            HandlePointerBegan();
        }
        else if (pointerPress.IsPressed() && isPressed)
        {
            HandlePointerMoved();
        }
        else if (pointerPress.WasReleasedThisFrame())
        {
            isPressed = false;
            HandlePointerEnded();
        }
    }

    private RaycastHit2D PointerRaycast()
    {
        Vector2 screenPos = pointerPosition.ReadValue<Vector2>();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        return Physics2D.Raycast(worldPos, Vector2.zero);
    }

    private void HandlePointerBegan()
    {
        RaycastHit2D hit = PointerRaycast();
        if (hit.collider == null) return;

        if (hit.collider.gameObject == _g.CurrentDot.Instance)
        {
            _g.OnBeginLine();
        }
    }

    private void HandlePointerMoved()
    {
        if (_g.CurrentLine == null) return;

        RaycastHit2D hit = PointerRaycast();

        Dot touchedDot = null;
        if (hit.collider != null)
        {
            touchedDot = _g.AvailableDots
                .FirstOrDefault(d => d.Instance == hit.collider.gameObject);
        }

        if (touchedDot == null)
        {
            _g.OnDragLine(new Touch
                { position = pointerPosition.ReadValue<Vector2>()});
        }
        else if (touchedDot.AttachedLines.Count > 0)
        {
            _g.OnBounce(touchedDot);
        }
        else
        {
            _g.OnEndLine(touchedDot);
        }
    }

    private void HandlePointerEnded()
    {
        if (_g.CurrentLine == null) return;

        if (_g.CurrentLine.GetEndDot() == null)
        {
            _g.OnLineCanceled();
        }
        else if (_g.CurrentLine.GetEndDot().AttachedLines.Count == 0)
        {
            _g.OnLineReleased();
        }
    }
}
