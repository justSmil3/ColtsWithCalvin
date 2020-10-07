using System;
using System.Data;
using System.Linq;
using System.Net.Security;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Permissions;
using System.Security.Policy;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering;

[System.Serializable]
public struct Segment
{
    public SphereCollider   Collider;
    public Vector3          Position;
    public Vector3          PreviousPosition;
    public Vector3          Veloctity;
}

public class VerletIntegrationRope : MonoBehaviour
{
    //depricated
    [SerializeField]
    private GameObject
        m_instanciator, m_target;
    public GameObject Instanciator => m_instanciator; 
    public GameObject Target => m_target;

    [SerializeField]
    private float 
        m_segmentDistance        = 00.400f,
        m_ropeMass               = 00.250f,
        m_segmentCollisionRadius = 00.050f,
        m_stiffness              = 01.500f,
        m_gravityMultiplier      = 04.000f,
        m_maxPlayerDistance      = 10.000f,
        m_updateRate             = 25.000f,
        m_friction               = 00.200f,
        m_straightRopeThrashhold = 00.005f,
        m_directionThrashhold    = 00.200f;

    [SerializeField, Range(0, 1)]
    private float m_ropeFoldThrashhold = 0.65f;

    // variable for fitting the rope; 
    private float 
        m_segmentSpacing  = 0.00f,
        m_ropeLength;

    private bool
        m_isRopeStraight = false;

    [SerializeField]
    private LayerMask m_physicsLayer = default;

    private PlayerControlls m_controlls;
    private Segment[] m_segments;

    #region Setup -> Awake, Enable and Disable functions 
    private void Start()
    {
        m_segments = new Segment[0];
        m_controlls = new PlayerControlls();
        m_controlls.PlayerOneControlls.InitiateRope.performed += ctx => InstantiateRope(m_instanciator, Target);
        m_controlls.PlayerTwoControlls.InitiateRope.performed += ctx => InstantiateRope(m_instanciator, Target);
        m_controlls.Enable();
    }
    private void OnEnable()
    {
        if (m_controlls != null)
            m_controlls.Enable();
    }
    private void OnDisable()
    {
        if (m_controlls != null)
            m_controlls.Disable();
    }
    #endregion

