﻿
using KevinFeng;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region properties
    public InputMaster inputMaster;
    #region Camera
    [SerializeField]
    private float cameraPanSpeed;
    Vector2 movementInput;
    #endregion

    #region SelectBox
    private bool isDragging;
    private Vector2 dragStartPos;
    #endregion

    #region Define Notifications
    public const string Selected = "InputManager.Selected";
    public const string BoxSelected = "InputManager.BoxSelected";
    public const string GoTo = "InputManager.GoTo";
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
            if (hit.transform.CompareTag("Unit"))
            {
                this.PostNotification(Selected, new GameObject[] { hit.collider.gameObject });
            }
            else
            {
                isDragging = true;
                dragStartPos = Mouse.current.position.ReadValue();
            }
        }
    }

    /// <summary>
    /// Select objects when a drag is done
    /// </summary>
    /// <param name="ctx"></param>
    void OnLeftButtonUp(InputAction.CallbackContext ctx)
    {
        if (isDragging)
        {
            Vector2 boxEnd = Mouse.current.position.ReadValue();

            bool IsWithinSelectionBounds(Transform transform)
            {
                Bounds viewportBounds = ScreenHelper.GetViewportBounds(Camera.main, dragStartPos, Input.mousePosition);
                return viewportBounds.Contains(Camera.main.WorldToViewportPoint(transform.position));
            }

            GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
            GameObject[] selectedUnits = units.Where(u => IsWithinSelectionBounds(u.transform)).ToArray();
            this.PostNotification(Selected, selectedUnits);
            isDragging = false;
        }
    }

    /// <summary>
    /// Order goto
    /// </summary>
    /// <param name="ctx"></param>
    void OnRightClick(InputAction.CallbackContext ctx)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
            this.PostNotification(GoTo, hit.point);
    }
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
            ScreenHelper.DrawScreenRectBorder(new Rect(dragStartPos.x, Screen.height - dragStartPos.y, boxEnd.x - dragStartPos.x, dragStartPos.y - boxEnd.y), 1f, Color.green);
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
