using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nvpRocketLauncher : MonoBehaviour, IMissleLauncher
{

    // +++ public fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++    
    // +++ editor fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField] private GameObject _rocketPrefab;
	[SerializeField] private GameObject _rocketSpawnPoint;
	[SerializeField] private bool _isLocalPlayerScript;




    // +++ private fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    private bool _fireMissile;
    // +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void Update()
    {
        if (!_fireMissile) return;
        _fireMissile = false;
        var rocket = Instantiate(
            _rocketPrefab,
            _rocketSpawnPoint.transform.position,
            _rocketSpawnPoint.transform.rotation
        );
    }
    // +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++




    // +++ interface methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public void LaunchMissle(Vector3 directionOfLaunch)
    {
        _fireMissile = true;
    }
}

