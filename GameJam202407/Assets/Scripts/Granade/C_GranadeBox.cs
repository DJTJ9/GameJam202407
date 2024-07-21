using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class C_GranadeBox : MonoBehaviour, I_PickUp
{
    [SerializeField] private GameObject GranadePref;
    public float SpawnOffset = 0.5f;
    public int MaxGranades = 10;
    int CurrentGranades;

    void Start()
    { 
        CurrentGranades = MaxGranades;    
    }

    public void PickUp(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (CurrentGranades == 0)
            {
                return;
            }
            else
            {
                CurrentGranades--;
            }

            SpawnGranade(); 
        }
    }

    void SpawnGranade()
    {
        Vector3 SpawnPos = transform.position;
        SpawnPos += SpawnOffset * transform.forward;
        Instantiate(GranadePref,SpawnPos,Quaternion.Euler(transform.eulerAngles));
    }
}
