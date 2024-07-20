using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ButtonParent : MonoBehaviour
{
    private float bufferTime = 0;
    private bool enableBuffering = false;
    private bool currentlyPressed = false;
    private bool currentlyBuffering = false;

    /// <summary>
    /// When the c# event is invoked this function will be called.
    /// </summary>
    /// <param name="enteredContext"></param>
    public void BehaviorIsCalled(InputAction.CallbackContext enteredContext)
    {
        switch (enteredContext.phase)
        {
            case InputActionPhase.Started:
                //if the action is started the buttonPressed Coroutine starts
                StartButtonPressedCoroutine();
                break;
            case InputActionPhase.Performed:
                //if the input is finished and buffering is enabled the buffer coroutine is started else the coroutines are stopped.
                if (enableBuffering) StartBufferCoroutine();
                else KillCoroutines();
                break;
            case InputActionPhase.Disabled:
                //If the input is disabled for some reason all associated coroutine are stopped.
                KillCoroutines();
                break;
        }
    }
    /// <summary>
    /// The function checks if the entered number is greater equal 0 and then if the number is 0 set the buffer to
    /// flase and if greater than 0 sets the buffer time to the entered number and enables buffering.
    /// </summary>
    /// <param name="setBufferTime"></param>
    protected void SetBuffer(float setBufferTime = 0)
    {
        if(setBufferTime >= 0)
        {
            if (setBufferTime == 0) enableBuffering = false;
            else
            {
                bufferTime = setBufferTime;
                enableBuffering = true;
            }
        }
    }
    protected abstract bool BehaviorCheck();
    protected IEnumerator BufferInput()
    {
        float tempTime = 0; //time since the buffer started is now 0
        while (tempTime < bufferTime && currentlyBuffering) //check if the buffer is still running and the time is not longer than the buffer duration 
        {
            yield return new WaitForFixedUpdate(); //wait for next fixedupdate
            if (!BehaviorCheck()) tempTime += Time.fixedDeltaTime; //update the buffertime if check was unsuccesfull 
            else break; //If the input was successfull the loop is exited.
        }
        currentlyBuffering = false; //buffer is not running anymore
    }
    protected IEnumerator ButtonPressed()
    { 
        while(currentlyPressed)
        {
            if (!BehaviorCheck()) yield return new WaitForFixedUpdate(); //wait for the next opertunity the button press can be executed 
            else break; //if the button was executed
        }
        currentlyPressed = false; //button pressed = false
    }
    protected void KillCoroutines()
    {
        currentlyBuffering = false;
        currentlyPressed = false;
    }
    private void StartBufferCoroutine()
    {
        currentlyBuffering = true; //The buffer is now running 
        currentlyPressed = false; //the button is not pressed anymore
        StartCoroutine(BufferInput());
    }
    private void StartButtonPressedCoroutine()
    {
        currentlyPressed = true; //button is currently pressed
        currentlyBuffering = false; //button is no longer buffering
        StartCoroutine(ButtonPressed());
    }
}