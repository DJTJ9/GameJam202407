using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public interface I_PickUp 
{
    public abstract void PickUp(InputAction.CallbackContext context);
    //public abstract SO_PickableVariables ReturnPickableVariables();
}