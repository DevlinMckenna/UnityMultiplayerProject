using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/InputReader")]


public class InputReader : ScriptableObject, IPlayerActions //<==this is the interface im having issues with - Devlin 
{

    public event Action<bool> PrimaryShootEvent;
    //named this event diffrently than the video but that shouldnt matter as long as im consistant in my OnPrimaryShoot method right?
    public event Action<Vector2> PrimaryMoveEvent;
    private Controls controls;

    private void OnEnable()
    {
        if (controls == null )
        {
            controls = new Controls();
            controls.Player0.SetCallBacks(this);
        }

        controls.Player0.Enable();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        PrimaryMoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPrimaryShoot(InputAction.CallbackContext context){

        if(context.performed){PrimaryShootEvent?.Invoke(true);}
        else if (context.canceled){PrimaryShootEvent?.Invoke(false);}
        

    }
}
