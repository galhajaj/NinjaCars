using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChipData
{
    public int ChipID;
    public string PrefabName;
    public string GridName;
    public string SocketName;
    public bool IsActive;
}

public abstract class Chip : MonoBehaviour
{
    public enum ChipType
    {
        TURRET,
        CONST,
        STATE,
        CONSUMABLE,
        SKILL,
        NONE
    }

    public ChipType Type;

    public int ChipID;
    public string GridName; // in which grid contained - inventory/turrets/skills
    public string SocketName; // the socket name inside grid

    private bool _isInstalled = false;
    public bool IsInstalled { get { return _isInstalled; } }

    private bool _isActive = false;
    public bool IsActive { get { return _isActive; } }

    private bool _isExecuted = false;

    public int Cost = 0;
    public float CostPerSecond = 0.0F;

    public Sprite IconPic;
    private GameObject _iconObject;
    private SpriteRenderer _iconSpriteRenderer;

    public GameObject TurretObject;

    void Awake()
    {
        changeColorByType();
    }

    void Start()
    {
        addIconChildObject();
    }
	
	void Update()
    {
        updateIconColor();
    }

    private void addIconChildObject()
    {
        _iconObject = new GameObject();
        _iconObject.name = "Icon";
        _iconSpriteRenderer = _iconObject.AddComponent<SpriteRenderer>();
        _iconSpriteRenderer.sprite = IconPic;
        _iconSpriteRenderer.sortingLayerName = "ChipsSortingLayer";
        _iconSpriteRenderer.sortingOrder = 1; // the chip body is 0, need to be in front of it
        _iconSpriteRenderer.color = Color.black;
        _iconObject.transform.position = this.transform.position;
        _iconObject.transform.parent = this.transform;
    }

    private void updateIconColor()
    {
        if (_iconObject == null)
            return;

        if (Type == ChipType.CONST)
        {
            if (IsInstalled)
                _iconSpriteRenderer.color = Color.white;
            else
                _iconSpriteRenderer.color = Color.black;

        }
        else
        {
            if (IsActive)
                _iconSpriteRenderer.color = Color.white;
            else
                _iconSpriteRenderer.color = Color.black;
        }
    }

    public ChipData GetChipData()
    {
        ChipData data = new ChipData();
        data.ChipID = this.ChipID;
        data.PrefabName = this.name.Replace("(Clone)", "");
        data.GridName = this.GridName;
        data.SocketName = this.SocketName;
        return data;
    }

    private void changeColorByType()
    {
        string type = this.gameObject.name.Split('_')[1];
        switch (type)
        {
            case "Turret":
                Type = ChipType.TURRET;
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case "Const":
                Type = ChipType.CONST;
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case "State":
                Type = ChipType.STATE;
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case "Consumable":
                Type = ChipType.CONSUMABLE;
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case "Skill":
                Type = ChipType.SKILL;
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
                break;
            default:
                Type = ChipType.NONE;
                break;
        }
    }

    // =====================================================================================================
    // every function below contains protected virtual function to ovveride (if needded) in inherited, and 
    // it called while the public called
    // =====================================================================================================
    public void Install()
    {
        if (_isInstalled)
            return;

        _isInstalled = true;
        install();
    }
    protected virtual void install() { }
    // =====================================================================================================
    public void Uninstall()
    {
        if (!_isInstalled)
            return;

        _isInstalled = false;
        uninstall();
    }
    protected virtual void uninstall() { }
    // =====================================================================================================
    public void Activate()
    {
        if (_isActive)
            return;

        _isActive = true;
        if (TurretObject != null)
            Tank.Instance.PutOnTurret(TurretObject);
        activate();

        // destroy consumable after activation
        if (Type == ChipType.CONSUMABLE)
        {
            this.transform.SetParent(null);
            DestroyImmediate(this.gameObject);
            DataManager.Instance.SaveDataToFile();
        }
    }
    protected virtual void activate() { }
    // =====================================================================================================
    public void Deactivate()
    {
        if (!_isActive)
            return;

        _isActive = false;
        if (TurretObject != null)
            Tank.Instance.PutOffTurret();
        deactivate();
    }
    protected virtual void deactivate() { }
    // =====================================================================================================
    public void ExecuteStart()
    {
        if (_isExecuted)
            return;

        _isExecuted = true;
        if (Tank.Instance.PowerData.Power < Cost)
            return;
        Tank.Instance.PowerData.Power -= Cost;
        executeStart();
    }
    protected virtual void executeStart() { }
    // =====================================================================================================
    public void ExecuteContinues()
    {
        if (!_isExecuted)
            return;

        float calculatedCost = Time.deltaTime * CostPerSecond;
        if (Tank.Instance.PowerData.Power < calculatedCost)
        {
            ExecuteEnd();
            return;
        }
        Tank.Instance.PowerData.Power -= calculatedCost;
        executeContinues();
    }
    protected virtual void executeContinues() { }
    // =====================================================================================================
    public void ExecuteEnd()
    {
        if (!_isExecuted)
            return;

        _isExecuted = false;
        executeEnd();
    }
    protected virtual void executeEnd() { }
    // =====================================================================================================
}
