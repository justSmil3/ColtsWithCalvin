
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerState_InAir : PlayerState
{
    public PlayerState_InAir(CustomCharacterController _data) : base(_data)
    {
    }
    public override IEnumerator OnStateEnter()
    {
        yield return null;
    }
}
