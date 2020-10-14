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
using UnityEngine.Rendering.Universal;

[System.Serializable]
public enum PlayerChoise
{
    PlayerOne,
    PlayerTwo
}
public class CustomCharacterController : PlayerStateMashine
{
    #region Variables
    private Rigidbody rb;

    [SerializeField]
    private Transform m_playerInputSpace;

    [SerializeField]
    private float
        m_jumpForce = 10,
        m_maxSpeed = 20,
        m_fallMultiplyer = 2.5f,
        m_lowJumpMultiplyer = 2f;

    [SerializeField, Range(0, 1)]
    private float m_drag;

    [SerializeField, Range(0, 50)]
    private float
        m_maxAcceleration = 10.00f,
        m_maxAirAcceleration = 01.00f,
        m_maxCliffAcceleration = 04.00f;

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
        m_stairMask = -1,
        m_cliffMask = -1;

    private Vector2 moveData = Vector2.zero;
    private Vector2 cameraMoveData = Vector2.zero;

    private int
        m_groundContactCount = 0,
        m_steepContactCount = 0,
        m_climpContactCount = 0,
        m_stepsSinceLastGrounded = 0,
        m_stepsSinceLastJumped = 0;

    private bool
        m_isJumpPressed,
        m_isOnCliff;

    private bool m_isOnGround => m_groundContactCount > 0;
    private bool m_isOnSteep => m_steepContactCount > 0;
    private bool m_isClimbing => m_climpContactCount > 0;

    private Vector3
        m_velocity,
        m_contactNormal,
        m_steepNormal,
        m_climbNormal,
        //temp 
        m_dirToCliff;


    private float
        m_minGroundDotProduct,
        m_minStairsDotProduct,
        m_realDrag;



    private PlayerControlls controlls;
    [SerializeField]
    private PlayerChoise p;
    #endregion
    #region Accessors
    public Rigidbody Rb() { return rb; }
    public Transform PlayerInputSpace() { return m_playerInputSpace; }
    public float JumpForce() { return m_jumpForce; }
    public float MaxSpeed() { return m_maxSpeed; }
    public float FallMultiplyer() { return m_fallMultiplyer; }
    public float LowJumpMultiplyer() { return m_lowJumpMultiplyer; }
    public float MaxAcceleration() { return m_maxAcceleration; }
    public float MaxAirAcceleration() { return m_maxAirAcceleration; }
    public float MaxCliffAcceleration() { return m_maxCliffAcceleration; }
    public float MaxGroundAngle() { return m_maxGroundAngle; }
    public float MaxStairsAngle() { return m_maxStairsAngle; }
    public float JumpHeight() { return jumpHeight; }
    public float MaxSnapSpeed() { return m_maxSnapSpeed; }
    public float ProbeDistance() { return m_probeDistance; }
    public float MinGroundDotProduct() { return m_minGroundDotProduct; }
    public float MinStairsDotProduct() { return m_minStairsDotProduct; }
    public bool IsJumpPressed() { return m_isJumpPressed; }
    public Vector3 Velocity() { return m_velocity; }
    public Vector3 ContactNormal() { return m_contactNormal; }
    public Vector3 SteepNormal() { return m_steepNormal; }
    public Vector3 ClimbNormal() { return m_climbNormal; }
        //temp
    public Vector3 DirToCliff() { return m_dirToCliff; }
    public PlayerControlls Controlls() { return controlls; }
    public PlayerChoise P() { return p; }
    public int GroundContactCount() { return m_groundContactCount; }
    public int SteepContactCount() { return m_steepContactCount; }
    public int ClimpContactCount() { return m_climpContactCount; }
    public int StepsSinceLastGrounded() { return m_stepsSinceLastGrounded; }
    public int StepsSinceLastJumped() { return m_stepsSinceLastJumped; }
    public float Drag() { return m_realDrag; }
    #endregion


    public void OnValidate()
    {
        m_minGroundDotProduct = Mathf.Cos(m_maxGroundAngle * Mathf.Deg2Rad);
        m_minStairsDotProduct = Mathf.Cos(m_maxStairsAngle * Mathf.Deg2Rad);
        m_realDrag = 1 - m_drag;
    }

    private void Awake()
    {
        OnValidate();
        m_playerInputSpace = m_playerInputSpace; 
        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = false;
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

        SwitchState(new PlayerState_OnGround(this));
    }

    private void OnEnable()
    {
        controlls.Enable();
    }

    private void OnDisable()
    {
        controlls.Disable();
    }

