using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsBar : MonoBehaviour
{
    public static SkillsBar Instance;

    public int NumberOfChipsAtStart = 3;

    private List<UnityEngine.Object> _allChips = new List<UnityEngine.Object>();

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
    }

    void Update()
    {

    }
    // =====================================================================================================
    public void AddUniqueRandomChips(int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            Transform nextFreeSocket = getNextFreeSocket();
            createUniqueRandomChip(nextFreeSocket);
        }
    }
    // =====================================================================================================
    private Transform getNextFreeSocket()
    {
        foreach (Transform tile in this.transform)
        {
            if (tile.childCount == 0)
            {
                return tile;
            }
        }

        return null;
    }
    // =====================================================================================================
    private void createUniqueRandomChip(Transform socket)
    {
        if (_allChips.Count == 0)
        {
            Debug.LogError("No More Unique Chips...");
            return;
        }

        int randomChipIndex = UnityEngine.Random.Range(0, _allChips.Count);
        GameObject newChip = Instantiate(_allChips[randomChipIndex] as GameObject);
        _allChips.RemoveAt(randomChipIndex);
        newChip.transform.position = socket.position;
        newChip.transform.parent = socket;
    }

    // =====================================================================================================
}
