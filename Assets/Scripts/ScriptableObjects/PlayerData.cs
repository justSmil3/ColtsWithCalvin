using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : ScriptableObject
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
        cameraRotationSpeed = 15f,
        m_fallMultiplyer = 2.5f,
        m_lowJumpMultiplyer = 2f;

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

    PlayerControlls controlls;
    public PlayerChoise p;

    Vector2 moveData = Vector2.zero;
    Vector2 cameraMoveData = Vector2.zero;

    int
        m_groundContactCount = 0,
        m_steepContactCount = 0,
        m_climpContactCount = 0,
        m_stepsSinceLastGrounded = 0,
        m_stepsSinceLastJumped = 0;

    bool
        m_isJumpPressed,
        m_isOnCliff;

    bool m_isOnGround => m_groundContactCount > 0;
    bool m_isOnSteep => m_steepContactCount > 0;
    bool m_isClimbing => m_climpContactCount > 0;

    private Vector3
        m_velocity,
        m_contactNormal,
        m_steepNormal,
        m_climbNormal,
        //temp 
        m_dirToCliff;


    private float
        m_minGroundDotProduct,
        m_minStairsDotProduct;
}
