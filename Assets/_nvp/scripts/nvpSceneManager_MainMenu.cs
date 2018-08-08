using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using newvisionsproject.managers.events;

public class nvpSceneManager_MainMenu : MonoBehaviour {

	// +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++




	// +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void Start () {
		
	}
	
	void Update () {
		
	}




	// +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public void OnLoginAsPlayerClicked(int playerId){
		Debug.Log("OnLoginAsPlayerClicked called");
		nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnLoginAsPlayerRequested, this, playerId);
	}

	public void OnEditPlayerSettings(int playerId){
		Debug.Log("OnEditPlayerSettings called");
		nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnEditPlayerSettingsRequested, this, playerId);
	}



	// +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

}
