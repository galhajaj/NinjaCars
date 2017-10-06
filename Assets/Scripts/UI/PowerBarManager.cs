using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBarManager : MonoBehaviour
{
    public GameObject BarUnitObject;

    private List<GameObject> _powerUnitsList = new List<GameObject>();

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
        while (_powerUnitsList.Count <  Players.Instance.GetLocal().PowerData.Power)
        {
            GameObject powerUnit = Instantiate(BarUnitObject);
            powerUnit.transform.SetParent(this.transform, false);
            _powerUnitsList.Add(powerUnit);
        }
        while (_powerUnitsList.Count > Players.Instance.GetLocal().PowerData.Power)
        {
            GameObject powerUnit = _powerUnitsList[_powerUnitsList.Count - 1];
            powerUnit.transform.SetParent(null);
            _powerUnitsList.Remove(powerUnit);
            DestroyImmediate(powerUnit);
        }
    }
}
