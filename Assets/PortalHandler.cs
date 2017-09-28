using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalHandler : MonoBehaviour {
    private enum PortalState
    {
        PS_Unlocked,
        PS_Locked
    }

    //I thought of doing it using animation. But then it seemed stupid. Or maybe two different sprites\textures?
    public Sprite[] TwoStatesSprite;
    public float LockingTime = 20f;
    private float lockTime = 0f;
    private PortalState state = PortalState.PS_Locked;

	// Use this for initialization
	void Start ()
    {
        this.GetComponent<SpriteRenderer>().sprite = TwoStatesSprite[0];
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (state == PortalState.PS_Locked && lockTime + Time.deltaTime > LockingTime)
        {
            state = PortalState.PS_Unlocked;
            this.GetComponent<SpriteRenderer>().sprite = TwoStatesSprite[1];
        }
        else
        {
            lockTime += Time.deltaTime;
        }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (state == PortalState.PS_Unlocked && coll.gameObject == Tank.Instance.gameObject)
        {
            //TODO: Should be part of some Scenesloaer
            //Destroy(gameObject);
//            Inventory.Instance.transform.SetParent(null);
//            DontDestroyOnLoad(Inventory.Instance.gameObject);
            UnityEngine.SceneManagement.SceneManager.LoadScene("headquartersScene");
        }
    }
}
