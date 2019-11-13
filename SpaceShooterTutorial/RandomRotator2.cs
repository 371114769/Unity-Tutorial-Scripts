using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator2 : MonoBehaviour {

    public float tumbler;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = Random.insideUnitSphere * tumbler;
    }
}
