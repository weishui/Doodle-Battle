using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public const string Selected = "InputManager.Selected";

    [SerializeField]
    private float cameraPanSpeed;

    public InputMaster inputMaster;
    Vector2 movementInput;

    // Start is called before the first frame update
    void Awake()
    {
        inputMaster = new InputMaster();
        inputMaster.Player.Move.performed += ctx => { movementInput = ctx.ReadValue<Vector2>(); Debug.Log("moving"); };
        inputMaster.Player.Select.performed += ctx => OnLeftClick(ctx);
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }

    void OnLeftClick(InputAction.CallbackContext ctx)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log("Hit: " + hit.collider.ToString());
            this.PostNotification(Selected, hit.collider.gameObject);
        }

    }

    private void MoveCamera()
    {
        Camera.main.transform.Translate(movementInput * cameraPanSpeed * Time.deltaTime);
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
