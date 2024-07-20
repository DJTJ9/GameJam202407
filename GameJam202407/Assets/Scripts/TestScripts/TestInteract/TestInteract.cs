using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteract : MonoBehaviour, I_Interactable
{
    [SerializeField] private Rigidbody attachedRigidbody;
    public void Interact()
    {
        attachedRigidbody.AddForce(new Vector3(0,10,0), ForceMode.Impulse);
    }
}
