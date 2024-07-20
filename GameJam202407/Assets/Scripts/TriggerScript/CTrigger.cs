using System;
using UnityEngine;

public class CTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask collisionMask;
    public event Action<bool> CollisionStateChange;
    private void OnTriggerEnter(Collider other)
    {
        if ((collisionMask.value & (1 << other.gameObject.layer)) > 0) CollisionStateChange.Invoke(true);
    }
    private void OnTriggerExit(Collider other) 
    {
        if((collisionMask.value & (1 << other.gameObject.layer)) > 0) CollisionStateChange.Invoke(false);
    }
}
