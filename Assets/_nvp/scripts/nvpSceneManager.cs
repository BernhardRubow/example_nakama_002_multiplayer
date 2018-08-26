using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using newvisionsproject.managers.events;
using System;

public class nvpSceneManager : MonoBehaviour {

	// +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	private string _currentScene = "";



	// +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void Start () {
		SubscribeToEvents();
	}




	// +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void OnDestroy(){
		UnsubscribeFromEvents();
	}

	void OnGameInitialized(object s, object e){
		LoadScene("menuMain");
	}




	// +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public void LoadScene(string sceneName){
		if(_currentScene != string.Empty) SceneManager.UnloadSceneAsync(_currentScene);
		SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
		_currentScene = sceneName;
	}

	public void LoadPlayerSettings(int playerId){
		if(playerId == 1){
			this.LoadScene("menuEditPlayer1Settings");

		}
		else{
			this.LoadScene("menuEditPlayer2Settings");
		}
	}

	void SubscribeToEvents(){
		nvpEventManager.INSTANCE.Subscribe(GameEvents.OnGameInitialized, OnGameInitialized);

	}

    void UnsubscribeFromEvents(){
		nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnGameInitialized, OnGameInitialized);
	}
}
