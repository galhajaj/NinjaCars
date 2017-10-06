using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserColor : NetworkBehaviour
{

	void Start ()
    {
        if (!isLocalPlayer)
            Destroy(this);

        if (isLocalPlayer)
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        else
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }
	
	void Update ()
    {
		
	}
}
