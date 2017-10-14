using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMapOnEsc : MonoBehaviour
{
    bool odd = true;

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

        if (Input.GetKeyDown(KeyCode.V))
        {
            odd = !odd;
            if (odd)
            {
                GameObject.Find("UserDetailsInfo").GetComponent<LogonManager>().UpdateMatchResult(true);
            }
                else
                GameObject.Find("UserDetailsInfo").GetComponent<LogonManager>().UpdateMatchResult(false);
        }
	}
}
