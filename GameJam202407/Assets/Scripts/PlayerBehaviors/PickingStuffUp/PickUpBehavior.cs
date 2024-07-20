using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpBehavior : MonoBehaviour
{
    private Transform startingTransform;
    private TextMeshProUGUI UIReference;
    private InputAction pickUpAction;

    private RaycastHit hitObject;
    private bool isHoldingSomething = false;
    private Transform childPosition; //the position the child object will be bound to     
    private I_PickUp targetPickUpBehavior;

    private Transform bufferedGameobject;

    private Transform currentlyHoldGameobject;
    private I_PickUp currentlyHoldPickUp;


    private void PickUpCast(RaycastHit? hitObject)
    {
        if (hitObject != null)
        {
            targetPickUpBehavior = hitObject.Value.transform.GetComponentInParent<I_PickUp>(); //reference to target interact behavior
            if (targetPickUpBehavior != null && !isHoldingSomething)
            {
                UIReference.enabled = true;
                if (bufferedGameobject == null) bufferedGameobject = hitObject.Value.transform.GetComponentInParent<Transform>(); //reference to the parent transform 
            }
            else
            {
                UIReference.enabled = false;
                bufferedGameobject = null;
            }
        }
        else
        {
            bufferedGameobject = null;
            UIReference.enabled = false; //if the reference is null diable the 
            targetPickUpBehavior = null; //dereferencing the interact target
        }
    }
    public void BehaviorIsCalled(InputAction.CallbackContext context)
    {
        if (context.started) //player pressed the button 
        {
            if (isHoldingSomething) DetachPickUp();
            else if (targetPickUpBehavior != null) SetPickUp();
        }
    }
    private void SetPickUp()
    {
        isHoldingSomething = true;
        AttachObject();
        SubscribePickUp(); //subscribing the input action
    }
    private void DetachPickUp()
    {
        UnsubscribePickUp();
        isHoldingSomething = false;
        RemoveObject();
    }
    /// <summary>
    /// Used to instantiate the behavior
    /// </summary>
    public void Initialize(ref Action<RaycastHit?> raycastEvent, TextMeshProUGUI InteractionIndicator, Transform targetPosition, InputAction itemAction)
    {
        UIReference = InteractionIndicator;
        raycastEvent += PickUpCast;
        childPosition = targetPosition;
        pickUpAction = itemAction;
    }
    public void Detach(ref Action<RaycastHit?> raycastEvent)
    {
        raycastEvent -= PickUpCast;
        if(currentlyHoldGameobject) UnsubscribePickUp();
    }
    private void AttachObject()
    {
        currentlyHoldPickUp = targetPickUpBehavior; //setting the interface to use

        currentlyHoldGameobject = bufferedGameobject; //setting the gameobject that is currently hold
        currentlyHoldGameobject.transform.parent = childPosition; //setting the transform parent to the child spot
        currentlyHoldGameobject.transform.localPosition = Vector3.zero; //resetting the position of the child
        currentlyHoldGameobject.transform.rotation = Quaternion.identity; //resetting the rotation
    }
    private void RemoveObject()
    {
        currentlyHoldGameobject.transform.parent = null; //removing the parent of the transform 
        currentlyHoldGameobject = null; //removing the currently hold object
        currentlyHoldPickUp = null; //removing the interface to use
    }
    private void SubscribePickUp()
    {
        pickUpAction.started += currentlyHoldPickUp.PickUp;
        pickUpAction.performed += currentlyHoldPickUp.PickUp;
        pickUpAction.canceled += currentlyHoldPickUp.PickUp;
    }
    private void UnsubscribePickUp()
    {
        pickUpAction.started -= currentlyHoldPickUp.PickUp;
        pickUpAction.performed -= currentlyHoldPickUp.PickUp;
        pickUpAction.canceled -= currentlyHoldPickUp.PickUp;
    }
}
