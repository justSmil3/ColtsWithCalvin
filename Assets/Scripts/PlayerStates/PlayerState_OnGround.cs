
using UnityEngine;
using System.Collections;

public class PlayerState_OnGround : PlayerState
{

    public PlayerState_OnGround(CustomCharacterController _data) : base(_data)
    {
    }


    public override IEnumerator UpdateState()
    {
        ////from SnapToGround
        //if (m_data.m_stepsSinceLastGrounded <= 1 && m_data.m_stepsSinceLastJumped > 2)
        //{
        //    float speed = m_data.m_velocity.magnitude;
        //    if (speed <= m_data.m_maxSnapSpeed)
        //    {
        //        m_data.m_groundContactCount = 1;
        //        m_data.m_contactNormal = m_data.hit.normal;
        //        float dot = Vector3.Dot(m_data.m_velocity, m_data.m_hit.normal);
        //        if (dot > 0f)
        //            m_data.m_velocity = (m_data.m_velocity - m_data.m_hit.normal * dot).normalized * speed;
        //    }
        //}

        //    //from CheckSteepContacts
        //    if (m_data.SteepContactCount() > 1)
        //    {
        //        Vector3 steepNormal = m_data.SteepNormal().normalized;
        //        if (m_data.SteepNormal().y >= m_data.MinGroundDotProduct())
        //        {
        //            m_data.m_groundContactCount = 1;
        //            m_data.m_contactNormal = m_data.m_steepNormal;
        //        }
        //    }

        //    //from UdateState
        //    m_data.m_stepsSinceLastGrounded = 0;
        //    if (m_data.m_groundContactCount > 1)
        //        m_data.m_contactNormal.Normalize();

        //    Jump();
        //    yield return null;

        //}

        //private void Jump()
        //{
        //    if (!m_data.m_isJumpPressed)
        //        return;

        //    m_data.m_isJumpPressed = false;
        //    m_data.m_stepsSinceLastJumped = 0;

        //    float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * m_data.jumpHeight);
        //    float alignedSpeed = Vector3.Dot(m_data.m_velocity, m_data.m_contactNormal);

        //    if (alignedSpeed > 0f)
        //        jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0);

        //    m_data.m_velocity += m_data.m_contactNormal * jumpSpeed;
        yield return null;
    }

}
