using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryByBoundary2 : MonoBehaviour
{
    public GameObject other;

    void OnTriggerExit(Collider other)
    {
        // Destroy everything that leaves the trigger
        Destroy(other.gameObject);
    }
}