    #region Debug Drawing
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = new Color(88, 0, 176);
        for (int i = 0; i < m_segments.Length; i++)
        {
            Gizmos.DrawSphere(m_segments[i].Position, m_segmentCollisionRadius);
        }
    }
    #endregion

    private void FixedUpdate()
    {
        SimulateRope();
    } 

    private void InstantiateRope(GameObject a, GameObject b)
    {
        if (m_segments.Length > 0)
        {
            foreach (Segment x in m_segments)
            {
                Destroy(x.Collider.gameObject);
            }
            m_segments = new Segment[0];
            return;
        }

        Rigidbody body1;
        Rigidbody body2;

        if (!a.TryGetComponent<Rigidbody>(out body1) || !b.TryGetComponent<Rigidbody>(out body2))
            return;

        Vector3 aPos = a.transform.position;
        Vector3 bPos = b.transform.position;

        m_instanciator  = a;
        m_target        = b;

        Vector3 routing = bPos - aPos;
        Vector3 dir     = routing.normalized;
        float   length  = routing.magnitude;
        m_ropeLength    = length;

        if (length > m_maxPlayerDistance) return;

        int numberOfSegments    = (int)(length / m_segmentDistance);
        m_segmentSpacing        = length / numberOfSegments;
        if(length % m_segmentDistance != 0)
        {
            numberOfSegments++;
        }

        m_segments = new Segment[numberOfSegments];
        for (int i = 0; i < numberOfSegments; i++)
        {
            Vector3 segmentPos  = dir * m_segmentSpacing * i + aPos;

            GameObject tmp      = new GameObject();
            tmp.layer           = 8; //Magic number i know but i honestly dont need this value twice and i surely do not need one to change it
            SphereCollider col  = tmp.gameObject.AddComponent<SphereCollider>(); 
            col.center          = segmentPos;
            col.radius          = m_segmentCollisionRadius;

            m_segments[i] = new Segment
            {
                Collider            = col,
                Position            = segmentPos,
                PreviousPosition    = segmentPos
            };
        }

    }

    //TODO this one is not working at all
    private void SimulateRope()
    {

        if (m_segments.Length == 0)
            return;

        //Calculate Velocity

        // check how straight the rope is in current time
        float dot = 0;
        for(int i = 0; i < m_segments.Length - 1; i++)
        {
            float checkDot = Mathf.Abs(
                   Vector3.Dot(m_segments[i].Collider.transform.forward,
                               m_segments[i + 1].Collider.transform.forward));
            if (checkDot <= m_ropeFoldThrashhold)
                continue;

            dot += Vector3.Dot(m_segments[i].Collider.transform.forward, 
                               m_segments[i + 1].Collider.transform.up);
        }
        m_isRopeStraight = (Mathf.Abs(dot) <= m_straightRopeThrashhold * m_segments.Length);
        // I thing this approach can also be used to reduce stretching in the rope middle 


        for (int r = 0; r < m_updateRate; r++)
        { 
            for (int i = 0; i < m_segments.Length; i++)
            {
                ref Segment data  = ref m_segments[i];

                Vector3 velocity  = data.Position - data.PreviousPosition;

                data.PreviousPosition 
                                  = data.Position;

                Vector3 SpringOne = Vector3.zero,
                        SpringTwo = Vector3.zero;

                if (i  > 0)
                    SpringOne = m_segments[i - 1].Position - data.Position;
                if (i  < m_segments.Length - 1)
                    SpringTwo = data.Position - m_segments[i + 1].Position;
                //if (i == 0)
                    //SpringOne = m_instanciator.GetComponent<Rigidbody>().velocity;
                //if (i == m_segments.Length - 1)
                    //SpringTwo =       m_target.GetComponent<Rigidbody>().velocity;

                float ks            = m_stiffness * m_ropeMass * m_segments.Length * Physics.gravity.magnitude;
                Vector3 fSpringOne  = -ks * (SpringOne.magnitude - m_segmentSpacing) * SpringOne.normalized;
                Vector3 fSpringTwo  = -ks * (SpringTwo.magnitude - m_segmentSpacing) * SpringTwo.normalized;
                Vector3 fGravity    =  Physics.gravity * m_ropeMass * m_gravityMultiplier;


                Vector3 force       = - fSpringOne + fGravity + fSpringTwo;

                data.Veloctity      =   velocity   + force    * Time.deltaTime;
                data.Veloctity     += - velocity   * m_friction;
                Debug.DrawRay(data.Position, data.Veloctity   * Time.deltaTime, Color.green); 
            }

            //Apply Velocity
            for (int i = 0; i < m_segments.Length; i++)
            {
                ref Segment data = ref m_segments[i];

                Vector3 deltaVelocity = data.Veloctity * Time.deltaTime;
                Vector3 VelocitySummand = Vector3.zero;

                Vector3[] V = new Vector3[]
                {
                new Vector3 ( deltaVelocity.x, 0.0f, 0.0f ),
                new Vector3 ( 0.0f, deltaVelocity.y, 0.0f ),
                new Vector3 ( 0.0f, 0.0f, deltaVelocity.z )
                };

                RaycastHit hitOne;
                RaycastHit hitTwo;
                RaycastHit hitThree;


                if (!Physics.Raycast(
                    data.Position, V[0].normalized, out hitOne, V[0].magnitude + m_segmentCollisionRadius, m_physicsLayer))
                {
                    VelocitySummand += V[0];
                }
                else if (hitOne.rigidbody != null)
                    ApplyCollision(hitOne.rigidbody, data);

                if (!Physics.Raycast(
                    data.Position, V[1].normalized, out hitTwo, V[0].magnitude + m_segmentCollisionRadius, m_physicsLayer))
                {
                    VelocitySummand += V[1];
                }
                else if (hitTwo.rigidbody != null)
                    ApplyCollision(hitTwo.rigidbody, data);

                if (!Physics.Raycast(
                    data.Position, V[2].normalized, out hitThree, V[0].magnitude + m_segmentCollisionRadius, m_physicsLayer))
                {
                    VelocitySummand += V[2];
                }
                else if (hitThree.rigidbody != null)
                    ApplyCollision(hitThree.rigidbody, data);


                if (i != 0 && i != m_segments.Length - 1)
                {
                    data.Position += VelocitySummand;
                    data.Collider.transform.position = data.Position;
                    data.Collider.transform.forward  =
                        (m_segments[i + 1].Position - data.Position).normalized;
                    //continue;
                }


                if (i == 0)
                {
                    data.Collider.transform.forward =
                        (m_segments[i + 1].Position - data.Position).normalized;
                    if (m_instanciator.TryGetComponent<Rigidbody>(out Rigidbody rBody))
                    {
                        float dirCheck = Vector3.Dot(
                            rBody.velocity.normalized, 
                            m_segments[i + 1].Collider.transform.forward);

                        if (m_isRopeStraight && dirCheck < 0 - m_directionThrashhold)
                            rBody.velocity = data.Veloctity;
                        data.Position = rBody.position;
                    }
                    //TODO here additional work has to be done
                }
                else
                if (i == m_segments.Length - 1)
                {
                    data.Collider.transform.forward =
                           (data.Position - m_segments[i - 1].Position).normalized;
                    if (m_target.TryGetComponent<Rigidbody>(out Rigidbody rBody))
                    {
                        float dirCheck = Vector3.Dot(
                            rBody.velocity.normalized,
                            m_segments[i - 1].Collider.transform.forward);

                        if (m_isRopeStraight && dirCheck > 0 + m_directionThrashhold)
                            rBody.velocity = data.Veloctity;
                        data.Position = rBody.position;
                    }
                    //TODO here additional work has to be done

                }

                //Debug Drawing
                //UnityEngine.Debug.DrawRay(data.Position, data.Collider.transform.right, Color.red);
                //UnityEngine.Debug.DrawRay(data.Position, data.Collider.transform.up, Color.green);
                //UnityEngine.Debug.DrawRay(data.Position, data.Collider.transform.forward, Color.blue);
            }
        }
    }

    //Todo: make this better 
    private void ApplyCollision(Rigidbody hitBody, Segment data)
    {
        Vector3 force = new Vector3(data.Veloctity.x, 0f, data.Veloctity.z);
        hitBody.AddForce(force);
    }

}