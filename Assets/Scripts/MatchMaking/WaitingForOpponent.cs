using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingForOpponent : MonoBehaviour {

    Text _loadingText;
    bool _isWaiting = false;
    float _waitingTime = 1;
	// Use this for initialization
	void Start () {
        _loadingText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (_isWaiting == false)
            return;
        
        _waitingTime -= Time.deltaTime;
        if (_waitingTime < 0)
        {
            _waitingTime = 1;
            _loadingText.text += ".";
        }
	}

    public void StartWaiting()
    {
        _loadingText.text = "Loading";
        _isWaiting = true;
    }

    public void StopWaiting()
    {
        _isWaiting = false;
        _loadingText.text = "";
    }
}
