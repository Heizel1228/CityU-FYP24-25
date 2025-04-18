using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    public PlayerInput.OnHandActions onHand;

    private PlayerMotor motor;
    private PlayerLook look;


    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        onHand = playerInput.OnHand;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Sprint.performed += ctx => motor.Sprint(true);  // 按下 Left Shift 時觸發
        onFoot.Sprint.canceled += ctx => motor.Sprint(false); // 放開 Left Shift 時觸發


    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //tell the playermotor to move using  the value from our movement action.
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
        onHand.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
        onHand.Disable();

        onFoot.Jump.performed -= ctx => motor.Jump();
        onFoot.Crouch.performed -= ctx => motor.Crouch();
        onFoot.Sprint.performed -= ctx => motor.Sprint(true);
        onFoot.Sprint.canceled -= ctx => motor.Sprint(false);
    }



}
