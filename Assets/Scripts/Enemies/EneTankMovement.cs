using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EneTankMovement : MonoBehaviour {

	public GameObject Ammo;

//	private float ShootingForce = 4f;
//	private float speed = 6f;

	private Rigidbody2D m_blueTank;
	private Rigidbody2D m_localRigidbody;

	private Vector2 m_force;
//	private float lastShootingTime = 0f;

	private Vector2 direction;
	// Use this for initialization
	void Start () {
//		m_blueTank = GameObject.Find ("Tank").GetComponent<Rigidbody2D>();
		m_localRigidbody = this.GetComponent<Rigidbody2D> ();
		m_localRigidbody.position.Set(Random.Range(0, 100), (Random.Range(0, 100)));
		
		m_force = new Vector2(Random.Range(-30,30),Random.Range(-30,30));
	}
	
	// Update is called once per frame
	void Update()
	{
		m_localRigidbody.AddForce (m_force);
	}
}
