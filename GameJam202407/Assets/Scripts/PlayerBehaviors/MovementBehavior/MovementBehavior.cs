using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementBehavior : MonoBehaviour
{
    private Rigidbody attachedRigidbody;
    private CTrigger attachedTrigger;
    
    //ground variables
    private float groundNorthSpeed; 
    private float groundSouthSpeed; 
    private float groundEastSpeed;
    private float groundWestSpeed;
    //

    private float zAxisLerp;
    private float xAxisLerp;

    private float sqrMaxSpeed;
    
    //air variables
    private float airNorthForce;
    private float airEastForce;
    private float airSouthForce;
    private float airWestForce;

    private float sqrMaxAirSpeed;
    
    //sprint ground variabels
    private float sprintNorthSpeed;
    private float sprintEastSpeed;
    private float sprintSouthSpeed;
    private float sprintWestSpeed;

    private float sqrMaxSprintSpeed;
    
    private float zAxisSprintLerp;
    private float xAxisSprintLerp;

    //calculation variables
    private bool isGrounded;
    private bool isSprinting = false;
    private bool enteredGround = false;
    private bool groundSprinting = false;
    private float timeBeyondMaxSpeed = 0;
    private Vector3 movement = new Vector3();
    private Vector2 startingVelocity = new Vector2();
    private Vector2 savedVelocity = new Vector2();
    private Vector2 velocityDiff = new Vector2();
    private Vector2 bufferedInput = new Vector2();
    private void UpdateInputValue()
    {
        if (isGrounded)
        {
            if (!isSprinting) GroundCalc();
            else SprintCalc();
        }
        else AirCalc();
    }
    private void OnDisable()
    {
        attachedTrigger.CollisionStateChange -= IsGrounded;
    }
    private void AirCalc()
    {
        enteredGround = false;
        groundSprinting = false;
        //normalising movementdirection
        bufferedInput.Normalize();
        //saving current velocity
        SaveCurrentVelocity();

        //calculation the forward and backwards speed
        movement.z =
        Mathf.Max(0, bufferedInput.y * airNorthForce) + //returns eithere 0 or maxForwardsSpeed
        Mathf.Min(0, bufferedInput.y * airSouthForce); //returns eithere 0 ir -maxBackwardsSpeed
        movement.x =    
        Mathf.Max(0, bufferedInput.x * airEastForce) + //returns eithere 0 or maxForwardsSpeeds
        Mathf.Min(0, bufferedInput.x * airWestForce); //returns eithere 0 ir -maxBackwardsSpeed

        if(savedVelocity.sqrMagnitude < sqrMaxAirSpeed)
        {
            movement = transform.right * movement.x + transform.forward * movement.z;
            attachedRigidbody.AddForce(movement);
        }
    }
    private void GroundCalc()
    {
        groundSprinting = false;
        bufferedInput.Normalize(); //normalise the input of the player

        enteredGround = SetStartingVelocity(enteredGround);
        //saving current velocity
        SaveCurrentVelocity();

        //calculation the forward and backwards speed
        movement.z =
        Mathf.Max(0, bufferedInput.y * groundNorthSpeed) + //returns eithere 0 or maxForwardsSpeed
        Mathf.Min(0, bufferedInput.y * groundSouthSpeed); //returns eithere 0 ir -maxBackwardsSpeed
        movement.x =
        Mathf.Max(0, bufferedInput.x * groundEastSpeed) + //returns eithere 0 or maxForwardsSpeed
        Mathf.Min(0, bufferedInput.x * groundWestSpeed); //returns eithere 0 ir -maxBackwardsSpeed

        //calculating the movement vector with the forward and sidwards movement
        movement = transform.forward * movement.z + transform.right * movement.x;

        if (savedVelocity.sqrMagnitude < sqrMaxSpeed)
        {
            //lerping the movement
            movement.x = Mathf.Lerp(savedVelocity.x, movement.x, zAxisLerp);
            movement.z = Mathf.Lerp(savedVelocity.y, movement.z, xAxisLerp);
            //applying the movement 
            attachedRigidbody.velocity = new Vector3(movement.x, attachedRigidbody.velocity.y, movement.z);
            timeBeyondMaxSpeed = 0;
        }
        else
        {
            //splitting the velocity of the player in the normal velocity and the velocity the player is above max speed
            velocityDiff = startingVelocity - new Vector2(movement.x, movement.z);
            //lerp the additionaly speed towards 0 
            velocityDiff.y = Mathf.Lerp(velocityDiff.y, 0, timeBeyondMaxSpeed);
            velocityDiff.x = Mathf.Lerp(velocityDiff.x, 0, timeBeyondMaxSpeed);
            //add time beyond max speed
            timeBeyondMaxSpeed += Time.fixedDeltaTime;
            //applying the velocinty to the movement 
            //attachedRigidbody.velocity = new Vector3(velocityDiff.x + movement.x, attachedRigidbody.velocity.y,velocityDiff.y + movement.z);
            attachedRigidbody.velocity = new Vector3(velocityDiff.x + movement.x, attachedRigidbody.velocity.y, velocityDiff.y + movement.z);
        }
    }
    private void SprintCalc()
    {
        enteredGround = false;
        bufferedInput.Normalize(); //normalise the input of the player

        groundSprinting = SetStartingVelocity(groundSprinting);

        //saving current velocity
        SaveCurrentVelocity();

        //calculation the forward and backwards speed
        movement.z = AxisMovement(bufferedInput.y, sprintNorthSpeed, sprintSouthSpeed);
        movement.x = AxisMovement(bufferedInput.x, sprintEastSpeed, sprintWestSpeed);

        //calculating the movement vector with the forward and backwards movement
        movement = transform.forward * movement.z + transform.right * movement.x;

        if (savedVelocity.sqrMagnitude < sqrMaxSprintSpeed)
        {
            //lerping the movement
            movement.x = Mathf.Lerp(savedVelocity.x, movement.x, zAxisSprintLerp);
            movement.z = Mathf.Lerp(savedVelocity.y, movement.z, xAxisSprintLerp);
            //applying the movement 
            attachedRigidbody.velocity = new Vector3(movement.x, attachedRigidbody.velocity.y, movement.z);
            timeBeyondMaxSpeed = 0;
        }
        else
        {
            //splitting the velocity of the player in the normal velocity and the velocity the player is above max speed
            velocityDiff = startingVelocity - new Vector2(movement.x, movement.z);
            //lerp the additionaly speed towards 0 
            velocityDiff.y = Mathf.Lerp(velocityDiff.y, 0, timeBeyondMaxSpeed);
            velocityDiff.x = Mathf.Lerp(velocityDiff.x, 0, timeBeyondMaxSpeed);
            //add time beyond max speed
            timeBeyondMaxSpeed += Time.fixedDeltaTime;
            //applying the velocinty to the movement 
            attachedRigidbody.velocity = new Vector3(velocityDiff.x + movement.x, attachedRigidbody.velocity.y, velocityDiff.y + movement.z);
        }
    }
    private bool SetStartingVelocity(bool enteredGroundState)
    {
        if (!enteredGroundState) //when the 
        {
            //save the velocity the player touched the ground with
            startingVelocity.x = attachedRigidbody.velocity.x;
            startingVelocity.y = attachedRigidbody.velocity.z;
            return true;
        }
        else return false;
    }
    private void SaveCurrentVelocity()
    {
        savedVelocity.x = attachedRigidbody.velocity.x; //sideways velocity
        savedVelocity.y = attachedRigidbody.velocity.z; //forward velocity
    }
    private float AxisMovement(float inputValue, float positiveDirection, float negativeDirection)
    {
       float tempfloat =  
            Mathf.Max(0, inputValue * positiveDirection) + //returns eithere 0 or maxForwardsSpeed
            Mathf.Min(0, inputValue * negativeDirection); //returns eithere 0 ir -maxBackwardsSpeed
        return tempfloat;
    }
    public void Initialize(Rigidbody targetObject,CTrigger groundTrigger, SO_MovementBehavior enteredBehavior, ref Action fixedUpdateEvent)
    {
        attachedRigidbody = targetObject;
        attachedTrigger = groundTrigger;
        attachedTrigger.CollisionStateChange += IsGrounded;

        //ground variables
        groundNorthSpeed = enteredBehavior.GroundNorthSpeed;
        groundEastSpeed = enteredBehavior.GroundEastSpeed;
        groundSouthSpeed = enteredBehavior.GroundSouthSpeed;
        groundWestSpeed = enteredBehavior.GroundWestSpeed;

        zAxisLerp = enteredBehavior.NorthSouthLerp;
        xAxisLerp = enteredBehavior.EastWestLerp;

        sqrMaxSpeed = enteredBehavior.MaxGroundSpeed * enteredBehavior.MaxGroundSpeed;
        //air variables
        airNorthForce = enteredBehavior.AirNorthForce;
        airEastForce = enteredBehavior.AirEastForce;
        airSouthForce = enteredBehavior.AirSouthForce;
        airWestForce = enteredBehavior.AirWestForce;

        sqrMaxAirSpeed = enteredBehavior.MaxAirSpeeds * enteredBehavior.MaxAirSpeeds;

        fixedUpdateEvent += UpdateInputValue;
    }
    public void InitializeSprinting(SO_MovementBehavior enteredBehavior)
    {
        sprintNorthSpeed = enteredBehavior.SprintNorthSpeed;
        sprintEastSpeed = enteredBehavior.SprintEastSpeed;
        sprintSouthSpeed = enteredBehavior.SprintSouthSpeed;
        sprintWestSpeed = enteredBehavior.SprintWestSpeed;

        sqrMaxSprintSpeed = enteredBehavior.MaxSprintingSpeed * enteredBehavior.MaxSprintingSpeed;

        zAxisSprintLerp = enteredBehavior.NorthSouthSprintLerp;
        xAxisSprintLerp = enteredBehavior.WestEastSprintLerp;
    }
    public void BehaviorCheck(InputAction.CallbackContext enteredContext)
    {
        switch (enteredContext.phase)
        {
            case InputActionPhase.Started:
                {
                    bufferedInput = enteredContext.ReadValue<Vector2>();
                    break;
                }
            case InputActionPhase.Performed:
                {
                    bufferedInput = enteredContext.ReadValue<Vector2>();
                    break;
                }
            case InputActionPhase.Canceled:
                {
                    bufferedInput = Vector2.zero;
                    break;
                }
        }
    }
    public void DetachMovement(ref Action fixedUpdateEvent) => fixedUpdateEvent -= UpdateInputValue;
    public void SprintingBehavior(InputAction.CallbackContext enteredContext)
    {
        switch (enteredContext.phase)
        {
            case InputActionPhase.Started:
                {
                    isSprinting = true;
                    break;
                }
            case InputActionPhase.Canceled:
                {
                    isSprinting = false;
                    break;
                }
        }
    }
    private void IsGrounded(bool enteredValue) => isGrounded = enteredValue;
}