using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparks.Core;
using System.Security.Cryptography;

public class LogonManager : MonoBehaviour {
    enum LogonStatus
    {
        LS_logoff = 0,
        LS_logon = 1
    }
    private GameObject _gui;
    private InputField _username;
    private InputField _password;
    private Text _welcomeLabel;
    private Button _sndBtn;

    //private GameObject _mainPanel;
    LogonStatus _logonStatus = LogonStatus.LS_logoff;

    private string _loggedOnUser = "";
    private int _gamesPlayed = 0;
    private int _gamesLost = 0;
    private int _gamesWon = 0;
    private int _winningStreak = 0;
    private int _hstry_winningStreak = 0;

    private static LogonManager _instance = null;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }

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
                        if(_winningStreak>_hstry_winningStreak)
                        {
                            _hstry_winningStreak = _winningStreak;
                        }
                    }
                    else
                    {
                        ++_gamesLost;
                        _winningStreak = 0;
                    }
                }
            });
    }

	// Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(gameObject);
        Debug.Log("Start LogonManager");
        if (_logonStatus == LogonStatus.LS_logon)
            return;
        _gui = GameObject.Find("UserDetailsGui");
//        _mainPanel = GameObject.Find("MainPanel");
//        SetMainPanelActive(true); // TODO: GalHajaj change that to true 
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

        //load user
        CspParameters cspParams = new CspParameters();

        cspParams.KeyContainerName = "UsernameKey";  // This is the key used to encrypt and decrypt can be anything.
        var provider = new RSACryptoServiceProvider(cspParams);
        // Check if _RegString exsists, if not create it with an encrypted value of -1
        if (PlayerPrefs.GetString("_Username") == "")
        {
            ;
        }
        else
        {
            string decryptedUsername = System.Text.Encoding.UTF7.GetString(
                provider.Decrypt   (System.Convert.FromBase64String(PlayerPrefs.GetString("_Username")) , true));

            string decryptedPSWD = System.Text.Encoding.UTF7.GetString(
                provider.Decrypt   (System.Convert.FromBase64String(PlayerPrefs.GetString("_Password")) , true));

            _username.text = decryptedUsername;
            _password.text = decryptedPSWD;

            Logon();
                
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Logon()
    {
        Debug.Log("Logon function");
        _loggedOnUser = "";
        _gamesPlayed = 0;
        _gamesWon = 0;
        _gamesLost = 0;
        _winningStreak = 0;
        _hstry_winningStreak = 0;

        if (_username == null)
        {
            Debug.Log("username textbox is null");
        }
        //gamesparks has their own registeration and authentication. I'll use theirs in the furutre
        string tempUser = _username.text;
        string tempPassword = _password.text;

        if (tempUser == "")
        {
            return;
        }
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("Logon")
            .SetEventAttribute("Username", tempUser)
            .SetEventAttribute("Password", tempPassword)
            .Send((response) =>
                {
                    Debug.Log("Logon callback");
                    if (!response.HasErrors)
                    {
                        _logonStatus = LogonStatus.LS_logon;
                        Debug.Log("Logon attempt was made successfuly");
                        bool success = (bool)response.ScriptData.GetBoolean("Success");
                        if (success == true)
                        {
                            Debug.Log("Successful logon - GameSparks");
                            _loggedOnUser = tempUser;
                            _gamesPlayed = (int)response.ScriptData.GetInt("GamesPlayed");
                            _gamesWon = (int)response.ScriptData.GetInt("GamesWon");
                            _gamesLost = _gamesPlayed - _gamesWon;
                            _winningStreak = (int)response.ScriptData.GetInt("WinningStreak");
                            _hstry_winningStreak = (int)response.ScriptData.GetInt("Hstry_WinningStreak");

                            //encrypt username
                            CspParameters cspParams = new CspParameters();
                            cspParams.KeyContainerName = "UsernameKey";  // This is the key used to encrypt and decrypt can be anything.
                            var provider = new RSACryptoServiceProvider(cspParams);
                            byte[] encryptedBytes = provider.Encrypt(
                                System.Text.Encoding.UTF8.GetBytes(_username.text), true);
                            // convert to base64string first for storage as a string in the registry.
                            string encryptionString =  System.Convert.ToBase64String(encryptedBytes);
                            PlayerPrefs.SetString ("_Username", encryptionString);

                            byte[] encryptedBytes2 = provider.Encrypt(
                                System.Text.Encoding.UTF8.GetBytes(_password.text), true);
                            // convert to base64string first for storage as a string in the registry.
                            string encryptionString2 =  System.Convert.ToBase64String(encryptedBytes2);
                            PlayerPrefs.SetString ("_Password", encryptionString2);

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
//        _mainPanel.SetActive(active);
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
            _welcomeLabel.text += "Wins: " + _gamesWon.ToString() + '\n';
            _welcomeLabel.text += "Loses: " + _gamesLost.ToString() + '\n';
            _welcomeLabel.text += "Current Winning Streak: " + _winningStreak.ToString() + '\n';
            _welcomeLabel.text += "Historical Winning Streak: " + _hstry_winningStreak.ToString() + '\n';

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

    public string GetUsername()
    {
        return _loggedOnUser;
    }

}
