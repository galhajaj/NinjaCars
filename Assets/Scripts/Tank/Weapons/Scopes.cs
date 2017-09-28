using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class Scopes : MonoBehaviour {

	public Texture2D closeScope;
	public Texture2D farScope;
	private Vector2 hotSpot = Vector2.zero;
	private Transform m_transform;

    private float _maxDistance = 0;
	// Use this for initialization
	void Start () {
		m_transform = this.GetComponent<Transform> ();
        _maxDistance = this.GetComponent<CannonShooting>().MaxDistance;
    }
	
	// Update is called once per frame
	void Update () {
		//decide of the scope type based on the distance from the blue tank
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = -(m_transform.position.x - Camera.main.transform.position.x);
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
		float distance = Mathf.Sqrt (Mathf.Pow (worldPos.x - m_transform.position.x, 2) + Mathf.Pow (worldPos.y - m_transform.position.y, 2));

        if (distance < _maxDistance) {
			setCurser (closeScope);
		} else {
			setCurser (farScope);
		}
	}

	void setCurser(Texture2D cursor)
	{
		if (cursor) {
			//cursor points to the center of the scope
			hotSpot.x = cursor.width / 2;
			hotSpot.y = cursor.height / 2;
		} else {
			hotSpot = Vector2.zero;
		}
		Cursor.SetCursor (cursor, hotSpot, CursorMode.ForceSoftware);
	}

	void OnDestroy()
	{
		//back to regular cursor
		setCurser (null);
	}
}
