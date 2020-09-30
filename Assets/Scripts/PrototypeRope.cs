using SplineMesh;
using System.Data;
using System.Xml.Serialization;
using UnityEngine;
public class PrototypeRope : MonoBehaviour
{
    [SerializeField]
    private GameObject Instanciator, Target;
    private PlayerControlls controlls;


    public float segmentDistance = .5f;
    public float pointRadius = .05f;
    public float stiffness = 1250;
    public float stretchamount = 1f;
    public int accuracy = 50;
    public float gravitationMultiplyer = 1;
    public float ropeMass = 1;
    [Range(0, 10)]
    public float bounce = 2;
    public LayerMask PhysicsLayer;

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
            for (int i = 0; i < segments.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(segments[i].position, pointRadius);
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(segments[i].collider.transform.position, pointRadius);
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

                velocity = velocity + force * Time.deltaTime + data.veloChange * Time.deltaTime;
                data.velocity = velocity;
                data.veloChange = Vector3.zero;

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
                    Vector3 tempPosition = data.position;
                    Vector3 addedForce = Vector3.zero;
                    Vector3 vx = new Vector3(data.velocity.x, 0f, 0f);
                    Vector3 vy = new Vector3(0f, data.velocity.y, 0f);
                    Vector3 vz = new Vector3(0f, 0f, data.velocity.z);
                    //does not quite work yet
                    if (!Physics.Raycast(tempPosition, vx.normalized, out RaycastHit hit, vx.magnitude * 2 + pointRadius, PhysicsLayer))
                    {
                        addedForce += vx;
                        Debug.DrawRay(tempPosition, vx, Color.yellow);
                    }
                    else
                    { 
                        if(hit.rigidbody != null)
                        {
                            ApplyRopeCollision(hit.rigidbody, data);
                        }
                        Debug.DrawRay(tempPosition, vx, Color.red); 
                    }
                    if (!Physics.Raycast(tempPosition, vy.normalized, out RaycastHit hit2, vy.magnitude * 2 + pointRadius, PhysicsLayer))
                    {
                        addedForce += vy;
                        Debug.DrawRay(tempPosition, vy, Color.green);
                    }
                    else
                    {
                        if (hit2.rigidbody != null)
                        {
                            ApplyRopeCollision(hit2.rigidbody, data);
                        }
                        Debug.DrawRay(tempPosition, vx, Color.red);
                    }
                    if (!Physics.Raycast(tempPosition, vz.normalized, out RaycastHit hit3, vz.magnitude * 2 + pointRadius, PhysicsLayer))
                    {
                        addedForce += vz;
                        Debug.DrawRay(tempPosition, vz, Color.blue);
                    }
                    else
                    {
                        if (hit3.rigidbody != null)
                        {
                            ApplyRopeCollision(hit3.rigidbody, data);
                        }
                        Debug.DrawRay(tempPosition, vx, Color.red);
                    }
                    tempPosition += addedForce * Time.deltaTime;
                    Rigidbody rbtemp = data.collider.GetComponent<Rigidbody>();
                    rbtemp.velocity += (tempPosition - rbtemp.position) * Time.deltaTime;
                    data.position = tempPosition;
                }
                else if (i == 0)
                {
                    Rigidbody rbtemp = data.collider.GetComponent<Rigidbody>();
                    rbtemp.position = Instanciator.transform.position;
                    data.position = Instanciator.transform.position;
                }
                else if (i == virtualSegments.Length - 1)
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
            foreach (Segment x in segments)
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
                col.radius = pointRadius;
                col.enabled = false;
                //make a new variable for this
                temp.layer = 8;
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

    //this code does not work XD 
    public void ApplyRopeCollision(Rigidbody other, Segment data)
    {
        float dt = Time.deltaTime;
        float m1 = ropeMass;
        float m2 = other.mass;
        Vector3 v1 = data.velocity;
        Vector3 v2 = other.velocity * dt;

        // calculate Impact
        float F1 = -m2 * v2.magnitude;
        float F2 = -m1 * v1.magnitude;

        // calculate direction 
        v1.Normalize();
        v2.Normalize();

        float a = Vector3.Dot(v1, v2) * Vector3.Angle(v1, v2);
        Vector3 d1 = Vector3.RotateTowards(v2, v1, a, 1);
        Vector3 d2 = Vector3.RotateTowards(v1, v2, a, 1);

        //calculate new momentum
        //these somehow return a zero vector
        Vector3 p1 = d1 * F1;
        Vector3 p2 = d2 * F2;

        UnityEngine.Debug.DrawRay(other.position, p2, Color.black);
        UnityEngine.Debug.DrawRay(data.position, p1, Color.black);
        UnityEngine.Debug.LogError(p2 + " : " + p2);
        data.veloChange += p1;
        other.velocity = p2;

    }

}
