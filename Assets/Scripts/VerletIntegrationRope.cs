using System;
using System.Net.Security;
using System.Runtime.InteropServices.ComTypes;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

[System.Serializable]
public struct Segment
{
    public GameObject collider;
    public Vector3 prevPos;
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 veloChange;
    public Vector3 veloAddition;
}

public class VerletIntegrationRope : MonoBehaviour
{
    [SerializeField]
    private GameObject Instanciator, Target;
    private PlayerControlls controlls; 


    public float segmentDistance = .5f;
    public float stiffness = 1250;
    public float stretchamount = 1f;
    public int accuracy = 50;
    public float gravitationMultiplyer = 1;
    [Range(0, 10)]
    public float bounce = 2;
    public int PhysicsLayer = 8;

    [UnityEngine.SerializeField]
    Segment[] segments;

    float MaxPlayerDistance;

    Vector3 PlayerOnePosition = Vector3.zero, PlayerTwoPosition = Vector3.zero;
    bool bRopeActive;

    Rigidbody rbp1, rbp2;

    private void Awake()
    {
        controlls = new PlayerControlls();
        controlls.PlayerOneControlls.InitiateRope.started += ctx => InstantiateRope(Instanciator, Target);
    //    if (Target == null) Instanciator.gameObject.SetActive(false);
    //    int numberOfPoints = 0;
    //    PlayerOnePosition = Instanciator.transform.position;
    //    PlayerTwoPosition = Target.transform.position;
    //    Vector3 dir = Target.transform.position - Instanciator.transform.position;
    //    Vector3 normalizedDir = dir.normalized;
    //    numberOfPoints = (int)(dir.magnitude / segmentDistance);
    //    MaxPlayerDistance = dir.magnitude;
    //    if(MaxPlayerDistance % segmentDistance != 0)
    //    {
    //        segmentDistance = dir.magnitude / numberOfPoints;
    //        numberOfPoints++;
    //    }
    //    segments = new Segment[numberOfPoints];
    //    for(int i = 0; i < numberOfPoints; i++)
    //    {
    //        Vector3 pos = Instanciator.transform.position;
    //        Vector3 posAddition = normalizedDir * i * segmentDistance;
    //        pos += posAddition;
    //        segments[i] = new Segment { position = pos, prevPos = pos };
    //    }

    //    rbp1 = Instanciator.GetComponent<Rigidbody>();
    //    rbp2 = Target.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        controlls.Enable();
    }

