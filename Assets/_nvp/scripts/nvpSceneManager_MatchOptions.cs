using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using newvisionsproject.managers.events;

public class nvpSceneManager_MatchOptions : MonoBehaviour {

	// +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	nvpSceneManager _sceneManager;



	// +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void Start () {
		Init();
	}
	
	void Update () {
		
	}




	// +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public void OnMatchMakerClicked(){
		Debug.Log("OnMatchMakerClicked  called");

		_sceneManager.LoadScene("menuMatchMaking");
	}

	public void OnCreateMatchClicked(){
		Debug.Log("OnCreateMatchClicked  called");		
	}

	public void OnJoinMatchClicked(){
		Debug.Log("OnJoinMatchClicked  called");
	}



	// +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	private void Init(){
		// get references
		_sceneManager = GameObject.Find("managers").GetComponent<nvpSceneManager>();
	}
}
