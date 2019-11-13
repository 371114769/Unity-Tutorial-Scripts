using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController2 : MonoBehaviour {

    public float speed;
    public float tilt;
    public Boundary boundary;

    public GameObject shot;
    public Transform ShotSpawn;
    public float fireRate;

    private float nextFire;
    private Rigidbody rb;
    private AudioSource AudioSource;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        AudioSource = GetComponent<AudioSource>();
	}

    private void Update()
    {

        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, ShotSpawn.position, ShotSpawn.rotation);// as GameObject;
            AudioSource.Play();
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * speed;

        rb.position = new Vector3
            (
                Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
                0,
                Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
            );
        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }
}