    protected override void FixedUpdate()
    {
        UpdateState();
        base.FixedUpdate();
        Move();
        Jump();


        float gravityMultiplyer = 1f;
        if (!m_isOnGround)
        {
            if (m_velocity.y < 0)
                gravityMultiplyer = m_fallMultiplyer;
            else if (!m_isJumpPressed)
                gravityMultiplyer = m_lowJumpMultiplyer;
        }
        if (!m_isClimbing)
            m_velocity += Physics.gravity * gravityMultiplyer * Time.deltaTime;

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
        int layer = collision.gameObject.layer;
        float minDot = GetMinDot(layer);
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            if (normal.y >= minDot)
            {
                m_groundContactCount += 1;
                m_contactNormal += normal;
            }
            else
            {
                if (normal.y > -0.01f)
                {
                    m_steepContactCount += 1;
                    m_steepNormal += normal;
                }
                else if ((m_cliffMask & (1 << layer)) != 0)
                {
                    m_dirToCliff = (collision.GetContact(0).point - transform.position).normalized;
                    m_climpContactCount += 1;
                    if (!m_isOnCliff)
                    {
                        rb.velocity = Vector3.zero;
                    }
                    m_isOnCliff = true;
                }
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
            if (m_groundContactCount > 1)
                m_contactNormal.Normalize();

        }
        else if (!m_isOnCliff)
        {
            m_contactNormal = Vector3.zero;
            m_isJumpPressed = false;
            temp_checkForPress = false;
        }
        else
        {
            //player is on the cliff
        }
    }
    #endregion



    private void Move()
    {
        AdjustVelocity();
        AdjustRotation();
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
                            m_maxAcceleration : m_isOnCliff ?
                                m_maxCliffAcceleration : m_maxAirAcceleration;

        maxSpeedChange  = acceleration * Time.deltaTime;

        if(m_isOnCliff)
        {
            // fürs erste. das ganze system hier funkt nämlich nicht wirklich 
            ClimpWall();
        }

        float xVelo    =
            Mathf.MoveTowards(m_velocity.x, desiredVelocity.x, maxSpeedChange);
        float zVelo    =
            Mathf.MoveTowards(m_velocity.z, desiredVelocity.z, maxSpeedChange);

        if(m_isOnGround && moveData == Vector2.zero)
        {
            xVelo *= m_realDrag;
            zVelo *= m_realDrag;
        }

        m_velocity.x = xVelo;
        m_velocity.z = zVelo;

    }

    private void AdjustRotation()
    {
        Vector3 dir = m_playerInputSpace.forward;
        Vector3 forward = dir - Vector3.up * Vector3.Dot(dir, Vector3.up);
        Quaternion rotation =
            Quaternion.LookRotation(
                forward.normalized,
                this.transform.up.normalized);
        this.transform.rotation = rotation;
    }

    private void ClearState()
    {
        //temp till state mashine arrives 
        if (m_isClimbing)
            m_isOnCliff = false;
        m_groundContactCount = m_steepContactCount = m_climpContactCount = 0;
        m_contactNormal      = m_steepNormal       = Vector3.zero;
    }

    private bool SnapToGround()
    {
        if (m_stepsSinceLastGrounded > 1 || m_stepsSinceLastJumped <= 2)
        {
            return false;
        }
        float speed = m_velocity.magnitude;
        if (speed > m_maxSnapSpeed)
        {
            return false;
        }
        if (!Physics.Raycast(rb.position, Vector3.down, out RaycastHit hit, m_probeDistance, m_probeMask))
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
        if (dot > 0f)
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
        if (m_steepContactCount > 1)
        {
            m_steepNormal.Normalize();
            if (m_steepNormal.y >= m_minGroundDotProduct)
            {
                m_groundContactCount = 1;
                m_contactNormal = m_steepNormal;
                return true;
            }
        }
        return false;
    }

    //again temp ..,. 
    public bool temp_checkForPress = false;
    private void ClimpWall()
    {
        if (!temp_checkForPress && !m_isJumpPressed) return;
        temp_checkForPress = true;

        rb.velocity += Vector3.up   * m_maxCliffAcceleration * Time.deltaTime +
                       m_dirToCliff * m_maxCliffAcceleration * 10 * Time.deltaTime;

    }


    //temporary back in here until char controller is using 100% state mashine logic
    private void Jump()
    {
        if (!m_isJumpPressed || !m_isOnGround)
            return;

        m_isJumpPressed = false;
        m_stepsSinceLastJumped = 0;

        float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
        float alignedSpeed = Vector3.Dot(m_velocity, m_contactNormal);

        if (alignedSpeed > 0f)
            jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0);

        m_velocity += m_contactNormal * jumpSpeed;
    }
}

