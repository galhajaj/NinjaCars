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
            // TODO: this is not the place... the inventory goes into the camera to make it follow it
            // but if stays there... the inventory deleted while moving to another scene
            // so, after remove its parent - need also to make again the DontDestroyOnLoad on it
            Inventory.Instance.transform.SetParent(null);
            DontDestroyOnLoad(Inventory.Instance.gameObject);

            SceneManager.LoadScene("headquartersScene");
        }
	}
}
