using Obi;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public enum PlayerChoise
{
    PlayerOne,
    PlayerTwo
}
public class CharacterController : MonoBehaviour
{
    private Rigidbody rb;
    public float 
        fallMultiplyer = 2.5f, 
        lowJumpMultiplyer = 2f, 
        jumpForce = 10, 
        speed = 10, 
        cameraRotationSpeed = 15f, 
        maxAcceleration = 10f;

    PlayerControlls controlls;
    public PlayerChoise p;
    public Transform playerInputSpace;

    Vector2 moveData = Vector2.zero;
    Vector2 cameraMoveData = Vector2.zero;

    bool onGround, inJump;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        controlls = new PlayerControlls();
        switch (p)
        {
            case PlayerChoise.PlayerOne:
                controlls.PlayerOneControlls.Jump.performed += ctx => inJump = true;
                controlls.PlayerOneControlls.Jump.canceled += ctx => inJump = false;
                controlls.PlayerOneControlls.Jump.performed += ctx => rb.velocity += transform.up * jumpForce;
                controlls.PlayerOneControlls.Move.performed += ctx => moveData = ctx.ReadValue<Vector2>();
                controlls.PlayerOneControlls.Move.canceled += ctx => moveData = Vector2.zero;
                break;
            case PlayerChoise.PlayerTwo:
                controlls.PlayerTwoControlls.Jump.performed += ctx => inJump = true;
                controlls.PlayerTwoControlls.Jump.canceled += ctx => inJump = false;
                controlls.PlayerTwoControlls.Jump.performed += ctx => rb.velocity += transform.up * jumpForce;
                controlls.PlayerTwoControlls.Move.performed += ctx => moveData = ctx.ReadValue<Vector2>();
                controlls.PlayerTwoControlls.Move.canceled += ctx => moveData = Vector2.zero;
                break;
            default:
                break;
        }
    }

    private void OnEnable()
    {
        controlls.Enable();
    }

    private void OnDisable()
    {
        controlls.Disable();
    }

    private void FixedUpdate()
    {
        Jump();
        Move();
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        if (playerInputSpace == null) return;
        Vector3 lookdir = playerInputSpace.forward.normalized;
        lookdir.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(lookdir);
    }


    private void OnCollisionStay(Collision collision)
    {
        onGround = true;
    }

    private void Jump()
    {

        float fallMult = 0;
        Vector3 jumpVelo = Vector3.zero;

        if (inJump)
        {
            fallMult = fallMultiplyer;
            //jumpVelo = transform.up * jumpForce * Time.deltaTime * 10f;

        }
        else if (!onGround)
        {
            fallMult = lowJumpMultiplyer + fallMultiplyer;
        }
        else return;

        onGround = false;
        Vector3 velo = Vector3.down * fallMult * Time.deltaTime;

    }

    private void Move()
    {
        Vector2 movementVelocity = moveData * speed * Time.deltaTime;
        Vector3 forwardMovement = transform.right * movementVelocity.x;
        Vector3 sideMovement = transform.forward * movementVelocity.y;
        Vector3 movement = forwardMovement + sideMovement;
        if (rb.velocity.magnitude <= maxAcceleration && movement != Vector3.zero)
        {
            rb.velocity += movement;
        }
    }


}
