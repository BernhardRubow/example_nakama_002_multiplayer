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

	void OnEditPlayerSettingsRequested(object s, object e){
		int playerIndex = (int)e;
		if(playerIndex == 1){
			this.LoadScene("menuEditPlayer1Settings");

		}
		else{
			this.LoadScene("menuEditPlayer2Settings");
		}
	}

	void OnPlayerSettingsSaved(object s, object e){
		this.LoadScene("menuMain");
	}

    private void OnLoginAsPlayerRequested(object arg1, object arg2)
    {
        this.LoadScene("menuWaiting");
    }

    private void OnSessionCreated(object arg1, object arg2)
    {
        this.LoadScene("menuMatchOptions");
    }




	// +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void LoadScene(string sceneName){
		if(_currentScene != string.Empty) SceneManager.UnloadSceneAsync(_currentScene);
		SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
		_currentScene = sceneName;
	}



	void SubscribeToEvents(){
		nvpEventManager.INSTANCE.SubscribeToEvent(GameEvents.OnGameInitialized, OnGameInitialized);
		nvpEventManager.INSTANCE.SubscribeToEvent(GameEvents.OnEditPlayerSettingsRequested, OnEditPlayerSettingsRequested);
		nvpEventManager.INSTANCE.SubscribeToEvent(GameEvents.OnPlayerSettingsSaved, OnPlayerSettingsSaved);
		nvpEventManager.INSTANCE.SubscribeToEvent(GameEvents.OnLoginAsPlayerRequested, OnLoginAsPlayerRequested);
		nvpEventManager.INSTANCE.SubscribeToEvent(GameEvents.OnSessionCreated, OnSessionCreated);

	}

    void UnsubscribeFromEvents(){
		nvpEventManager.INSTANCE.UnsubscribeFromEvent(GameEvents.OnGameInitialized, OnGameInitialized);
		nvpEventManager.INSTANCE.UnsubscribeFromEvent(GameEvents.OnEditPlayerSettingsRequested, OnEditPlayerSettingsRequested);
		nvpEventManager.INSTANCE.UnsubscribeFromEvent(GameEvents.OnPlayerSettingsSaved, OnPlayerSettingsSaved);
		nvpEventManager.INSTANCE.UnsubscribeFromEvent(GameEvents.OnLoginAsPlayerRequested, OnLoginAsPlayerRequested);
		nvpEventManager.INSTANCE.UnsubscribeFromEvent(GameEvents.OnSessionCreated, OnSessionCreated);
	}
}
