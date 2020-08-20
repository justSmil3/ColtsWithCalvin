using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharControll : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        if (!TryGetComponent<Rigidbody>(out rb))
            Destroy(this);
    }
}
