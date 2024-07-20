using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPickUp : MonoBehaviour, I_PickUp
{
    public void PickUp(InputAction.CallbackContext context)
    {
        switch(context.phase)
        {
            case InputActionPhase.Started:
                Debug.Log("Started execution");
                break;
            case InputActionPhase.Performed:
                Debug.Log("Performced");
                break;
            case InputActionPhase.Canceled:
                Debug.Log("Cancled");
                break;
        }
    }
}
