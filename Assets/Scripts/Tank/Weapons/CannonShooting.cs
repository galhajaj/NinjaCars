using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShooting : MonoBehaviour
{
    public bool ContinuousShooting = false;
    public float LoadingTime = 1.0f;
    public float ShootingForce = 50.0f;
    public float MaxDistance = 12f;

    public GameObject Nuzzle1;
    public GameObject Nuzzle2;
    public GameObject Nuzzle3;
    public GameObject Nuzzle4;
    public GameObject Nuzzle5;
    public GameObject Nuzzle6;
    public GameObject Nuzzle7;

    public bool SimultaneouslyShooting = false;

    public GameObject Ammo;

    private List<Transform> _nuzzlesList;  //should be a vector or array. I'll change it later on.
    private int _nextNuzzle = 0;

    void Awake()
    {
        _nuzzlesList = new List<Transform>();
    }

    //void AddNuzzle()

	void Start ()
    {
        for (int i = 1; i < 8; ++i)
        {
            Transform nuzzleTransform = this.transform.Find("Nuzzle" + i.ToString());
            if (nuzzleTransform != null)
            {
                _nuzzlesList.Add(nuzzleTransform);
                ++_nextNuzzle;
            }
        }

        if (_nuzzlesList.Count == 0)
        {
            Debug.LogWarning("nuzzles 0");
            _nextNuzzle = -1;
        }

        if (Ammo == null)
        {
            //Ammo = Resources.Load(defaultShell);
        }
	}

	// Update is called once per frame
    void Update ()
    {
        if (!Tank.Instance.IsActive)
            return;

        if (ShouldShoot())
        {
            Fire();
        }
    }

    bool ShouldShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }
        else if(ContinuousShooting && Input.GetMouseButton(0))
        {
            return true;
        }

        return false;
    }

    void Fire()
    {
        if (SimultaneouslyShooting)
        {
            foreach (Transform nuzzle in _nuzzlesList)
            {
                FireSingleShot(nuzzle);
            }
        }
        else
        {
            if (_nextNuzzle == -1)
            {
                return;
            }

            if (_nextNuzzle == _nuzzlesList.Count)
            {
                _nextNuzzle = 0;
            }

            FireSingleShot(_nuzzlesList[_nextNuzzle]);
            ++_nextNuzzle;
        }
    }

    void FireSingleShot(Transform Nuzzle)
    {
        //GameObject
        GameObject shell = (GameObject)Instantiate (Ammo, Nuzzle.position, Nuzzle.rotation);
        Physics2D.IgnoreCollision (shell.GetComponent<Collider2D> (), this.GetComponentInParent<Collider2D> ());
        Vector2 force = shell.transform.up * ShootingForce;
        shell.GetComponent<Rigidbody2D> ().AddForce (force);
        Destroy (shell, 20);//find a good destruction formula

    }
}
