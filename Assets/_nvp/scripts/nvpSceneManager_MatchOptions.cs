using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using newvisionsproject.managers.events;

public class nvpSceneManager_MatchOptions : MonoBehaviour {

	// +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++




	// +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void Start () {
		
	}
	
	void Update () {
		
	}




	// +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public void OnMatchMakerClicked(){
		Debug.Log("OnMatchMakerClicked  called");
		nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnStartMatchMakingRequested, this, null);
	}

	public void OnCreateMatchClicked(){
		Debug.Log("OnCreateMatchClicked  called");		
	}

	public void OnJoinMatchClicked(){
		Debug.Log("OnJoinMatchClicked  called");
	}



	// +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
