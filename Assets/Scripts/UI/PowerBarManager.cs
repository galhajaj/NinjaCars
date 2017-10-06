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
        while (_powerUnitsList.Count <  Players.Instance.GetLocal().PowerData.MaxPower)
        {
            GameObject powerUnit = Instantiate(BarUnitObject);
            powerUnit.transform.SetParent(this.transform, false);
            _powerUnitsList.Add(powerUnit);
        }
        while (_powerUnitsList.Count > Players.Instance.GetLocal().LifeData.MaxLife)
        {
            GameObject powerUnit = _powerUnitsList[_powerUnitsList.Count - 1];
            powerUnit.transform.SetParent(null);
            _powerUnitsList.Remove(powerUnit);
            DestroyImmediate(powerUnit);
        }

        // fill bars
        for (int i = 0; i < _powerUnitsList.Count; ++i)
        {
            float ratio = 0.0F;

            if (Mathf.FloorToInt(Players.Instance.GetLocal().PowerData.Power) > i)
                ratio = 1.0F;
            else if (Mathf.FloorToInt(Players.Instance.GetLocal().PowerData.Power) == i)
                ratio = Players.Instance.GetLocal().PowerData.Power - Mathf.FloorToInt(Players.Instance.GetLocal().PowerData.Power);

            _powerUnitsList[i].transform.Find("Fill").transform.localScale = new Vector3(ratio, 1.0F, 1.0F);

        }
    }
}
