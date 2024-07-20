using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class C_RocketLauncher : MonoBehaviour, I_PickUp
{
    public void PickUp(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SpawnProjectile();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void SpawnProjectile()
    {


    }

}
