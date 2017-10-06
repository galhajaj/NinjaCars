using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoorikanBarManager : MonoBehaviour
{
    public GameObject BarUnitObject;

    private List<GameObject> _ammoUnitsList = new List<GameObject>();

    void Start()
    {

    }

    void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (Players.Instance.GetLocal() == null)
            return;

        // update bar numbers
        while (_ammoUnitsList.Count < Players.Instance.GetLocal().AmmoData.AmmoCount)
        {
            GameObject ammoUnit = Instantiate(BarUnitObject);
            ammoUnit.transform.SetParent(this.transform, false);
            _ammoUnitsList.Add(ammoUnit);
        }
        while (_ammoUnitsList.Count > Players.Instance.GetLocal().AmmoData.AmmoCount)
        {
            GameObject ammoUnit = _ammoUnitsList[_ammoUnitsList.Count - 1];
            ammoUnit.transform.SetParent(null);
            _ammoUnitsList.Remove(ammoUnit);
            DestroyImmediate(ammoUnit);
        }

        // fill bars
        /*for (int i = 0; i < _lifeUnitsList.Count; ++i)
        {
            float ratio = 0.0F;

            if (Mathf.FloorToInt(Players.Instance.GetLocal().LifeData.Life) > i)
                ratio = 1.0F;
            else if (Mathf.FloorToInt(Players.Instance.GetLocal().LifeData.Life) == i)
                ratio = Players.Instance.GetLocal().LifeData.Life - Mathf.FloorToInt(Players.Instance.GetLocal().LifeData.Life);

            _lifeUnitsList[i].transform.Find("Fill").transform.localScale = new Vector3(ratio, 1.0F, 1.0F);
        }*/
    }
}
