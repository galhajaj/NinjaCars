using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInput : MonoBehaviour
{
    private List<GameObject> _skillsTiles = new List<GameObject>();
    private List<GameObject> _turretsTiles = new List<GameObject>();

    void Start ()
    {
        initTileLists();
    }
	
	void Update ()
    {
        updateSkillsKeysSelection();
        updateTurretsKeysSelection();
        updateTurretsScrollerSelection();
        updateSkillExecution();
    }

    private void initTileLists()
    {
        Transform skillsGrid = Inventory.Instance.SkillsGrid.transform;
        Transform turretsGrid = Inventory.Instance.TurretsGrid.transform;

        foreach (Transform tile in turretsGrid)
            _turretsTiles.Add(tile.gameObject);

        foreach (Transform tile in skillsGrid)
            _skillsTiles.Add(tile.gameObject);
        // move the last tile to be the first, because the numbers goes from 0(KeyCode 48) to 9(KeyCode 57)
        // and in keyboard, 0 is the last key (1 - 0)
        GameObject lastSkillTile = _skillsTiles[_skillsTiles.Count - 1];
        _skillsTiles.RemoveAt(_skillsTiles.Count - 1);
        _skillsTiles.Insert(0, lastSkillTile);
    }

    private void updateSkillsKeysSelection()
    {
        // skills (0 - 9)
        for (int i = 48, j = 0; i <= 57; ++i, ++j)
        {
            if (Input.GetKeyDown((KeyCode)i))
            {
                if (_skillsTiles[j].transform.childCount > 0)
                {
                    Chip chipScript = _skillsTiles[j].transform.GetChild(0).GetComponent<Chip>();
                    if (chipScript.Type == Chip.ChipType.STATE || chipScript.Type == Chip.ChipType.CONSUMABLE)
                    {
                        if (chipScript.IsActive)
                            chipScript.Deactivate();
                        else
                            chipScript.Activate();

                    }
                    else if (chipScript.Type == Chip.ChipType.SKILL)
                    {
                        Inventory.Instance.ActivateChipExclusively(chipScript);
                    }
                }
            }
        }
    }

    private void updateTurretsKeysSelection()
    {
        // turrets (F1 - F4)
        for (int i = 282, j = 0; i <= 285; ++i, ++j)
        {
            if (Input.GetKeyDown((KeyCode)i))
            {
                if (_turretsTiles[j].transform.childCount > 0)
                {
                    Chip chipScript = _turretsTiles[j].transform.GetChild(0).GetComponent<Chip>();
                    Inventory.Instance.ActivateChipExclusively(chipScript);
                }
            }
        }
    }

    private void updateTurretsScrollerSelection()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            bool isMoveForward = (Input.GetAxis("Mouse ScrollWheel") > 0f);

            Chip activeTurretChip = Inventory.Instance.GetActiveChip(Chip.ChipType.TURRET);
            if (activeTurretChip != null)
            {
                activeTurretChip.Deactivate();
                Chip nextChip = Inventory.Instance.GetNextChip(activeTurretChip, isMoveForward);
                Inventory.Instance.ActivateChipExclusively(nextChip);
            }
        }
    }

    private void updateSkillExecution()
    {
        Chip chipScript = Inventory.Instance.GetActiveChip(Chip.ChipType.SKILL);
        if (chipScript == null)
            return;

        // execute start
        if (Input.GetMouseButtonDown(1))
        {
            chipScript.ExecuteStart();
        }
        // execute continues
        if (Input.GetMouseButton(1))
        {
            chipScript.ExecuteContinues();
        }
        // execute end
        if (Input.GetMouseButtonUp(1))
        {
            chipScript.ExecuteEnd();
        }
    }
}
