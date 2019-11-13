using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveManeuver2 : MonoBehaviour {

    public float dodge;
    public float smoothing;
    public Vector2 startWait;
    public Vector2 maneuverTime;
    public Vector2 maneuverWait;
    public Boundary boundary;
    public float tilt;

    private float currentspeed;
    private float targetManeuver;
    private Rigidbody rb;

    void Start () 
    {
        rb = GetComponent<Rigidbody>();
        currentspeed = rb.velocity.z;
        StartCoroutine(Evade());
        Debug.Log(currentspeed);
	}
	
    IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));

        while (true)
        {
            targetManeuver = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x);
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
            targetManeuver = 0;
            yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
        }
    }


	void FixedUpdate () 
    {
        float newMenuver = Mathf.MoveTowards(rb.velocity.x, targetManeuver, Time.deltaTime * smoothing);
        rb.velocity = new Vector3(newMenuver, 0.0f, currentspeed);
        rb.position = new Vector3
            (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
            );
        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }
}
