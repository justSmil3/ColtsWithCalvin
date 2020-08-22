using Obi;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public enum PlayerChoise
{
    PlayerOne,
    PlayerTwo
}
public class SimpleCharControll : MonoBehaviour
{
    private Rigidbody rb;
    public float fallMultiplyer = 2.5f, lowJumpMultiplyer = 2f, jumpForce = 10, speed = 10, cameraRotationSpeed = 15f, maxAcceleration = 10f;

    SimpleCharImput controlls;
    public PlayerChoise p;

    Vector2 moveData = Vector2.zero;
    Vector2 cameraMoveData = Vector2.zero;

    bool onGround, inJump;

    private void Awake()
    {
        if (!TryGetComponent<Rigidbody>(out rb))
            Destroy(this);
        controlls = new SimpleCharImput();
        switch (p)
        {
            case PlayerChoise.PlayerOne:
                controlls.PlayerOne.Jump.performed += ctx => inJump = true;
                controlls.PlayerOne.Jump.canceled += ctx => inJump = false;
                controlls.PlayerOne.Jump.performed += ctx => rb.velocity += transform.up * jumpForce;
                controlls.PlayerOne.Move.performed += ctx => moveData = ctx.ReadValue<Vector2>();
                controlls.PlayerOne.Move.canceled += ctx => moveData = Vector2.zero; 
                controlls.PlayerOne.CameraMove.performed += ctx => cameraMoveData = ctx.ReadValue<Vector2>();
                controlls.PlayerOne.CameraMove.canceled += ctx => cameraMoveData = Vector2.zero;
                break;
            case PlayerChoise.PlayerTwo:
                controlls.PlayerTwo.Jump.performed += ctx => inJump = true;
                controlls.PlayerTwo.Jump.canceled += ctx => inJump = false;
                controlls.PlayerTwo.Jump.performed += ctx => rb.velocity += transform.up * jumpForce;
                controlls.PlayerTwo.Move.performed += ctx => moveData = ctx.ReadValue<Vector2>();
                controlls.PlayerTwo.Move.canceled += ctx => moveData = Vector2.zero;
                controlls.PlayerTwo.CameraMove.performed += ctx => cameraMoveData = ctx.ReadValue<Vector2>();
                controlls.PlayerTwo.CameraMove.canceled += ctx => cameraMoveData = Vector2.zero;
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
        CameraMove();
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
        rb.velocity += velo + jumpVelo;
    }

    private void Move()
    {
        Vector2 movementVelocity = moveData * speed * Time.deltaTime;
        Vector3 forwardMovement = transform.right * movementVelocity.x;
        Vector3 sideMovement = transform.forward * movementVelocity.y;
        Vector3 movement = forwardMovement + sideMovement;
        Debug.LogError(movement);
        if (rb.velocity.magnitude <= maxAcceleration)
            rb.velocity += movement;
    }

    void CameraMove()
    {
        Vector2 cameraMovement = (cameraMoveData * cameraRotationSpeed * Time.deltaTime).normalized;
        Vector3 eulerRotationCamera = new Vector3(-cameraMovement.y, 0f, 0f);
        this.transform.GetChild(0).gameObject.transform.eulerAngles += eulerRotationCamera;
        this.transform.eulerAngles += new Vector3(0f, cameraMovement.x, 0f);
    }
}
