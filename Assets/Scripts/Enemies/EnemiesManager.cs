using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour {

	public GameObject EnemyObject;

	private float timeToGenerate = 0f;
	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		timeToGenerate -= Time.deltaTime;
		if (timeToGenerate > 0) {
			return;
		}

		float posX = Random.Range (0, 10);
		float posY = Random.Range (0, 10);

        /*GameObject enemy = (GameObject)*/Instantiate (EnemyObject, new Vector2 (posX, posY), new Quaternion ());
		timeToGenerate = Random.Range (3, 5);
	}
}
