using Obi;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[System.Serializable]
public enum PlayerChoise
{
    PlayerOne,
    PlayerTwo
}
public class CharacterController : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private Transform m_playerInputSpace;

    [SerializeField]
    private float
        fallMultiplyer = 2.5f,
        lowJumpMultiplyer = 2f,
        jumpForce = 10,
        m_maxSpeed = 20,
        cameraRotationSpeed = 15f;

    [SerializeField, Range(0, 50)]
    private float
        m_maxAcceleration    = 10.00f,
        m_maxAirAcceleration = 01.00f;

    [SerializeField, Range(0f, 90f)]
    private float 
        m_maxGroundAngle = 25f,
        m_maxStairsAngle = 50f;

    [SerializeField, Range(0f, 10f)]
    private float jumpHeight = 2f;

    [SerializeField, Range(0f, 100f)]
    private float m_maxSnapSpeed = 100f;

    [SerializeField, Min(0f)]
    private float m_probeDistance = 1f;

    [SerializeField]
    private LayerMask 
        m_probeMask = -1,
        m_stairMask = -1;

    PlayerControlls controlls;
    public PlayerChoise p;

    Vector2 moveData = Vector2.zero;
    Vector2 cameraMoveData = Vector2.zero;

    int
        m_groundContactCount     = 0,
        m_steepContactCount      = 0,
        m_stepsSinceLastGrounded = 0,
        m_stepsSinceLastJumped   = 0;

    bool 
        m_isJumpPressed;

    bool m_isOnGround => m_groundContactCount > 0;
    bool m_isOnSteep  => m_steepContactCount  > 0;  

    private Vector3
        m_velocity,
        m_contactNormal,
        m_steepNormal;


    private float
        m_minGroundDotProduct,
        m_minStairsDotProduct;

    private void OnValidate()
    {
        m_minGroundDotProduct = Mathf.Cos(m_maxGroundAngle * Mathf.Deg2Rad);
        m_minStairsDotProduct = Mathf.Cos(m_maxStairsAngle * Mathf.Deg2Rad);
    }

    private void Awake()
    {
        OnValidate();
        rb = this.GetComponent<Rigidbody>();
        controlls = new PlayerControlls();
        switch (p)
        {
            case PlayerChoise.PlayerOne:
                controlls.PlayerOneControlls.Jump.performed += ctx => m_isJumpPressed |= true;
                controlls.PlayerOneControlls.Move.performed += ctx => moveData = ctx.ReadValue<Vector2>();
                controlls.PlayerOneControlls.Move.canceled  += ctx => moveData = Vector2.zero;
                break;
            case PlayerChoise.PlayerTwo:
                controlls.PlayerTwoControlls.Jump.performed += ctx => m_isJumpPressed |= true;
                controlls.PlayerTwoControlls.Move.performed += ctx => moveData = ctx.ReadValue<Vector2>();
                controlls.PlayerTwoControlls.Move.canceled  += ctx => moveData = Vector2.zero;
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
        UpdateState();
        Move();
        Jump();

        rb.velocity  = m_velocity;
        // change to a state mashine later on in the developement
        ClearState(); 
    }

    #region temp state mashine replacement 
    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollission(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollission(collision);   
    }

    private void EvaluateCollission(Collision collision)
    {
        float minDot = GetMinDot(collision.gameObject.layer);
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            if(normal.y >= minDot)
            {
                m_groundContactCount += 1;
                m_contactNormal      += normal;
            }
            else if(normal.y > -0.01f)
            {
                m_steepContactCount += 1;
                m_steepNormal       += normal;
            }
        }
    }

    void UpdateState()
    {
        m_stepsSinceLastGrounded += 1;
        m_stepsSinceLastJumped   += 1;
        m_velocity = rb.velocity;

        if (m_isOnGround || SnapToGround() || CheckSteepContacts())
        {
            m_stepsSinceLastGrounded = 0;
            if(m_groundContactCount > 1)
                m_contactNormal.Normalize();
        }
        else
        {
            m_contactNormal = Vector3.up;
        }
    }
    #endregion

    private void Jump()
    {
        if (!m_isJumpPressed || !m_isOnGround)
            return;

        m_isJumpPressed = false;
        m_stepsSinceLastJumped = 0;

        float jumpSpeed    = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
        float alignedSpeed = Vector3.Dot(m_velocity, m_contactNormal);

        if (alignedSpeed > 0f)
            jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0);

        m_velocity += m_contactNormal * jumpSpeed;
    }

    private void Move()
    {
        moveData          = Vector2.ClampMagnitude(moveData, 1f);
        AdjustVelocity();
    }

    private Vector3 ProjectOnContactPlane (Vector3 vector)
    {
        return vector - m_contactNormal * Vector3.Dot(vector, m_contactNormal);
    }

    private void AdjustVelocity ()
    {
        float maxSpeedChange, acceleration;
        Vector3 desiredVelocity;

        if (m_playerInputSpace)
        {
            Vector3 forward = m_playerInputSpace.forward;
            forward.y = 0f;
            forward.Normalize();
            Vector3 right   = m_playerInputSpace.right;
            right.y   = 0f;
            right.Normalize();
            desiredVelocity = 
                (forward * moveData.y + right * moveData.x) * m_maxSpeed;
        }
        else
        {
            desiredVelocity = new Vector3(moveData.x, 0f, moveData.y) * m_maxSpeed;
        }

        Vector3 xAxis   = ProjectOnContactPlane(Vector3.right).normalized;
        Vector3 zAxis   = ProjectOnContactPlane(Vector3.forward).normalized;
                        
        float currentX  = Vector3.Dot(m_velocity, xAxis);
        float currentZ  = Vector3.Dot(m_velocity, zAxis);
                        
        acceleration    = m_isOnGround ?
                            m_maxAcceleration : m_maxAirAcceleration;

        maxSpeedChange  = acceleration * Time.deltaTime;

        m_velocity.x    =
            Mathf.MoveTowards(m_velocity.x, desiredVelocity.x, maxSpeedChange);
        m_velocity.z    =
            Mathf.MoveTowards(m_velocity.z, desiredVelocity.z, maxSpeedChange);
    }

    private void ClearState()
    {
        m_groundContactCount = m_steepContactCount = 0;
        m_contactNormal      = m_steepNormal       = Vector3.zero;
    }

    private bool SnapToGround()
    {
        if(m_stepsSinceLastGrounded > 1 || m_stepsSinceLastJumped <= 2)
        {
            return false;
        }
        float speed = m_velocity.magnitude;
        if(speed > m_maxSnapSpeed)
        {
            return false;
        }    
        if(!Physics.Raycast(rb.position, Vector3.down, out RaycastHit hit, m_probeDistance, m_probeMask))
        {
            return false;
        }
        if (hit.normal.y < GetMinDot(hit.collider.gameObject.layer))
        {
            return false;
        }

        m_groundContactCount = 1;
        m_contactNormal = hit.normal;
        float dot = Vector3.Dot(m_velocity, hit.normal);
        if(dot > 0f)
            m_velocity = (m_velocity - hit.normal * dot).normalized * speed;
        return true;
    }

    private float GetMinDot (int layer)
    {
        return m_stairMask != 1 << layer ?
            m_minGroundDotProduct : m_minStairsDotProduct;
    }

    private bool CheckSteepContacts()
    {
        if(m_steepContactCount > 1)
        {
            m_steepNormal.Normalize();
            if(m_steepNormal.y >= m_minGroundDotProduct)
            {
                m_groundContactCount = 1;
                m_contactNormal      = m_steepNormal;
                return true;
            }
        }
        return false;
    }
}