    private void OnDisable()
    {
        controlls.Disable();   
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(Instanciator.transform.position, .3f);
        if (Target != null)
            Gizmos.DrawSphere(Target.transform.position, .3f);
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            for(int i = 0; i < segments.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(segments[i].position, 0.05f);
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(segments[i].collider.transform.position, .05f);
            }
        }
    }

    bool ValidateVector3(Vector3 data)
    {
        bool one = (!float.IsNaN(data.x) && !float.IsNaN(data.y) && !float.IsNaN(data.z));
        bool two = (data.x != Mathf.Infinity && data.y != Mathf.Infinity && data.z != Mathf.Infinity);
        bool three = (data.x != Mathf.NegativeInfinity && data.y != Mathf.NegativeInfinity && data.z != Mathf.NegativeInfinity);
        return (one && two && three);    
    }

    private void FixedUpdate()
    {
        if (true) // change true to the position of the rope 
        {
            //VerletSimulation();
            if (segments.Length == 0) return;
            sim();
            ConstrainPlayerPosition();
        }
    }

    // dont judge by the velocity but rather by the input 
    void ConstrainPlayerPosition()
    {
        Vector3 currentPlayer1Position = Instanciator.transform.position, currentPlayer2Position = Target.transform.position;

        
        if (currentPlayer1Position == PlayerOnePosition && currentPlayer2Position == PlayerTwoPosition)
            return;


        if ((currentPlayer1Position - currentPlayer2Position).magnitude > MaxPlayerDistance + stretchamount)
        {
            float mmag = (rbp1.velocity - rbp2.velocity).magnitude;
            Vector3 mhat = Vector3.zero;
            Debug.Log("mmag = " + mmag + "\n" + "body one = " + rbp1.velocity + "\n" + " body two  = " + rbp2.velocity);
            if (rbp1.velocity.magnitude < rbp2.velocity.magnitude)
            {
                mhat = (segments[1].position - rbp1.position).normalized;
                rbp1.velocity += mmag * mhat * Time.deltaTime * rbp1.mass;
            }
            else if (rbp1.velocity.magnitude > rbp2.velocity.magnitude)
            {
                mhat = (segments[segments.Length - 2].position - rbp2.position).normalized;
                rbp2.velocity += mmag * mhat * Time.deltaTime * rbp2.mass;
            }
            else
            {
                mhat = (segments[segments.Length - 2].position - rbp2.position).normalized;
                rbp2.velocity += (mmag * mhat * Time.deltaTime * rbp2.mass) / 2;
                mhat = (segments[1].position - rbp1.position).normalized;
                rbp1.velocity += (mmag * mhat * Time.deltaTime * rbp1.mass) / 2;
            }

        }
        else
        {
            PlayerOnePosition = currentPlayer1Position;
            PlayerTwoPosition = currentPlayer2Position;
        }
    }

    //TODO use something else than rigid bodys to handle collisions
    void sim()
    {
        if (segments.Length == 0) return;
        Segment[] virtualSegments = new Segment[segments.Length];
        System.Array.Copy(segments, virtualSegments, segments.Length);

        //simulate on the virtual enviroment;
        for (int steps = 0; steps < accuracy; steps++)
        {
            for (int i = 0; i < virtualSegments.Length; i++)
            {
                Segment data = virtualSegments[i];


                Vector3 velocity = (data.position - data.prevPos) * Time.deltaTime;


                Vector3 SpringOne = Vector3.zero, SpringTwo = Vector3.zero;
                if (i > 0)
                {
                    SpringOne = virtualSegments[i - 1].position - data.position;
                }
                if (i < virtualSegments.Length - 1)
                {
                    SpringTwo = data.position - virtualSegments[i + 1].position;
                }



                float ks = stiffness;
                Vector3 fSpringOne = -ks * (SpringOne.magnitude - segmentDistance) * SpringOne.normalized;
                Vector3 fSpringTwo = -ks * (SpringTwo.magnitude - segmentDistance) * SpringTwo.normalized;

                float playerdistance = (Instanciator.transform.position - Target.transform.position).magnitude;
                float pdr = Mathf.Clamp01(playerdistance / MaxPlayerDistance);
                float pdrinv = 1 - pdr;
                
                Vector3 fGravity = pdrinv * Physics.gravity * gravitationMultiplyer;

                Vector3 force = fSpringTwo + fGravity - fSpringOne;

                velocity = velocity + force * Time.deltaTime;
                data.velocity = velocity;

                virtualSegments[i] = data;
            }

            //apply velocit to position;
            for (int i = 0; i < virtualSegments.Length; i++)
            {
                Segment data = virtualSegments[i];
                data.prevPos = data.position;
                if (i > 0 && i < virtualSegments.Length - 1)
                {
                    //temp
                    Vector3 tempPosition = data.position + data.velocity * Time.deltaTime;
                    Rigidbody rbtemp = data.collider.GetComponent<Rigidbody>();
                    rbtemp.velocity += (tempPosition - rbtemp.position) * Time.deltaTime; 
                    data.position = tempPosition;
                }
                else if ( i == 0)
                {
                    Rigidbody rbtemp = data.collider.GetComponent<Rigidbody>();
                    rbtemp.position = Instanciator.transform.position;
                    data.position = Instanciator.transform.position;
                }
                else if(i == virtualSegments.Length-1)
                {
                    Rigidbody rbtemp = data.collider.GetComponent<Rigidbody>();
                    rbtemp.position = Target.transform.position;
                    data.position = Target.transform.position;
                }
                virtualSegments[i] = data;
            }
        }

        //apply sim
        System.Array.Copy(virtualSegments, segments, segments.Length);
    }

    public void InstantiateRope(GameObject a, GameObject b)
    {
        // WIP make the rope change system somehow different 

        if (segments.Length > 0)
        {
            foreach(Segment x in segments)
            {
                Destroy(x.collider);
            }
            segments = new Segment[0];
        }
        else if (a.TryGetComponent<Rigidbody>(out rbp1) && b.TryGetComponent<Rigidbody>(out rbp2))
        {
            int numberOfPoints = 0;
            PlayerOnePosition = a.transform.position;
            PlayerTwoPosition = b.transform.position;
            Instanciator = a;
            Target = b;
            Vector3 dir = Target.transform.position - Instanciator.transform.position;
            Vector3 normalizedDir = dir.normalized;
            numberOfPoints = (int)(dir.magnitude / segmentDistance);
            MaxPlayerDistance = dir.magnitude;
            if (MaxPlayerDistance % segmentDistance != 0)
            {
                segmentDistance = dir.magnitude / numberOfPoints;
                numberOfPoints++;
            }
            segments = new Segment[numberOfPoints];
            for (int i = 0; i < numberOfPoints; i++)
            {
                Vector3 pos = Instanciator.transform.position;
                Vector3 posAddition = normalizedDir * i * segmentDistance;
                pos += posAddition;
                GameObject temp = new GameObject();
                SphereCollider col = temp.AddComponent<SphereCollider>();
                Rigidbody rb = temp.AddComponent<Rigidbody>();
                
                rb.position = pos;
                temp.transform.position = pos;
                rb.useGravity = false;
                rb.drag = 10 - bounce;
                col.radius = .05f;
                temp.layer = PhysicsLayer;
                segments[i] = new Segment
                {
                    position = pos,
                    prevPos = pos,
                    velocity = Vector3.zero,
                    collider = temp
                };
            }
        }
    }
}
