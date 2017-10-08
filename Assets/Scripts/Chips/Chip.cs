using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Sprite IconPic;
    private GameObject _iconObject;
    private SpriteRenderer _iconSpriteRenderer;

    void Awake()
    {

    }

    void Start()
    {
        changeColorByType();
        addIconChildObject();
    }
	
	void Update()
    {

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

    private void changeColorByType()
    {
        if (Type == ChipType.PASSIVE)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (Type == ChipType.ACTIVE)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
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

        _isExecuted = true;
        if (Players.Instance.GetLocal().PowerData.Power < Cost)
            return;
        Players.Instance.GetLocal().PowerData.Power -= Cost;
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
}
