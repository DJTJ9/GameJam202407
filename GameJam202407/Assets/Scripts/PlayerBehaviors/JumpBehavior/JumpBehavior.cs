using System.Collections;
using UnityEngine;

public class JumpBehavior : ButtonParent
{
    private Rigidbody attachedTarget;
    private CTrigger triggerConstraint;
    private bool isGrounded;
    //double jump variables
    private bool enableDoubleJump = false;
    private bool doubleJumpAvailable = true; 
    private float doubleJumpStrength;
    private float maxDoubleJumpSpeed;
    //cd variables
    private bool isOnCD = false;
    private float passedTime = 0;
    private float jumpCD;
    //normal jump variables
    private bool jumpAvailable = true;
    private float maxNormalJumpSpeed;
    private float jumpStrength;
    //temp variables
    private float bufferedYSpeed;
    private Vector3 jumpForce = new Vector3();
    protected override bool BehaviorCheck()
    {
        //triggerConstraint.CurrentlyColliding && jumpAvailable
        if (isGrounded) //is a jump available and the trigger colliding
        {
            if (jumpAvailable && !isOnCD)
            {
                ExecuteJump();
                return true;
            }
        }
        else if (enableDoubleJump)
        {
            if (doubleJumpAvailable && !isOnCD)
            {
                ExecuteDoubleJump();
                return true;
            }
        }
        return false;
    }
    public void Initialise(Rigidbody targetObject, CTrigger attachedTrigger, SO_JumpBehavior enteredbehavior)
    {
        if (attachedTrigger != null) triggerConstraint = attachedTrigger; //ctrigger that will be used for collision
        SetBuffer(enteredbehavior.jumpBufferTime); //buffertime for the base class
        attachedTarget = targetObject; //rigidbody that will be used as a force target

        triggerConstraint.CollisionStateChange += SetCollisionState;

        jumpCD = enteredbehavior.jumpCooldown;
        jumpStrength = enteredbehavior.JumpStrength;
        enableDoubleJump = enteredbehavior.EnableDoubleJump;
        maxDoubleJumpSpeed = enteredbehavior.MaxDoubleJumpSpeed;
        doubleJumpStrength = enteredbehavior.DoubleJumpStrength;
    }
    private void OnDisable()
    {
        triggerConstraint.CollisionStateChange -= SetCollisionState;
    }
    private IEnumerator JumpCD()
    {
        passedTime = 0;
        while (passedTime < jumpCD)
        {
            yield return new WaitForFixedUpdate();
            passedTime += Time.deltaTime;
        }
        isOnCD = false;
    }
    private void NormalJumpCalc()
    {
        //what is the current upward velocity? 
        jumpForce = new Vector3(attachedTarget.velocity.x, jumpStrength, attachedTarget.velocity.z);
        bufferedYSpeed = attachedTarget.velocity.y;

        if (bufferedYSpeed <= 0) attachedTarget.velocity = jumpForce;
        else
        {
            jumpForce.y += bufferedYSpeed; //adding the jump strength to the current upwards velocity
            if (jumpForce.y > maxNormalJumpSpeed) attachedTarget.velocity = new Vector3(jumpForce.x, attachedTarget.velocity.y, jumpForce.z); //setting to maxspeed if the velocitiy is higher 
            else attachedTarget.velocity = jumpForce; //setting the velocity to the tempSpeed if it is in range 
        }
    }
    private void DoubleJumpCalc()
    {
        //what is the current upward velocity? 
        jumpForce = new Vector3(attachedTarget.velocity.x, doubleJumpStrength, attachedTarget.velocity.z);
        bufferedYSpeed = attachedTarget.velocity.y;

        if (bufferedYSpeed <= 0) attachedTarget.velocity = jumpForce;
        else
        {
            jumpForce.y += bufferedYSpeed; //adding the jump strength to the current upwards velocity
            if (jumpForce.y > maxDoubleJumpSpeed) attachedTarget.velocity = new Vector3(jumpForce.x, attachedTarget.velocity.y, jumpForce.z); //setting to maxspeed if the velocitiy is higher
            else attachedTarget.velocity = jumpForce; //setting the velocity to the jumpforce if it is in range 
        }
    }
    private void ResetJumpVariables()
    {
        jumpAvailable = true;
        doubleJumpAvailable = true;
    }
    private void ExecuteJump()
    {
        jumpAvailable = false; //remove doublejump
        isOnCD = true;
        NormalJumpCalc();
        StartCoroutine(JumpCD());
    }
    private void ExecuteDoubleJump()
    {
        doubleJumpAvailable = false;
        isOnCD = true;
        StartCoroutine(JumpCD());
        DoubleJumpCalc();
    }
    private void SetCollisionState(bool enteredValue) 
    {
        if (enteredValue)
        { 
            ResetJumpVariables();
            isGrounded = true;
        }
        else isGrounded = false;
    }
}
