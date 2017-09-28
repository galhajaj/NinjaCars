using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    void Awake()
    {
        string currentCountry = PlayerPrefs.GetString("CurrentLocation");
        Debug.Log("We're in " + currentCountry);
    }

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
}
