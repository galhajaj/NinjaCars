using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

	void Start ()
    {
		
	}
	    
	void Update ()
    {
        this.GetComponent<Text>().text = MatchParams.Instance.HostPlayerScore.ToString() + " - " + MatchParams.Instance.VisitorPlayerScore.ToString();
    }
}
