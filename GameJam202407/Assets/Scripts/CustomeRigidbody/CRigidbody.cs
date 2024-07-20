using System;
using UnityEngine;

public class CRigidbody : MonoBehaviour
{
    private Rigidbody attachedRigidbody;
    private Vector3 gravity;
    public void Initialize(SO_RigidbodyBehavior enteredBehavior, Rigidbody targetRigidbody,ref Action fixedUpdateEvent)
    {
        attachedRigidbody = targetRigidbody;
        attachedRigidbody.useGravity = false;
        attachedRigidbody.interpolation = RigidbodyInterpolation.None;
        gravity = enteredBehavior.Gravity;
        fixedUpdateEvent += ApplyCGravity;
    }
    public void Detach(ref Action fixedUpdateEvent) => fixedUpdateEvent -= ApplyCGravity;
    private void ApplyCGravity()
    {
        attachedRigidbody.AddForce(gravity, ForceMode.Acceleration);
    }
}
