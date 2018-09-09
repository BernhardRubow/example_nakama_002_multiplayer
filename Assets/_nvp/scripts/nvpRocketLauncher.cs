using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nvpRocketLauncher : MonoBehaviour, IMissleLauncher, ILocalPlayerAwareScript
{

    // +++ public fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++    
    // +++ editor fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField] private GameObject _rocketPrefab;
	[SerializeField] private GameObject _rocketSpawnPoint;
	[SerializeField] private bool _isLocalPlayerScript;



    // +++ private fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void Update(){
		if(!_isLocalPlayerScript) return;
		if (Input.GetKeyUp(KeyCode.F)){
			LaunchMissle(this.transform.forward);
		}
	}
    // +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++




    // +++ interface methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public void LaunchMissle(Vector3 directionOfLaunch)
    {
        var rocket = Instantiate(
            _rocketPrefab,
            _rocketSpawnPoint.transform.position,
            _rocketSpawnPoint.transform.rotation
        );
    }

    public void SetIsLocalPlayer()
    {
        _isLocalPlayerScript = true;
    }
}

