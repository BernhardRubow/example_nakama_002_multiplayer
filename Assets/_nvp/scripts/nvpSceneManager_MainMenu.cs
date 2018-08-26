using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using newvisionsproject.managers.events;

public class nvpSceneManager_MainMenu : MonoBehaviour {

	// +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	private nvpNetworkManager _networkManager;
	private nvpSceneManager _sceneManager;



	// +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void Start () {
		// Get references
		_networkManager = GameObject.Find("managers").GetComponent<nvpNetworkManager>();
		_sceneManager = GameObject.Find("managers").GetComponent<nvpSceneManager>();
	}
	
	void Update () {
		
	}




	// +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public async void OnLoginAsPlayerClicked(int playerId){
		Debug.Log("OnLoginAsPlayerClicked called");

		// invoke event to show waiting screen
		_sceneManager.LoadScene("menuWaiting");

		// Wait for the networkmanger to connect the player
		await _networkManager.LoginPlayerAsync(playerId);

		// then invoke event to open match options
		_sceneManager.LoadScene("menuMatchOptions");
	}

	public void OnEditPlayerSettings(int playerId){
		Debug.Log("OnEditPlayerSettings called");

		// invoke event to open player settings scenen
		_sceneManager.LoadPlayerSettings(playerId);
	}



	// +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

}
