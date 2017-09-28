using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMapOnEsc : MonoBehaviour
{
    void Start()
    {
        Debug.LogWarning("Remember to remove script BackToMapOnEsc from Manager in battle scene - or not...");
    }

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("mainScene");
        }
	}
}
