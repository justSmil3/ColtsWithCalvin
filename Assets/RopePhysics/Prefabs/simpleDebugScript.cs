using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleDebugScript : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        Debug.LogError(this.name);
    }
}
