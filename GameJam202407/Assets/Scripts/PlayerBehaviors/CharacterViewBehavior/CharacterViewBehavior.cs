using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterViewBehavior : MonoBehaviour
{
    private Transform targetCamera;
    private Transform bodyTransform;

    private bool invertXAxis = false;
    private float sensetivity;
    private float lerpValue;
    private float lowerClamp;
    private float upperClamp;

    private float tempAxisValue;
    private float tempLerpValue;
    private Vector2 bufferedInput = new Vector2();
    private Vector3 targetRotation = Vector3.zero;
    public void MouseMoment(InputAction.CallbackContext enterdContext)
    {
        BufferInput(enterdContext.ReadValue<Vector2>());
    }
    private void ExecuteInput()
    {
        if (bufferedInput.sqrMagnitude > 0)
        {
            tempLerpValue = Time.deltaTime * lerpValue;
            //X axis
            tempAxisValue = invertXAxis ? Mathf.Lerp(0, bufferedInput.y, tempLerpValue) : -Mathf.Lerp(0, bufferedInput.y, tempLerpValue);
            bufferedInput.y =- tempAxisValue; //remove value from buffer
            targetRotation.x = tempAxisValue; //set rotation to value

            //Y axis
            tempAxisValue = Mathf.Lerp(0, bufferedInput.x, tempLerpValue);
            bufferedInput.x =- tempAxisValue; //remove value from buffer
            targetRotation.y = tempAxisValue; //set rotation to value

            //adding the current rotation to the target rotation
            targetRotation.x += targetCamera.rotation.eulerAngles.x;
            targetRotation.y += targetCamera.rotation.eulerAngles.y;
            targetRotation.z = 0;

            //Shifting targetRotation.x to be within -180 to 180 degrees range 
            if (targetRotation.x > 180) targetRotation.x -= 360;

            // Clamping the X rotation between -80 and 80 degrees
            targetRotation.x = Mathf.Clamp(targetRotation.x, lowerClamp, upperClamp);

            //setting parent and camera child rotation
            bodyTransform.rotation = Quaternion.Euler(0 ,targetRotation.y, 0);
            targetCamera.rotation = Quaternion.Euler(targetRotation);
        }
    }
    private void BufferInput(Vector2 mouseDelta)
    {
        bufferedInput.x += mouseDelta.x * sensetivity;
        bufferedInput.y += mouseDelta.y * sensetivity;
    }
    public void Initialize(Transform cameraTransform,Transform targetBodyTransform, SO_ViewVariables enteredBehavior, ref Action updateEvent)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        targetCamera = cameraTransform;
        bodyTransform =  targetBodyTransform;

        lerpValue = enteredBehavior.LerpValue;
        invertXAxis = enteredBehavior.InvertLookDirection;
        sensetivity = enteredBehavior.Sensetivity;
        lowerClamp = enteredBehavior.LowerdViewClamp;
        upperClamp = enteredBehavior.UpperdViewClamp;

        updateEvent += ExecuteInput;
    }
    public void Detach(ref Action updateEvent) => updateEvent -= ExecuteInput;
}
