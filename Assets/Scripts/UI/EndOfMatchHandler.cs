using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndOfMatchHandler : MonoBehaviour {

    bool _isGameOver = false;
    float _timeToLeave = 3;
	// Use this for initialization
	void Start () {
        GetComponent<Text>().text = "";
    }

	// Update is called once per frame
	void Update () {
        if (_isGameOver)
            _timeToLeave -= Time.deltaTime;

        if (_timeToLeave < 0)
        {
            _isGameOver = false;
            _timeToLeave = 3;
            GetComponent<Text>().text = "";
            SceneManager.LoadScene("mainSceneNew");
        }
	}

    public void EndOfMatch(bool won)
    {
        if (_isGameOver)
            return;
        if (won)
        {
            GetComponent<Text>().text = "YOU WON";
        }
        else
        {
            GetComponent<Text>().text = "YOU LOST";
        }
        _isGameOver = true;
    }
}
