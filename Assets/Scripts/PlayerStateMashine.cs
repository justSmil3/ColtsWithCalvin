using UnityEngine;

public class PlayerStateMashine : MonoBehaviour
{
    protected IPlayerState m_currentState = null;
    
    protected void SwitchState(IPlayerState _state)
    {
        if (m_currentState != null)
            m_currentState.OnStateExit();
        m_currentState = _state;
        _state.OnStateEnter();
    }

    protected void FixedUpdate()
    {
        if (m_currentState != null)
            m_currentState.UpdateState();
    }
}
