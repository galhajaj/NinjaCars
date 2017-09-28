using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportToLocationOnClick : MonoBehaviour
{
	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    void OnMouseDown()
    {
        PlayerPrefs.SetString("CurrentLocation", this.gameObject.name);
        SceneManager.LoadScene("battlegroundScene");
    }
}
