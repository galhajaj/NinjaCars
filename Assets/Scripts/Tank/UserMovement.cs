using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserMovement : NetworkBehaviour
{
    public float Thrust = 300.0F;
    public float ReverseThrust = 150.0F;
    public float SidewaysThrust = 300.0F;
    public float AngularThrust = 300.0F;

    private Rigidbody2D _rigidBody;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

	void Start ()
    {
        if (!isLocalPlayer)
            Destroy(this);
    }
	
    void FixedUpdate()
    {
        /*if (!LocalPlayer.Instance.Get().IsActive)
            return;*/

        MoveForward();
        MoveBackward();
        MoveRight();
        MoveLeft();
        RotateRight();
        RotateLeft();
	}

    void MoveForward()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _rigidBody.AddForce(_rigidBody.transform.up * Thrust);
        }
    }
        
    void MoveBackward()
    {
        if (Input.GetKey(KeyCode.S))
        {
            _rigidBody.AddForce(_rigidBody.transform.up * (-1) * ReverseThrust);
        }
    }

    void MoveRight()
    {
        if (Input.GetKey(KeyCode.E))
        {
            _rigidBody.AddForce(_rigidBody.transform.right * SidewaysThrust);
        }
    }

    void MoveLeft()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            _rigidBody.AddForce(_rigidBody.transform.right * (-1) * SidewaysThrust);
        }
    }

    void RotateLeft()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _rigidBody.AddTorque(AngularThrust);
        }
    }

    void RotateRight()
    {
        if (Input.GetKey(KeyCode.D))
        {
            _rigidBody.AddTorque(-AngularThrust);
        }
    }
}
