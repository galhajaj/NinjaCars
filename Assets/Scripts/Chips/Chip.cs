using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Chip : MonoBehaviour
{
    public enum ChipType
    {
        PASSIVE,
        ACTIVE
    }

    public GameObject FillChildObj;
    public GameObject FrameChildObj;
    public GameObject IconChildObj;
    public GameObject CostChildObj;
    private Transform _fillTransform;
    private Image _fillImage;
    private Transform _icon;
    private Transform _frame;
    private Transform _cost;
    private Text _costText;

    public ChipType Type = ChipType.ACTIVE;

    private bool _isExecuted = false;

    public int Cost = 0;
    //public float CostPerSecond = 0.0F;

    public float CooldownTime = 1.0F;
    private float _currentCooldownTime = 0.0F;

    public int RequiredAmmo = 0;

    public Sprite IconPic;

    private Color _NonActiveColor = new Color(0.15F, 0.15F, 0.15F);

    void Awake()
    {

    }

    void Start()
    {
        _fillTransform = Instantiate(FillChildObj).transform;
        _fillTransform.SetParent(this.transform, false);
        _fillImage = _fillTransform.GetComponent<Image>();

        _frame = Instantiate(FrameChildObj).transform;
        _frame.SetParent(this.transform, false);

        _icon = Instantiate(IconChildObj).transform;
        _icon.SetParent(this.transform, false);
        setIcon(IconPic);

        _cost = Instantiate(CostChildObj).transform;
        _costText = _cost.Find("Text").GetComponent<Text>();
        _costText.text = Cost.ToString();
        _cost.SetParent(this.transform, false);

        changeColorByType();
    }
	
	void Update()
    {
        if (_currentCooldownTime > 0.0F)
            _currentCooldownTime -= Time.deltaTime;
        /*if (_currentCooldownTime < 0.0F)
            _currentCooldownTime = 0.0F;*/

        updateCooldownUI();
    }

    protected void updateCostGui()
    {
        _costText.text = Cost.ToString();
    }

    protected void setIcon(Sprite icon)
    {
        _icon.GetComponent<Image>().sprite = icon;
    }

    private void changeColorByType()
    {
        if (Type == ChipType.PASSIVE)
        {
            _fillImage.color = Color.grey;
        }
        else if (Type == ChipType.ACTIVE)
        {
            _fillImage.color = Color.green;
        }
    }

    // =====================================================================================================
    // every function below contains protected virtual function to ovveride (if needded) in inherited, and 
    // it called while the public called
    // =====================================================================================================
    public void ExecuteStart()
    {
        if (_isExecuted)
            return;

        if (_currentCooldownTime > 0.0F)
            return;

        _isExecuted = true;
        if (Players.Instance.GetLocal().PowerData.Power < Cost)
            return;

        if (Players.Instance.GetLocal().AmmoData.AmmoCount < RequiredAmmo)
            return;

        Players.Instance.GetLocal().PowerData.Power -= Cost;
        _currentCooldownTime = CooldownTime;
        executeStart();
    }
    protected virtual void executeStart() { }
    // =====================================================================================================
    /*public void ExecuteContinues()
    {
        if (!_isExecuted)
            return;

        float calculatedCost = Time.deltaTime * CostPerSecond;
        if (Players.Instance.GetLocal().PowerData.Power < calculatedCost)
        {
            ExecuteEnd();
            return;
        }
        Players.Instance.GetLocal().PowerData.Power -= calculatedCost;
        executeContinues();
    }
    protected virtual void executeContinues() { }*/
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
    private void updateCooldownUI()
    {
        if (Type == ChipType.PASSIVE)
            return;

        float ratio = 1.0F - _currentCooldownTime / CooldownTime;
        if (ratio > 1.0F)
            ratio = 1.0F;

        _fillTransform.localScale = new Vector3(1.0F, ratio);

        if (_currentCooldownTime <= 0.0F && 
            Players.Instance.GetLocal().PowerData.Power >= Cost && 
            Players.Instance.GetLocal().AmmoData.AmmoCount >= RequiredAmmo)
        {
            _fillImage.color = Color.green;
        }
        else
        {
            _fillImage.color = _NonActiveColor;
        }
    }
}
