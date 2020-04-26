
using KevinFeng;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    #region properties

    public InputMaster inputMaster;

    #region Camera
    [SerializeField]
    private float cameraPanSpeed;
    Vector2 movementInput;
    #endregion

    #region Selection
    private List<GameObject> selectedObjects;
    private bool isDragging;
    private Vector2 dragStartPos;
    [SerializeField]
    private Texture boxTex;
    #endregion

    #endregion

    #region MonoBehaviour
    void Awake()
    {
        inputMaster = new InputMaster();
        inputMaster.Player.Move.performed += ctx => { movementInput = ctx.ReadValue<Vector2>(); };
        inputMaster.Player.Select.performed += ctx => OnLeftClick(ctx);
        inputMaster.Player.Select.canceled += ctx => OnLeftButtonUp(ctx);
        inputMaster.UI.RightClick.performed += ctx => OnRightClick(ctx);
    }

    
    void Update()
    {
        MoveCamera();
    }

    private void OnGUI()
    {
        DrawSelectBox();
    }
    #endregion

    #region Notification Posters
    void OnLeftClick(InputAction.CallbackContext ctx)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.CompareTag("Unit") || hit.transform.CompareTag("Building"))
            {
                DeselectAllInList();
                selectedObjects = new List<GameObject> { hit.collider.gameObject };
                SelectAllInList();
            }
            else
            {
                isDragging = true;
                dragStartPos = Mouse.current.position.ReadValue();
            }
        }
    }

    void OnLeftButtonUp(InputAction.CallbackContext ctx)
    {
        if (isDragging)
        {
            DeselectAllInList();
            GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
            GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
            units = units.Where(u => IsWithinSelectionBounds(u.transform)).ToArray();
            buildings = buildings.Where(u => IsWithinSelectionBounds(u.transform)).ToArray();

            if (units.Length > 0)
                selectedObjects = units.ToList();
            else
                selectedObjects = buildings.ToList();
            SelectAllInList();
            isDragging = false;
        }
    }

    void OnRightClick(InputAction.CallbackContext ctx)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
            foreach (GameObject o in selectedObjects)
                ExecuteEvents.Execute<IMoveEventHandler>(o, null, (x, y) => x.OnDestination(hit.point));
    }

    #region helpers
    void DeselectAllInList()
    {
        if (selectedObjects != null)
            foreach (var item in selectedObjects)
                ExecuteEvents.Execute<ISelectEventHandler>(item, null, (x, y) => x.SwitchSelected(false));
    }

    void SelectAllInList()
    {
        if (selectedObjects != null)
            foreach (var item in selectedObjects)
                ExecuteEvents.Execute<ISelectEventHandler>(item, null, (x, y) => x.SwitchSelected(true));
    }

    bool IsWithinSelectionBounds(Transform transform)
    {
        Bounds viewportBounds = ScreenHelper.GetViewportBounds(Camera.main, dragStartPos, Input.mousePosition);
        return viewportBounds.Contains(Camera.main.WorldToViewportPoint(transform.position));
    }
    #endregion

    #endregion

    #region MonoBehaviour related
    private void MoveCamera()
    {
        Camera.main.transform.Translate(movementInput * cameraPanSpeed * Time.deltaTime);
    }

    private void DrawSelectBox()
    {
        Vector2 boxEnd = Mouse.current.position.ReadValue();

        if (isDragging)
        {
            Rect selectBox = new Rect(dragStartPos.x, Screen.height - dragStartPos.y, boxEnd.x - dragStartPos.x, dragStartPos.y - boxEnd.y);
            ScreenHelper.DrawScreenRectBorder(selectBox, 1f, Color.green);
            GUI.DrawTexture(selectBox, boxTex);
        }
    }
    #endregion

    #region OnEnable & OnDisable
    private void OnEnable()
    {
        inputMaster.Enable();
    }

    private void OnDisable()
    {
        inputMaster.Disable();
    }
    #endregion
}

