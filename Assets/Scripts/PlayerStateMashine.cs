using UnityEngine;
using System;
using System.CodeDom;

public class PlayerStateMashine : MonoBehaviour
{
    protected PlayerState m_currentState = null;
    
    protected void SwitchState(PlayerState _state)
    {
    //    if (_state.GetType() == m_currentState.GetType())
    //        return;

        if (m_currentState != null)
            StartCoroutine(m_currentState.OnStateExit());
        m_currentState = _state;
        StartCoroutine(_state.OnStateEnter());
    }

    protected virtual void FixedUpdate()
    {
        if (m_currentState == null)
            return;
        StartCoroutine(m_currentState.UpdateState());
        Debug.Log(m_currentState);

    }
}
