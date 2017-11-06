using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBarManager : MonoBehaviour
{
    public static SkillBarManager Instance;

    public int NumberOfChipsAtStart = 3;

    private List<UnityEngine.Object> _allChips = new List<UnityEngine.Object>();
    private List<GameObject> _skillUnitsList = new List<GameObject>();

    private int _currentSkillDigit = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
        // init chips array
        UnityEngine.Object[] allChipsArray = Resources.LoadAll("Chips");
        for (int i = 0; i < allChipsArray.Length; ++i)
            _allChips.Add(allChipsArray[i]);

        AddUniqueRandomChips(NumberOfChipsAtStart);

        AddChip("SkillBoost");
        //AddChip("SkillTeleport");
    }

    void Update()
    {
        updateSkillActivationInput();
    }
    // =====================================================================================================
    public void AddUniqueRandomChips(int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            createUniqueRandomChip();
        }
    }
    // =====================================================================================================
    public void AddChip(string name)
    {
        if (_allChips.Count == 0)
        {
            Debug.LogError("No Chips...");
            return;
        }

        GameObject newChip = Instantiate(Resources.Load("Chips/"+name) as GameObject);

        newChip.transform.SetParent(this.transform, false);
        _skillUnitsList.Add(newChip);

        Debug.Log("Create chip " + newChip.name);
    }
    // =====================================================================================================
    private void createUniqueRandomChip()
    {
        if (_allChips.Count == 0)
        {
            Debug.LogError("No More Unique Chips...");
            return;
        }

        int randomChipIndex = UnityEngine.Random.Range(0, _allChips.Count);
        GameObject newChip = Instantiate(_allChips[randomChipIndex] as GameObject);
        _allChips.RemoveAt(randomChipIndex);

        newChip.transform.SetParent(this.transform, false);
        _skillUnitsList.Add(newChip);

        Debug.Log("Create chip " + newChip.name);
    }

    // =====================================================================================================
    private void updateSkillActivationInput()
    {
        // skills (1 - 5)
        for (int i = 49, j = 0; i <= 53 && j < _skillUnitsList.Count; ++i, ++j)
        {
            Chip chipScript = _skillUnitsList[j].GetComponent<Chip>();

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
    // =====================================================================================================
    public int GetNextSkillDigit()
    {
        _currentSkillDigit++;
        return _currentSkillDigit;
    }
    // =====================================================================================================
}

