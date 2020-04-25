
using KevinFeng;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public const string Selected = "InputManager.Selected";
    public const string BoxSelected = "InputManager.BoxSelected";
    public const string GoTo = "InputManager.GoTo";

    [SerializeField]
    private float cameraPanSpeed;

    public InputMaster inputMaster;
    Vector2 movementInput;

    #region SelectBox
    private bool isDragging;
    private Vector2 boxStart;

    private Rect selectBox;
    public Texture boxTex;

    private GameObject[] units;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        inputMaster = new InputMaster();
        inputMaster.Player.Move.performed += ctx => { movementInput = ctx.ReadValue<Vector2>(); };
        inputMaster.Player.Select.performed += ctx => OnLeftClick(ctx);
        inputMaster.UI.RightClick.performed += ctx => OnRightClick(ctx);
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        Vector2 boxEnd = Mouse.current.position.ReadValue();

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            OnDragSelection();
            isDragging = false;
        }

        selectBox = new Rect(boxStart.x, Screen.height - boxStart.y, boxEnd.x - boxStart.x, boxStart.y - boxEnd.y);
    }

    void OnDragSelection()
    {
        units = GameObject.FindGameObjectsWithTag("Unit");
        GameObject[] selectedUnits = units.Where(u => IsWithinSelectionBounds(u.transform)).ToArray();

        this.PostNotification(Selected, selectedUnits);

    }

    void OnLeftClick(InputAction.CallbackContext ctx)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.CompareTag("Unit"))
            {
                this.PostNotification(Selected, new GameObject[] { hit.collider.gameObject });
            }
            else
            {
                isDragging = true;
                boxStart = Mouse.current.position.ReadValue();
                Debug.Log("Dragging");
            }
        }

    }

    void OnRightClick(InputAction.CallbackContext ctx)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            this.PostNotification(GoTo, hit.point);
        }

    }

    private void MoveCamera()
    {
        Camera.main.transform.Translate(movementInput * cameraPanSpeed * Time.deltaTime);
    }

    private bool IsWithinSelectionBounds(Transform transform)
    {
        var camera = Camera.main;
        Bounds viewportBounds = ScreenHelper.GetViewportBounds(Camera.main, boxStart, Input.mousePosition);
        return viewportBounds.Contains(camera.WorldToViewportPoint(transform.position));
    }

    private void OnGUI()
    {
        if (isDragging)
        {
            //GUI.DrawTexture(selectBox, boxTex);
            ScreenHelper.DrawScreenRectBorder(selectBox, 1f, Color.green);
        }

    }

    private void OnEnable()
    {
        inputMaster.Enable();
    }

    private void OnDisable()
    {
        inputMaster.Disable();
    }
}
