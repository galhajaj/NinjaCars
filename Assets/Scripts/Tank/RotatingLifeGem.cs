using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingLifeGem : MonoBehaviour
{
    private float _randDistanceFromCenterX;
    private float _randDistanceFromCenterY;
    public float MinDistance = 1.25F;
    public float MaxDistance = 3.5F;
    private float _speed;
    public float MinSpeed = 5F;
    public float MaxSpeed = 10F;
    //Quaternion _originalRotation;
    int _rotationDirection;


    void Awake()
    {
        //_originalRotation = transform.rotation;
        _rotationDirection = (Random.Range(0, 2) == 0) ? 1 : -1;
    }

    void Start ()
    {
        _randDistanceFromCenterX = Random.Range(MinDistance, MaxDistance);
        _randDistanceFromCenterY = Random.Range(MinDistance, MaxDistance);
        _speed = Random.Range(MinSpeed, MaxSpeed);
        transform.position = transform.parent.position + new Vector3(_randDistanceFromCenterX, _randDistanceFromCenterY, 0.0F);
    }
	
	void Update ()
    {
        transform.RotateAround(transform.parent.position, Quaternion.Euler(0, 90, 0) * Vector3.right * _rotationDirection, _speed);
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
        //transform.rotation = _originalRotation;
    }
}
