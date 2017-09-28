using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public static Tank Instance;

    public Damagable LifeData;
    public PowerUser PowerData;
    public UserMovement MovementData;

    public bool IsActive = true;

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

    public void SetPosition(float x, float y)
    {
        transform.position = new Vector3(x, y);
    }
}
