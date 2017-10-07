using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Tank : NetworkBehaviour
{
    public PowerUser PowerData;
    public UserMovement MovementData;
    public UserShooting AmmoData;

    public bool IsActive = true;

    void Awake()
    {

    }

    void Start ()
    {
        
    }
	
	void Update ()
    {
		
	}

    public void SetPosition(float x, float y)
    {
        transform.position = new Vector3(x, y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "JadeTag")
        {
            PowerData.Power++;
            NetworkServer.Destroy(other.gameObject);
        }
    }
}
