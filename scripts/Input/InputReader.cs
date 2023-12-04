using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/InputReader")]


public class InputReader : ScriptableObject, IPlayer0Actions 
{

    public event Action<bool> PrimaryFireEvent;
   
    public event Action<Vector2> PrimaryMoveEvent;

    public Vector2 AimPosition {get; private set;}
    private Controls controls;

    private void OnEnable()
    {
        if (controls == null )
        {
            controls = new Controls();
            controls.Player0.SetCallbacks(this);
        }

        controls.Player0.Enable();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        PrimaryMoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPrimaryFire(InputAction.CallbackContext context){


        if(context.performed){PrimaryFireEvent?.Invoke(true);}
        else if (context.canceled){PrimaryFireEvent?.Invoke(false);}
        

    }

    public void OnAim(InputAction.CallbackContext context){
       AimPosition = context.ReadValue<Vector2>();
    }
}
