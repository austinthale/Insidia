using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8.0f;
    public float turnSpeed = 30.0f;
    [Range(0, 50)]
    public float gravity = 10f;
    public float jumpHeight = 10f;
    [SerializeField]
    private float _yVelocity = 0f;
    private CharacterController _charController;

    public string moveHorz = "Horizontal";
    public string moveVert = "Vertical";
    public string jump = "Jump";

    // Use this for initialization
    void Start()
    {
        _charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        var y = Input.GetAxis(moveHorz) * Time.deltaTime * turnSpeed;
        var z = Input.GetAxis(moveVert) * Time.deltaTime * moveSpeed;
        if (Input.GetButtonDown(jump) && IsTouchingGround())
            _yVelocity += Mathf.Sqrt(2 * gravity * jumpHeight);


        // Rotates/turns the player
        transform.Rotate(0, y, 0);

        Vector3 velocity = Vector3.zero; //vector3(0,0,0)
        velocity.z = z;

        // Take accel of grav per second and add to yVelocity
        _yVelocity -= gravity * Time.deltaTime;
        // Assign speed to velocity.y part of the vector.
        velocity.y = _yVelocity * Time.deltaTime;
        velocity = transform.rotation * velocity;

        // Moves character via char controller
        _charController.Move(velocity);

        if (IsTouchingGround())
            _yVelocity = 0;

    }

    private bool IsTouchingGround()
    {
        return (_charController.collisionFlags & CollisionFlags.Below) != 0;
    } 
}
