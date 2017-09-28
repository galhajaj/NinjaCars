using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour
{
    private Text _creditsText;

	void Start ()
    {
        _creditsText = this.gameObject.GetComponent<Text>();
	}
	
	void Update ()
    {

        _creditsText.text = "Credits: " + DataManager.Instance.Saved.Credits.ToString();
    }
}
