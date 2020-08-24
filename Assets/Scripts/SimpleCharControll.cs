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
public class SimpleCharControll : MonoBehaviour
{
    private Rigidbody rb;
    private ObiRigidbody orb; 
    public float fallMultiplyer = 2.5f, lowJumpMultiplyer = 2f, jumpForce = 10, speed = 10, cameraRotationSpeed = 15f, maxAcceleration = 10f;
    public GameObject obiRopeAttachement;
    public GameObject obiRope;
    public ObiRopeBlueprint ropeBlueprint;
    public ObiCollisionMaterial obiCollisionMaterial;
    public Material ropeMaterial;
    public ObiRopeSection section;

    SimpleCharImput controlls;
    public PlayerChoise p;

    Vector2 moveData = Vector2.zero;
    Vector2 cameraMoveData = Vector2.zero;

    bool onGround, inJump;

    private void Awake()
    {
        rb = obiRopeAttachement.GetComponent<Rigidbody>();
        orb = obiRopeAttachement.GetComponent<ObiRigidbody>();
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
                controlls.PlayerOne.ChangeRope.started += ctx => RopeChange();
                // handle kinetics
                controlls.PlayerOne.Jump.performed += ctx => orb.kinematicForParticles = true;
                controlls.PlayerOne.Move.performed += ctx => orb.kinematicForParticles = true;
                controlls.PlayerOne.CameraMove.performed += ctx => orb.kinematicForParticles = true; 
                controlls.PlayerOne.Jump.canceled += ctx => orb.kinematicForParticles = false;
                controlls.PlayerOne.Move.canceled += ctx => orb.kinematicForParticles = false;
                controlls.PlayerOne.CameraMove.canceled += ctx => orb.kinematicForParticles = false;
                break;
            case PlayerChoise.PlayerTwo:
                controlls.PlayerTwo.Jump.performed += ctx => inJump = true;
                controlls.PlayerTwo.Jump.canceled += ctx => inJump = false;
                controlls.PlayerTwo.Jump.performed += ctx => rb.velocity += transform.up * jumpForce;
                controlls.PlayerTwo.Move.performed += ctx => moveData = ctx.ReadValue<Vector2>();
                controlls.PlayerTwo.Move.canceled += ctx => moveData = Vector2.zero;
                controlls.PlayerTwo.CameraMove.performed += ctx => cameraMoveData = ctx.ReadValue<Vector2>();
                controlls.PlayerTwo.CameraMove.canceled += ctx => cameraMoveData = Vector2.zero;
                controlls.PlayerTwo.ChangeRope.started += ctx => RopeChange();
                // handle kinetics
                controlls.PlayerTwo.Jump.performed += ctx => orb.kinematicForParticles = true;
                controlls.PlayerTwo.Move.performed += ctx => orb.kinematicForParticles = true;
                controlls.PlayerTwo.CameraMove.performed += ctx => orb.kinematicForParticles = true;
                controlls.PlayerTwo.Jump.canceled += ctx => orb.kinematicForParticles = false;
                controlls.PlayerTwo.Move.canceled += ctx => orb.kinematicForParticles = false;
                controlls.PlayerTwo.CameraMove.canceled += ctx => orb.kinematicForParticles = false;
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
        RopeMove();
    }

    void RopeMove()
    {
        this.transform.position = obiRopeAttachement.transform.position;
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
        if (rb.velocity.magnitude <= maxAcceleration && movement != Vector3.zero)
        {
            rb.velocity += movement;
        }
    }

    void CameraMove()
    {
        Vector2 cameraMovement = (cameraMoveData * cameraRotationSpeed * Time.deltaTime).normalized;
        Vector3 eulerRotationCamera = new Vector3(-cameraMovement.y, 0f, 0f);
        this.transform.GetChild(0).gameObject.transform.eulerAngles += eulerRotationCamera;
        this.transform.eulerAngles += new Vector3(0f, cameraMovement.x, 0f);
    }

    void RopeChange()
    {
        if(obiRope == null)
        {
            GameObject temp = new GameObject("ObiRope");
            ObiRope rope = temp.AddComponent<ObiRope>();
            ObiRopeExtrudedRenderer extruder = temp.AddComponent<ObiRopeExtrudedRenderer>();


            GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");

            //ropeBlueprint = new ObiRopeBlueprint();
            //ObiParticleGroup firstParticleGroup = ropeBlueprint.AppendNewParticleGroup("start");
            //ObiParticleGroup secondParticleGroup = ropeBlueprint.AppendNewParticleGroup("end");
            //ropeBlueprint.positions[0] = Vector3.zero;
            //ropeBlueprint.positions[1] = Vector3.zero;



            extruder.section = section;
            extruder.thicknessScale = .8f;
            MeshRenderer rend = temp.GetComponent<MeshRenderer>();
            rend.material = ropeMaterial;
            rope.collisionMaterial = obiCollisionMaterial;
            rope.ropeBlueprint = ropeBlueprint;
            temp.layer = 8;

            Vector3 halfDist = (Players[1].transform.position - Players[0].transform.position) / 2;
            temp.transform.position = Players[0].transform.position + halfDist;


            GameObject dad = GameObject.FindGameObjectWithTag("ObiRopeSolver");
            temp.transform.SetParent(dad.transform);

            obiRope = temp;
        }
        else
        {
            Destroy(obiRope);
            obiRope = null;
        }
    }
}
