using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparks.Core;

public class LogonManager : MonoBehaviour {
    private GameObject _gui;
    private InputField _username;
    private InputField _password;
    private Text _welcomeLabel;
    private Button _sndBtn;

    private GameObject _mainPanel;

    private string _loggedOnUser = "";
    private int _gamesPlayed = 0;
    private int _gamesWon = 0;
    private int _winningStreak = 0;

    //split into two different classes. one for results and one for communication with the server\logon
    public void UpdateMatchResult(bool won)
    {
        int _i_didWin = 0;
        if (won)
            _i_didWin = 1;
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("UPD_RES")
            .SetEventAttribute("Username", _loggedOnUser)
            .SetEventAttribute("DidWin", _i_didWin)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    Debug.Log("Winning update attempt was made successfuly");

                    ++_gamesPlayed;
                    if (won)
                    {
                        ++_gamesWon;
                        ++_winningStreak;
                    }
                    else
                    {
                        _winningStreak = 0;
                    }
                }
            });
    }

	// Use this for initialization
	void Start ()
    {
        _gui = GameObject.Find("UserDetailsGui");
        _mainPanel = GameObject.Find("MainPanel");
        SetMainPanelActive(false);
        foreach (InputField inpfld in _gui.GetComponentsInChildren<InputField>())
        {
            if (inpfld.name == "UsernameInput")
            {
                _username = inpfld;
            }
            if (inpfld.name == "PasswordInput")
            {
                _password = inpfld;
            }
        }


        
        foreach (Text txt in _gui.GetComponentsInChildren<Text>())
        {
            if (txt.name == "WelcomeLabel")
            {
                _welcomeLabel = txt;
            }
        }

        _sndBtn = _gui.GetComponentInChildren<Button>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Logon()
    {
        _loggedOnUser = "";
        _gamesPlayed = 0;
        _gamesWon = 0;
        _winningStreak = 0;

        //gamesparks has their own registeration and authentication. I'll use theirs in the furutre
        string tempUser = _username.text;
        string tempPassword = _password.text;
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("Logon")
            .SetEventAttribute("Username", tempUser)
            .SetEventAttribute("Password", tempPassword)
            .Send((response) =>
                {
                    if (!response.HasErrors)
                    {
                        Debug.Log("Logon attempt was made successfuly");
                        bool success = (bool)response.ScriptData.GetBoolean("Success");
                        if (success == true)
                        {
                            Debug.Log("SUCCESS");
                            _loggedOnUser = tempUser;
                            _gamesPlayed = (int)response.ScriptData.GetInt("GamesPlayed");
                            _gamesWon = (int)response.ScriptData.GetInt("GamesWon");
                            _winningStreak = (int)response.ScriptData.GetInt("WinningStreak");
                        }
                        else
                        {
                            Debug.Log("FAILED");
                        }
                    }
                    else
                    {
                        Debug.Log("Error logging");
                    }
                    UpdateGui();
                });
    }

    void SetMainPanelActive(bool active)
    {
        _mainPanel.SetActive(active);
    }

    public void UpdateGui(bool visible = true)
    {
        if (!visible)
        {
            _gui.SetActive(false);
            return;
        }

        if (_loggedOnUser != "")
        {
            _welcomeLabel.text = "Hello " + _loggedOnUser + '\n';
            _welcomeLabel.text += "Games: " + _gamesPlayed.ToString() + '\n';
            _welcomeLabel.text += "Wins: " + _gamesWon.ToString() + '\n';
            _welcomeLabel.text += "WinningStreak: " + _winningStreak.ToString() + '\n';

            _sndBtn.image.color = Color.green;

            SetMainPanelActive(true);
        }
        else
        {
            _welcomeLabel.text = "You are not connected";
            _sndBtn.image.color = Color.red;
            SetMainPanelActive(false);
        }

        _gui.SetActive(true);
    }
}
