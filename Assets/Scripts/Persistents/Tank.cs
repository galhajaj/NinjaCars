using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public static Tank Instance;

    public Damagable LifeData;
    public PowerUser PowerData;
    public UserMovement MovementData;

    public bool IsActive = true;

    private GameObject _currentTurret = null;

    private Quaternion _lastTurretDir = Quaternion.identity;
    public Quaternion LastTurretDir
    {
        get { return _lastTurretDir; }
        set { _lastTurretDir = value; }
    }

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    void Start ()
    {
        
    }
	
	void Update ()
    {
		
	}

    public void PutOnTurret(GameObject turretGameObject)
    {
        _currentTurret = Instantiate(turretGameObject, transform.position, transform.rotation);
        _currentTurret.transform.parent = this.transform;

        // init the dir to be as the last turret
        _currentTurret.transform.rotation = LastTurretDir;
    }

    public void PutOffTurret()
    {
        if (_currentTurret == null)
            return;
        _currentTurret.transform.parent = null;
        DestroyImmediate(_currentTurret.gameObject);
    }

    public void SetPosition(float x, float y)
    {
        transform.position = new Vector3(x, y);
    }
}
