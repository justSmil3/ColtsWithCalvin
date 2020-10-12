using UnityEditor.VersionControl;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField]
    private Transform m_focus = default;

    [SerializeField]
    private LayerMask m_obstructionMask = -1;

    [SerializeField, Range(1f, 20f)]
    private float m_distance = 5f;

    [SerializeField, Min(0f)]
    private float m_focusRadius = 1f;

    [SerializeField, Range(0f, 1f)]
    private float m_focusCentering = 0.5f;

    [SerializeField, Range(1f, 360f)]
    private float m_rotationSpeed = 90f;

    [SerializeField, Range(-89f, 89f)]
    private float 
        m_minVerticalAngle = -30f, 
        m_maxVerticalAngle = 60f;

    [SerializeField, Min(0f)]
    private float m_alignDelay = 5f;

    [SerializeField, Range(0f, 90f)]
    private float m_alignSmoothRange = 45f;

    private Vector3
        m_focusPoint,
        m_previousFocusPoint;

    private Vector2
        m_orbitAngles = new Vector3(45f, 0f),
        m_cameraInput;

    private float
        m_lastManualRotationTime;

    private PlayerControlls controlls;

    private Camera m_regularCamera;
    private Vector3 m_cameraHalfExtends
    {
        get
        {
            Vector3 halfExtends;
            halfExtends.y =
                m_regularCamera.nearClipPlane *
                Mathf.Tan(0.5f * Mathf.Deg2Rad * m_regularCamera.fieldOfView);
            halfExtends.x = halfExtends.y * m_regularCamera.aspect;
            halfExtends.z = 0f;
            return halfExtends;
        }
    }

    private void OnValidate()
    {
        if (m_maxVerticalAngle < m_minVerticalAngle)
            m_maxVerticalAngle = m_minVerticalAngle;
    }

    private void OnEnable()
    {
        if (!m_focus) return;
        controlls = new PlayerControlls();
        CustomCharacterController cCon = m_focus.GetComponent<CustomCharacterController>();
        if (!cCon)    return;
        switch (cCon.p)
        {
            case PlayerChoise.PlayerOne:
                controlls.PlayerOneControlls.Camera.performed += 
                    ctx => m_cameraInput = new Vector2(-ctx.ReadValue<Vector2>().y, ctx.ReadValue<Vector2>().x);
                controlls.PlayerOneControlls.Camera.canceled  += 
                    ctx => m_cameraInput = Vector2.zero;
                break;
            case PlayerChoise.PlayerTwo:
                controlls.PlayerTwoControlls.Camera.performed += 
                    ctx => m_cameraInput = new Vector2(ctx.ReadValue<Vector2>().y, ctx.ReadValue<Vector2>().x);
                controlls.PlayerTwoControlls.Camera.canceled +=
                    ctx => m_cameraInput = Vector2.zero;
                break;
            default:
                break;
        }
        controlls.Enable();
    }

    private void OnDisable()
    {
        if (controlls == null) return;
        controlls.Disable();   
    }

    private void Awake()
    {
        m_regularCamera         = GetComponent<Camera>();
        m_focusPoint            = m_focus.position;
        transform.localRotation = Quaternion.Euler(m_orbitAngles);
    }

    private void LateUpdate()
    {
        UpdateFocusPoint();

        Quaternion lookRotation = Quaternion.Euler(m_orbitAngles);

        if (ManualRotation() || AutomaticRotation())
        {
            ConstainAngles();
            lookRotation = Quaternion.Euler(m_orbitAngles);
        }
        else
        {
            lookRotation = transform.localRotation;
        }

        Vector3 lookDirection   = lookRotation * Vector3.forward;
        Vector3 lookPosition    = m_focusPoint - lookDirection * m_distance;

        Vector3 rectOffset      = lookDirection * m_regularCamera.nearClipPlane;
        Vector3 rectPosition    = lookPosition + rectOffset;
        Vector3 castFrom        = m_focus.position;
        Vector3 castLine        = rectPosition - castFrom;
        float castDistance      = castLine.magnitude;
        Vector3 castDirection   = castLine / castDistance;

        if (Physics.BoxCast(
            castFrom, m_cameraHalfExtends, castDirection, out RaycastHit hit, 
            lookRotation, castDistance, m_obstructionMask))
        {
            rectPosition = castFrom + castDirection * hit.distance;
            lookPosition = rectPosition - rectOffset;
        }

        transform.SetPositionAndRotation(lookPosition, lookRotation);
    }

    private void UpdateFocusPoint ()
    {
        m_previousFocusPoint = m_focusPoint; 
        Vector3 targetPoint  = m_focus.position;

        if(m_focusRadius > 0f)
        {
            float distance = Vector3.Distance(targetPoint, m_focusPoint);
            float t = 1f; 
            if(distance > 0.01f && m_focusCentering > 0f)
            {
                t = Mathf.Pow(1f - m_focusCentering, Time.unscaledDeltaTime);
            }
            if(distance > m_focusRadius)
            {
                t = Mathf.Min(t, m_focusRadius / distance);
            }
            m_focusPoint = Vector3.Lerp(targetPoint, m_focusPoint, t);
        }
        else
        {
            m_focusPoint = targetPoint;
        }
    }

    private bool ManualRotation()
    {
        const float e = 0.001f;
        if(m_cameraInput.x < -e || m_cameraInput.x > e ||
           m_cameraInput.y < -e || m_cameraInput.y > e)
        {
            m_orbitAngles += m_rotationSpeed * Time.unscaledDeltaTime * m_cameraInput;
            m_lastManualRotationTime = Time.unscaledTime;
            return true;
        }
        return false;
    }

    bool AutomaticRotation()
    {
        if(Time.unscaledTime - m_lastManualRotationTime < m_alignDelay)
        {
            return false;
        }

        Vector2 movement = new Vector2(
            m_focusPoint.x - m_previousFocusPoint.x,
            m_focusPoint.z - m_previousFocusPoint.y);
        float movementDeltaSqr = movement.sqrMagnitude;
        if(movementDeltaSqr < 0.000001f)
        {
            return false;
        }

        float headingAngle   = GetAngle(movement / Mathf.Sqrt(movementDeltaSqr));
        float deltaAbs       = Mathf.Abs(Mathf.DeltaAngle(m_orbitAngles.y, headingAngle));
        float rotationChange = m_rotationSpeed * Mathf.Min(Time.unscaledDeltaTime, movementDeltaSqr);

        if(deltaAbs < m_alignSmoothRange)
        {
            rotationChange *= deltaAbs / m_alignSmoothRange;
        }
        else if(180 - deltaAbs < m_alignSmoothRange)
        {
            rotationChange *= (180f - deltaAbs) / m_alignSmoothRange;
        }

        m_orbitAngles.y =
            Mathf.MoveTowardsAngle(m_orbitAngles.y, headingAngle, rotationChange);

        return true;
    }

    private void ConstainAngles()
    {
        m_orbitAngles.x =
            Mathf.Clamp(m_orbitAngles.x, m_minVerticalAngle, m_maxVerticalAngle);

        if(m_orbitAngles.y < 0f)
        {
            m_orbitAngles.y += 360f;
        }
        else if(m_orbitAngles.y >= 360f)
        {
            m_orbitAngles.y -= 360f;
        }
    }

    private static float GetAngle (Vector2 direction)
    {
        float  angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
        return direction.x < 0f ? 360f - angle : angle;
    }

}
