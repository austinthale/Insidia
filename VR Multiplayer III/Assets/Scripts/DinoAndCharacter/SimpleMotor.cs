using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that serves as an incomplete example of how to program movement.
/// To use, slap on any object that also has a CharacterInput on it.
/// <para>Created by Christian Clark</para>
/// </summary>
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CharacterInput))]
public class SimpleMotor : MonoBehaviour {

    //Movement control variables.
    public float speed = 5f;
    public float turnSpeed = 720f;
    public float jumpStrength = 10f;
    public float yVelocity = 0f;
    public float gravity = 10f;
    public bool grounded = false;
    public float terminalVelocity = -20;

    //Private caching variables.
    private CharacterController _characterController;
    private CharacterInput _characterInput;


    //Cache references to the CharacterController component and the gameCharacter script.
    public void Awake()
    {
        //Due to the [RequireComponent] attributes on the class itself, it can be garenteed that this code will work (as long as it's set up through Unity's editor).
        _characterController = GetComponent<CharacterController>();
        _characterInput = GetComponent<CharacterInput>();
    }

    //Runs after Awake, but before Start.
    public void OnEnable()
    {
        //Subscribe to the Jump input's delegate
        _characterInput.Jump.OnChange += OnJump;
    }

    public void OnDisable()
    {
        //Make sure to unsubscribe or else you can cause the program to crash.
        _characterInput.Jump.OnChange -= OnJump;
    }

    //The function subscribed to the OnMovementUpdate of the gameCharacter.
    //It is given both the gameCharacter that sent the event (just in case you don't already have this info)
    //and the amount of time that has passed since the last call of the event (works just like Time.deltaTime, but only for this event).
    private void FixedUpdate()
    {
        // Fetch the input values from the game character //
        
        //What is our current move input?
        Vector3 moveInput = new Vector3(_characterInput.Move.Value.x, 0, _characterInput.Move.Value.y);
        if (moveInput.magnitude > 1f)
            moveInput.Normalize();

        // Apply the inputs //

        //Calculate the velocity we need to be moving at using the move input.
        Vector3 velocity = moveInput * speed;

        //Rotate to face the input.
        if (moveInput != Vector3.zero)
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.LookRotation(moveInput, transform.up), turnSpeed * Time.fixedDeltaTime);

        // Calucate physics //

        //Calcuate y velocity.
        //First apply gravity acceleration. (Which is speed per second so use deltaTime here.)
        yVelocity -= gravity * Time.fixedDeltaTime;
        //Make sure we don't fall too fast, both for gameplay reasons (staying in control of the character as it falls)
        //and technical reasons (not clipping through the ground)
        if (yVelocity < terminalVelocity)
            yVelocity = terminalVelocity;
        //Apply the y velocity to the velocity vector.
        velocity.y = yVelocity;

        // Apply physics //

        //Move according to our velocity (which is in speed per second so use deltaTime).
        _characterController.Move(velocity * Time.fixedDeltaTime);

        // Resolve movement collisions //

        //Check to see if we collided with the ground.
        if ((_characterController.collisionFlags & CollisionFlags.Below) != 0)
        {
            //If we collided with the ground, then we're grounded.
            grounded = true;
            //Also set y velocity to zero.
            yVelocity = 0f;
        }
        else
            //otherwise, we're not grounded
            grounded = false;
    }

    //This function is subscribed to gameCharacter.Input.Jump.OnChange, meaning that it is called every time
    //the Jump input changes from off to on or on to off (is pressed or released).
    //This function exists because otherwise the input of the jump button being pressed could be missed (making for laggy controls)
    //and because if this was being checked in Move() the player could jump repeatedly by holding the button down.
    private void OnJump(CharacterInput charInput, bool input)
    {
        //Only jump if the Jump input changed from off to on (was pressed) and we're on the ground.
        if (input && grounded)
        {
            //This y velocity change will be applied in the next movement update.
            yVelocity += jumpStrength;
            //Make sure we're not grounded anymore (both to prevent doing multiple jumps inbetween movement updates)
            //and to let other scripts we're no longer on the ground.
            grounded = false;
        }
    }
}
