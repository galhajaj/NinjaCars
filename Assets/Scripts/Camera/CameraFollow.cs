using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public float interpVelocity;
	public float minDistance;
	public float followDistance;
	public Vector3 offset;

	private GameObject _target = null;
	// ================================================================================================ //
	void Start ()
	{

    }
    // ================================================================================================ //
    void LateUpdate () 
	{
        if (_target == null)
            _target = LocalPlayer.Instance.Get().gameObject;

        if (_target == null)
            return;

        Vector3 posNoZ = transform.position;
		posNoZ.z = _target.transform.position.z;

		Vector3 targetDirection = (_target.transform.position - posNoZ);

		interpVelocity = targetDirection.magnitude * 5f;

		Vector3 targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime); 

		transform.position = Vector3.Lerp( transform.position, targetPos + offset, 1.0F/*0.25f*/);
	}
	// ================================================================================================ //
}