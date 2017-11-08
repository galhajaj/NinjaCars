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

    void Start ()
    {
        _randDistanceFromCenterX = Random.Range(MinDistance, MaxDistance);
        _randDistanceFromCenterY = Random.Range(MinDistance, MaxDistance);
        _speed = Random.Range(MinSpeed, MaxSpeed);
        transform.position = transform.parent.position + new Vector3(_randDistanceFromCenterX, _randDistanceFromCenterY, 0.0F);
    }
	
	void Update ()
    {
        transform.RotateAround(transform.parent.position, Quaternion.Euler(0, 90, 0) * Vector3.right, _speed);
    }
}
