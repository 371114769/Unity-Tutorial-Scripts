using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller2 : MonoBehaviour {

    public float scrollspeed;
    public float tileSizeZ;

    private Vector3 startposition;

    void Start()
    {
        startposition = transform.position;
    }

    // Update is called once per frame
    void Update () {
        float newPosition = Mathf.Repeat(Time.time * scrollspeed, tileSizeZ);
        transform.position =  startposition + Vector3.forward * newPosition;
		
	}
}
