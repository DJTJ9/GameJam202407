using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class C_RocketLauncher : MonoBehaviour, I_PickUp
{
    [SerializeField] private GameObject ProjectilePref;
    public float SpawnOffset = 0.5f;

    public void PickUp(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SpawnProjectile();
        }
    }

    void SpawnProjectile()
    {
        Vector3 SpawnPos = transform.position;
        SpawnPos += SpawnOffset * transform.forward;
        Instantiate(ProjectilePref, SpawnPos, Quaternion.Euler(transform.eulerAngles));
    }

}
