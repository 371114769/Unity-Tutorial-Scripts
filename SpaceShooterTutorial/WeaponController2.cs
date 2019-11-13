using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController2 : MonoBehaviour {

    public GameObject shot;
    public Transform shotSpawn;

    public float delay;
    public float fireRate;

    private AudioSource AudioSource;

    // Use this for initialization
    void Start () {
        AudioSource = GetComponent<AudioSource>();
        InvokeRepeating("Fire", delay, fireRate);
	}
	
	void Fire () {
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        AudioSource.Play();
	}
}
