using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserMovement : NetworkBehaviour
{
    public KeyCode ForwardKey = KeyCode.UpArrow;
    public KeyCode BackwardKey = KeyCode.DownArrow;
    public KeyCode RightKey = KeyCode.RightArrow;
    public KeyCode LeftKey = KeyCode.LeftArrow;

    public float Thrust = 300.0F;
    public float ReverseThrust = 150.0F;
    public float SidewaysThrust = 300.0F;
    public float AngularThrust = 300.0F;

    private Rigidbody2D _rigidBody;
    private Tank _tankScript;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _tankScript = GetComponent<Tank>();
    }

	void Start ()
    {
        if (!isLocalPlayer)
            Destroy(this);
    }
	
    void FixedUpdate()
    {
        if (!_tankScript.IsActive)
            return;

        MoveForward();
        MoveBackward();
        /*MoveRight();
        MoveLeft();*/
        RotateRight();
        RotateLeft();
	}

    void MoveForward()
    {
        if (Input.GetKey(ForwardKey))
        {
            _rigidBody.AddForce(_rigidBody.transform.up * Thrust);
        }
    }
        
    void MoveBackward()
    {
        if (Input.GetKey(BackwardKey))
        {
            _rigidBody.AddForce(_rigidBody.transform.up * (-1) * ReverseThrust);
        }
    }

    /*void MoveRight()
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
    }*/

    void RotateLeft()
    {
        if (Input.GetKey(LeftKey))
        {
            _rigidBody.AddTorque(AngularThrust);
        }
    }

    void RotateRight()
    {
        if (Input.GetKey(RightKey))
        {
            _rigidBody.AddTorque(-AngularThrust);
        }
    }
}
