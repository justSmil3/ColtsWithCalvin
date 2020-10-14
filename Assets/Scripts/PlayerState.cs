
using System.Collections;


public abstract class PlayerState 
{
    protected CustomCharacterController m_data;

    public PlayerState(CustomCharacterController _data)
    {
        m_data = _data;
    }

    public virtual IEnumerator OnStateEnter() { yield break; }
    public virtual IEnumerator  OnStateExit() { yield break; }
    public virtual IEnumerator  UpdateState() { yield break; }
}
