using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsBar : MonoBehaviour
{
    public static SkillsBar Instance;

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

    public GameObject ChipObject;

    void Start()
    {

    }

    void Update()
    {

    }
    // =====================================================================================================
    private void addRandomChips(int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            Transform nextFreeSocket = getNextFreeSocket();
            createRandomChip(nextFreeSocket);
        }
    }
    // =====================================================================================================
    private Transform getNextFreeSocket()
    {
        Transform freeSocket = null;

        foreach (Transform tile in this.transform)
        {
            if (tile.childCount == 0)
            {
                return freeSocket;
            }
        }

        return null;
    }
    // =====================================================================================================
    private void createRandomChip(Transform socket)
    {
        UnityEngine.Object[] allChips = Resources.LoadAll("Chips");
        int randomChipIndex = UnityEngine.Random.Range(0, allChips.Length);
        GameObject newChip = Instantiate(allChips[randomChipIndex] as GameObject);
        Chip chipScript = newChip.GetComponent<Chip>();
    }

    // =====================================================================================================
}
