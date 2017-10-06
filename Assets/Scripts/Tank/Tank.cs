using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
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
}
