using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float SpinRate = 100;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * SpinRate, Space.World);
    }
}
