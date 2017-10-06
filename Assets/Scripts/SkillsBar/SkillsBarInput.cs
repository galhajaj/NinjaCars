using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsBarInput : MonoBehaviour
{
    private List<GameObject> _skillsTiles = new List<GameObject>();

    void Start ()
    {
        initTileLists();
    }
	
	void Update ()
    {
        updateSkillsKeysSelection();
    }

    private void initTileLists()
    {
        foreach (Transform tile in this.transform)
            _skillsTiles.Add(tile.gameObject);
    }

    private void updateSkillsKeysSelection()
    {
        // skills (1 - 5)
        for (int i = 49, j = 0; i <= 53; ++i, ++j)
        {
            Transform tileTransform = _skillsTiles[j].transform;
            if (tileTransform.childCount <= 0)
                continue;
            Chip chipScript = tileTransform.GetChild(0).GetComponent<Chip>();

            if (Input.GetKeyDown((KeyCode)i))
            {
                chipScript.ExecuteStart();
            }

            /*if (Input.GetKey((KeyCode)i))
            {
                chipScript.ExecuteContinues();
            }*/

            if (Input.GetKeyUp((KeyCode)i))
            {
                chipScript.ExecuteEnd();
            }
        }
    }
}
