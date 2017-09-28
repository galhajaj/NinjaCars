using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public float interpVelocity;
	public float minDistance;
	public float followDistance;
	public GameObject target;
	public Vector3 offset;
	Vector3 targetPos;

	// ================================================================================================ //
	void Start ()
	{
		targetPos = transform.position;
        if (target == null)
        {
            //TASK: asd
            //TODO: break dependency
            target = GameObject.Find("Tank(Clone)");
        }
	}
	// ================================================================================================ //
	void LateUpdate () 
	{
		if (target)
		{
			Vector3 posNoZ = transform.position;
			posNoZ.z = target.transform.position.z;

			Vector3 targetDirection = (target.transform.position - posNoZ);

			interpVelocity = targetDirection.magnitude * 5f;

			targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime); 

			transform.position = Vector3.Lerp( transform.position, targetPos + offset, 1.0F/*0.25f*/);

		}
	}
	// ================================================================================================ //
}