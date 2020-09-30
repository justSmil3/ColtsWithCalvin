using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimplePlayer : MonoBehaviour
{
    public KeyCode w;
    public KeyCode s;
    public KeyCode a;
    public KeyCode d;
    public KeyCode jump;
    public KeyCode shootRope;
    
    private Vector3 i;
    public float speed;
    public float jumpHeight;
    public float playerHeight;

    private bool moved;
    private bool isGrounded;

    private Rigidbody _rigidbody;
    private ConfigurableJoint _joint;
    
    public LayerMask ground;

    public Rigidbody otherPlayer;
    public GameObject ropePartPrefab;

    public GameObject rope;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _joint = GetComponent<ConfigurableJoint>();

        _joint.xMotion = ConfigurableJointMotion.Free;
        _joint.yMotion = ConfigurableJointMotion.Free;
        _joint.zMotion = ConfigurableJointMotion.Free;
    }

    // Update is called once per frame
    void Update()
    {
        i = Vector3.zero;
        Debug.DrawLine(_rigidbody.position,_rigidbody.position+Vector3.down * playerHeight,Color.blue);
        if (Physics.Raycast(_rigidbody.position, Vector3.down, playerHeight, ground))
        {
            isGrounded = true;
            if (Input.GetKey(w))
            {
                i.z += 1;
            }

            if (Input.GetKey(s))
            {
                i.z -= 1;
            }

            if (Input.GetKey(a))
            {
                i.x -= 1;
            }

            if (Input.GetKey(d))
            {
                i.x += 1;
            }

            if (Input.GetKeyDown(jump))
            {
                Jump();
            }
        }
        else
        {
            isGrounded = false;
        }


        if (Input.GetKeyDown(shootRope))
        {
            ShootRope();
        }
    }

    private void FixedUpdate()
    {
        if (i == Vector3.zero && moved && isGrounded)
        {
            moved = false;
            _rigidbody.velocity = Vector3.zero;
        }
        else
        {
            moved = true;
            _rigidbody.AddForce(i * speed, ForceMode.Impulse);
        }
    }


    private void Jump()
    {
        _rigidbody.AddForce(Vector3.up*jumpHeight,ForceMode.Impulse);
    }

    private void ShootRope()
    {
        if (rope.transform.childCount == 0)
        {
            float distance = Vector3.Distance(_rigidbody.position, otherPlayer.position);
            Vector3 dir = otherPlayer.position - _rigidbody.position;
            ConfigurableJoint ropePart = _joint;
            for (float i = 0; i < distance; i += 0.4f)
            {
                GameObject ropePartNew = Instantiate(ropePartPrefab, _rigidbody.position + (dir * 0.4f),
                    Quaternion.identity,
                    rope.transform);

                ropePart.connectedBody =
                    ropePartNew.GetComponent<Rigidbody>();

                ropePart = ropePartNew.GetComponent<ConfigurableJoint>();
            }

            ropePart.connectedBody = otherPlayer;

            ConfigurableJoint otherJoint = otherPlayer.GetComponent<ConfigurableJoint>();
            
            
            SetConfigurableJointMotion(ConfigurableJointMotion.Locked,_joint);
        }
        else
        {
            
            ConfigurableJoint otherJoint = otherPlayer.GetComponent<ConfigurableJoint>();
            
            
            SetConfigurableJointMotion(ConfigurableJointMotion.Free,_joint);
            
            int all = rope.transform.childCount;
            for (int i = 0; i < all ; i++)
            {
                Destroy(rope.transform.GetChild(i).gameObject);
            }
        }
    }


    private void SetConfigurableJointMotion(ConfigurableJointMotion motion, ConfigurableJoint joint)
    {
        joint.xMotion = motion;
        joint.yMotion = motion;
        joint.zMotion = motion;
    }
}
