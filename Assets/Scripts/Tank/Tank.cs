using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Tank : NetworkBehaviour
{
    public PowerUser PowerData;
    public UserMovement MovementData;
    public UserShooting AmmoData;
    public GameObject ShieldObj;

    public bool IsActive = true;

    void Awake()
    {

    }

    void Start ()
    {
        // change layer to local one
        if (isLocalPlayer)
        {
            this.gameObject.layer = LayerMask.NameToLayer("TankLocalLayer");
        }
    }
	
	void Update ()
    {
		
	}

    public void SetPosition(float x, float y)
    {
        transform.position = new Vector3(x, y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "JadeTag")
        {
            PowerData.Power++;
            NetworkServer.Destroy(other.gameObject);
        }
    }

    [Command]
    public void CmdAddShield()
    {
        RpcAddShieldOnClient();
    }

    [ClientRpc]
    private void RpcAddShieldOnClient()
    {
        GameObject shield = Instantiate(ShieldObj, this.transform.position, Quaternion.identity) as GameObject;
        int shieldCount = this.transform.childCount;
        float newScale = shield.transform.localScale.x * (1.0F + 0.1F * (float)shieldCount);
        shield.transform.localScale = new Vector3(newScale, newScale);
        shield.transform.SetParent(this.transform);
    }

    // ########################### TODO: try to put those functions here instead of inside UserShooting
    [Command]
    public void CmdRemoveShield(GameObject obj)
    {
        RpcRemoveShieldOnClient(obj);
    }

    [ClientRpc]
    private void RpcRemoveShieldOnClient(GameObject obj)
    {
        int shieldCount = obj.transform.childCount;
        if (shieldCount <= 0)
            return;

        Transform shieldToRemove = obj.transform.GetChild(shieldCount - 1);
        shieldToRemove.transform.SetParent(null);
        Destroy(shieldToRemove.gameObject);
    }
    // ###########################
}
