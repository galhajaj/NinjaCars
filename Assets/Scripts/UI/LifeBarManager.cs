using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarManager : MonoBehaviour
{
    public GameObject BarUnitObject;

    private List<GameObject> _lifeUnitsList = new List<GameObject>();

	void Start ()
    {

	}
	
	void Update ()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        // update bar numbers
        while (_lifeUnitsList.Count < Tank.Instance.LifeData.MaxLife)
        {
            GameObject lifeUnit = Instantiate(BarUnitObject);
            lifeUnit.transform.SetParent(this.transform, false);
            _lifeUnitsList.Add(lifeUnit);
        }
        while (_lifeUnitsList.Count > Tank.Instance.LifeData.MaxLife)
        {
            GameObject lifeUnit = _lifeUnitsList[_lifeUnitsList.Count - 1];
            lifeUnit.transform.SetParent(null);
            _lifeUnitsList.Remove(lifeUnit);
            DestroyImmediate(lifeUnit);
        }

        // fill bars
        for (int i = 0; i < _lifeUnitsList.Count; ++i)
        {
            float ratio = 0.0F;

            if (Mathf.FloorToInt(Tank.Instance.LifeData.Life) > i)
                ratio = 1.0F;
            else if (Mathf.FloorToInt(Tank.Instance.LifeData.Life) == i)
                ratio = Tank.Instance.LifeData.Life - Mathf.FloorToInt(Tank.Instance.LifeData.Life);

            _lifeUnitsList[i].transform.Find("Fill").transform.localScale = new Vector3(ratio, 1.0F, 1.0F);
        }
    }
}
