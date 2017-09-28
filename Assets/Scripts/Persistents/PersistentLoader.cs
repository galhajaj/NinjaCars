using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentLoader : MonoBehaviour
{
    public GameObject DataManagerObj;
    public GameObject TankObj;
    public GameObject InventoryObj;

    void Awake()
    {
        // destroy persistents in main scene
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "mainScene")
        {
            if (DataManager.Instance != null)
                DestroyImmediate(DataManager.Instance.gameObject);
            if (Tank.Instance != null)
                DestroyImmediate(Tank.Instance.gameObject);
            if (Inventory.Instance != null)
                DestroyImmediate(Inventory.Instance.gameObject);
        }

        // init persistents
        if (DataManager.Instance == null)
            Instantiate(DataManagerObj);
        if (Tank.Instance == null)
            Instantiate(TankObj);
        if (Inventory.Instance == null)
            Instantiate(InventoryObj);

        // hide/show elements by scene name
        if (scene.name == "mainScene" || scene.name == "headquartersScene")
        {
            Tank.Instance.IsActive = false;
            Tank.Instance.SetPosition(5.0F, 0.0F);
            Inventory.Instance.transform.Find("Grid_Inventory").gameObject.SetActive(true);
            Inventory.Instance.transform.localScale = new Vector3(1.0F, 1.0F, 1.0F);
            Inventory.Instance.transform.position = new Vector3(100.0F, 100.0F, -10.0F);
        }
        else if (scene.name == "garageScene")
        {
            Tank.Instance.IsActive = false;
            Tank.Instance.SetPosition(5.0F, 0.0F);
            Inventory.Instance.transform.Find("Grid_Inventory").gameObject.SetActive(true);
            Inventory.Instance.transform.localScale = new Vector3(1.0F, 1.0F, 1.0F);
            Inventory.Instance.transform.position = new Vector3(0.0F, -2.0F, -10.0F);
        }
        else if (scene.name == "battlegroundScene")
        {
            Tank.Instance.IsActive = true;
            Tank.Instance.SetPosition(5.0F, 0.0F);
            Inventory.Instance.transform.localScale = new Vector3(2.0F, 2.0F, 1.0F);
            Inventory.Instance.transform.position = new Vector3(0.0F, -13.0F, -10.0F);
            Inventory.Instance.transform.SetParent(Camera.main.transform);
            Inventory.Instance.transform.Find("Grid_Inventory").gameObject.SetActive(false);
        }
    }

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
}
