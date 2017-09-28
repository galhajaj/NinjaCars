using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
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

    public enum PackType
    {
        REGULAR,
        PREMIUM,
        SUPER
    }

    public GameObject ChipObject;
    public int ChipsNumberInPack = 5;
    public int RegularPackCost = 100;
    public int PremiumPackCost = 250;
    public int SuperPackCost = 500;

    public const string INVENTORY_GRID_NAME = "Grid_Inventory";
    public const string SKILLS_GRID_NAME = "Grid_Skills";
    public const string TURRETS_GRID_NAME = "Grid_Turrets";

    private GameObject _inventoryGrid;
    public GameObject InventoryGrid { get { return _inventoryGrid; } }
    private GameObject _skillsGrid;
    public GameObject SkillsGrid { get { return _skillsGrid; } }
    private GameObject _turretsGrid;
    public GameObject TurretsGrid { get { return _turretsGrid; } }

    void Start()
    {
        _inventoryGrid = GameObject.Find(INVENTORY_GRID_NAME);
        _skillsGrid = GameObject.Find(SKILLS_GRID_NAME);
        _turretsGrid = GameObject.Find(TURRETS_GRID_NAME);
    }

    void Update()
    {

    }
    // =====================================================================================================
    private int getCost(PackType packType)
    {
        if (packType == PackType.REGULAR)
            return RegularPackCost;
        if (packType == PackType.PREMIUM)
            return PremiumPackCost;
        if (packType == PackType.SUPER)
            return SuperPackCost;
        return 0;
    }
    // =====================================================================================================
    public void BuyPack(PackType packType)
    {
        // check if can afford
        int cost = getCost(packType);
        if (DataManager.Instance.Saved.Credits < cost)
            return;

        // check free sockets
        List<Transform> freeSockets = getFreeSocketsForChipsPack();
        if (freeSockets == null)
            return;

        // pay
        DataManager.Instance.Saved.Credits -= cost;

        // add chips
        for (int i = 0; i < freeSockets.Count; ++i)
            createRandomChip(freeSockets[i]);

        // save to file
        DataManager.Instance.SaveDataToFile();
    }
    // =====================================================================================================
    public void PutOnChip(GameObject chip, Transform socket)
    {
        chip.transform.position = socket.position;
        chip.transform.parent = socket;

        Chip chipScript = chip.GetComponent<Chip>();
        chipScript.SocketName = socket.name;
        chipScript.GridName = socket.parent.name;

        if (chipScript.GridName != "Grid_Inventory")
        {
            chipScript.Install();
            if (chipScript.Type == Chip.ChipType.TURRET || chipScript.Type == Chip.ChipType.SKILL)
            {
                ActivateChipExclusively(chipScript);
            }
            /*else if (chipScript.Type == Chip.ChipType.STATE)
            {
                chipScript.Activate();
            }*/
        }
    }
    // =====================================================================================================
    public void PutOffChip(GameObject chip)
    {
        Chip chipScript = chip.GetComponent<Chip>();

        if (chipScript.GridName != "Grid_Inventory")
        {
            chipScript.Uninstall();
            if (chipScript.Type == Chip.ChipType.TURRET || chipScript.Type == Chip.ChipType.SKILL)
            {
                if (chipScript.IsActive)
                {
                    chipScript.Deactivate();
                    Chip nextChip = GetNextChip(chipScript);
                    if (nextChip != chipScript)
                        ActivateChipExclusively(nextChip);
                }
            }
            else if (chipScript.Type == Chip.ChipType.STATE)
            {
                if (chipScript.IsActive)
                    chipScript.Deactivate();
            }
        }
    }
    // =====================================================================================================
    private void createRandomChip(Transform socket)
    {
        UnityEngine.Object[] allChips = Resources.LoadAll("Chips");
        int randomChipIndex = UnityEngine.Random.Range(0, allChips.Length);
        GameObject newChip = Instantiate(allChips[randomChipIndex] as GameObject);
        Chip chipScript = newChip.GetComponent<Chip>();
        chipScript.ChipID = DataManager.Instance.GetNextChipID();
        PutOnChip(newChip, socket);
    }
    // =====================================================================================================
    private void createChipByData(ChipData chipData)
    {
        // get chip object to create from resources
        GameObject chipObjectToCreate = null;
        UnityEngine.Object[] allChips = Resources.LoadAll("Chips");
        foreach(GameObject chipObj in allChips)
        {
            if (chipObj.name == chipData.PrefabName)
                chipObjectToCreate = chipObj;
        }
        if (chipObjectToCreate == null)
            return;

        // create
        GameObject newChip = Instantiate(chipObjectToCreate);

        // get chips socket
        GameObject grid = GameObject.Find(chipData.GridName);
        Transform socket = grid.transform.Find(chipData.SocketName);

        Chip chipScript = newChip.GetComponent<Chip>();
        chipScript.ChipID = chipData.ChipID;
        PutOnChip(newChip, socket);
    }
    // =====================================================================================================
    public void FillChipsListFromDataManager()
    {
        foreach (ChipData chipData in DataManager.Instance.Saved.ChipsData)
        {
            createChipByData(chipData);
        }
    }
    // =====================================================================================================
    private List<Transform> getFreeSocketsForChipsPack()
    {
        List<Transform> freeSockets = new List<Transform>();

        foreach (Transform tile in _inventoryGrid.transform)
        {
            if (tile.childCount == 0)
            {
                freeSockets.Add(tile);
                if (freeSockets.Count >= ChipsNumberInPack)
                    return freeSockets;
            }
        }

        return null;
    }
    // =====================================================================================================
    // activate chip and deactivate all Others at the same type
    public void ActivateChipExclusively(Chip chip)
    {
        // return if already active
        if (chip.IsActive)
            return;

        // deactivate all the rest with the same type
        Transform grid = chip.transform.parent.parent;
        foreach (Transform tileItem in grid)
        {
            if (tileItem.childCount > 0)
            {
                Chip chipInTileScript = tileItem.GetChild(0).GetComponent<Chip>();
                if (chipInTileScript.IsActive && chipInTileScript.Type == chip.Type)
                    chipInTileScript.Deactivate();
            }
        }

        // activate chosen
        chip.Activate();
    }
    // =====================================================================================================
    // return the next/previous chip at the same type in grid
    public Chip GetNextChip(Chip currentChip, bool isForward = true)
    {
        // get relevant grid
        Transform grid = currentChip.transform.parent.parent;

        // create list of all sockets with chips in chosen type
        List<Chip> chips = new List<Chip>();
        foreach (Transform tile in grid)
        {
            if (tile.childCount > 0)
            {
                Chip chipScript = tile.GetChild(0).GetComponent<Chip>();
                if (chipScript.Type == currentChip.Type)
                    chips.Add(chipScript);
            }
        }

        // if only one - return it
        if (chips.Count == 1)
            return currentChip;

        // make the current to be the first. for convenience at the last stage...
        Chip firstChip = chips[0];
        while (firstChip != currentChip)
        {
            chips.Remove(firstChip);
            chips.Add(firstChip);
            firstChip = chips[0];
        }

        // return the next chip
        if (isForward)
            return chips[1];
        else
            return chips[chips.Count - 1];
    }
    // =====================================================================================================
    public Chip GetActiveChip(Chip.ChipType chipType)
    {
        Transform grid = (chipType == Chip.ChipType.SKILL) ? SkillsGrid.transform : TurretsGrid.transform;
        foreach (Transform tile in grid)
        {
            if (tile.childCount > 0)
            {
                Chip chipScript = tile.GetChild(0).GetComponent<Chip>();
                if (chipScript.Type == chipType && chipScript.IsActive)
                    return chipScript;
            }
        }

        return null;
    }
    // =====================================================================================================
}
