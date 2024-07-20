using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InteractBehavior : MonoBehaviour
{
    private Transform startingTransform;
    private TextMeshProUGUI UIReference;

    private float interactDistance;
    private RaycastHit hitObject;
    private I_Interactable targetInteractBehavior;
    public void BehaviorIsCalled(InputAction.CallbackContext enteredContext)
    {
        if(enteredContext.started && targetInteractBehavior != null) targetInteractBehavior.Interact();
    }
    private void InteractCast(RaycastHit? hitObject)
    {
        if (hitObject != null)
        {
            targetInteractBehavior = hitObject.Value.transform.GetComponentInParent<I_Interactable>(); //reference to target interact behavior
            if (targetInteractBehavior != null) UIReference.enabled = true;
            else UIReference.enabled = false;
        }
        else
        { 
            UIReference.enabled = false; //if the reference is null diable the 
            targetInteractBehavior = null; //dereferencing the interact target
        } 
    }
    public void Initialize(ref Action<RaycastHit?> raycastEvent, TextMeshProUGUI InteractionIndicator)
    {
        UIReference = InteractionIndicator;
        raycastEvent += InteractCast;
    }
    public void Detach(ref Action<RaycastHit?> raycastEvent) => raycastEvent -= InteractCast;
}