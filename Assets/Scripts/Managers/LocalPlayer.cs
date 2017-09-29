using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalPlayer : MonoBehaviour
{
    public static LocalPlayer Instance;

    void Awake()
    {
        if (Instance == null)
        {
            //DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public Tank Get()
    {
        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (cur.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                return cur.GetComponent<Tank>();
            }
        }

        return null;
    }
}
