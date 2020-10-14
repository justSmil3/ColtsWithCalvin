using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Player Values", menuName = "ScriptableObjects/Data")]
public class PlayerData : ScriptableObject
{
    public Rigidbody rb;

    [SerializeField]
    public Transform m_playerInputSpace;

    [SerializeField]
    public float
        fallMultiplyer = 2.5f,
        lowJumpMultiplyer = 2f,
        jumpForce = 10,
        m_maxSpeed = 20,
        cameraRotationSpeed = 15f,
        m_fallMultiplyer = 2.5f,
        m_lowJumpMultiplyer = 2f;

    [SerializeField, Range(0, 50)]
    public float
        m_maxAcceleration = 10.00f,
        m_maxAirAcceleration = 01.00f,
        m_maxCliffAcceleration = 04.00f;

    [SerializeField, Range(0f, 90f)]
    public float
        m_maxGroundAngle = 25f,
        m_maxStairsAngle = 50f;

    [SerializeField, Range(0f, 10f)]
    public float jumpHeight = 2f;

    [SerializeField, Range(0f, 100f)]
    public float m_maxSnapSpeed = 100f;

    [SerializeField, Min(0f)]
    public float m_probeDistance = 1f;

    [SerializeField]
    public LayerMask
        m_probeMask = -1,
        m_stairMask = -1,
        m_cliffMask = -1;

    public Vector2 moveData = Vector2.zero;
    public Vector2 cameraMoveData = Vector2.zero;

    public int
        m_groundContactCount = 0,
        m_steepContactCount = 0,
        m_climpContactCount = 0,
        m_stepsSinceLastGrounded = 0,
        m_stepsSinceLastJumped = 0;

    public bool
        m_isJumpPressed,
        m_isOnCliff;

    public bool m_isOnGround => m_groundContactCount > 0;
    public bool m_isOnSteep => m_steepContactCount > 0;
    public bool m_isClimbing => m_climpContactCount > 0;

    public Vector3
        m_velocity,
        m_contactNormal,
        m_steepNormal,
        m_climbNormal,
        //temp 
        m_dirToCliff;


    public float
        m_minGroundDotProduct,
        m_minStairsDotProduct;

    public void OnValidate()
    {
        m_minGroundDotProduct = Mathf.Cos(m_maxGroundAngle * Mathf.Deg2Rad);
        m_minStairsDotProduct = Mathf.Cos(m_maxStairsAngle * Mathf.Deg2Rad);
    }

    public RaycastHit m_hit;
}
