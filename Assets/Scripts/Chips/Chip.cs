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

    public ChipType Type = ChipType.ACTIVE;

    private bool _isExecuted = false;

    public int Cost = 0;
    //public float CostPerSecond = 0.0F;

    public float CooldownTime = 1.0F;
    private float _currentCooldownTime = 0.0F;

    public Sprite IconPic;
    //private GameObject _iconObject;
    //private SpriteRenderer _iconSpriteRenderer;
    private Transform _fillTransform;
    private Image _fillImage;

    private Color _NonActiveColor = new Color(0.15F, 0.15F, 0.15F);

    void Awake()
    {

    }

    void Start()
    {
        _fillTransform = this.transform.Find("Fill");
        _fillImage = _fillTransform.GetComponent<Image>();
        this.transform.Find("Icon").GetComponent<Image>().sprite = IconPic;
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

    private void changeColorByType()
    {
        if (Type == ChipType.PASSIVE)
        {
            _fillTransform.GetComponent<Image>().color = Color.grey;
        }
        else if (Type == ChipType.ACTIVE)
        {
            _fillTransform.GetComponent<Image>().color = Color.green;
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

        if (_currentCooldownTime <= 0.0F && Players.Instance.GetLocal().PowerData.Power >= Cost)
        {
            _fillImage.color = Color.green;
        }
        else
        {
            _fillImage.color = _NonActiveColor;
        }
    }
}
