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

    private void OnLoginAsPlayerRequested(object s, object e)
    {
        this.LoadScene("menuWaiting");
    }

    private void OnSessionCreated(object s, object e)
    {
        this.LoadScene("menuMatchOptions");
    }

	private void OnStartMatchMakingRequested(object s, object e)
	{
		this.LoadScene("menuMatchMaking");
	}

    private void OnNakama_MatchStarted(object arg1, object arg2)
    {
        this.LoadScene("gameMain");
    }




	// +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void LoadScene(string sceneName){
		if(_currentScene != string.Empty) SceneManager.UnloadSceneAsync(_currentScene);
		SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
		_currentScene = sceneName;
	}



	void SubscribeToEvents(){
		nvpEventManager.INSTANCE.Subscribe(GameEvents.OnGameInitialized, OnGameInitialized);
		nvpEventManager.INSTANCE.Subscribe(GameEvents.OnEditPlayerSettingsRequested, OnEditPlayerSettingsRequested);
		nvpEventManager.INSTANCE.Subscribe(GameEvents.OnPlayerSettingsSaved, OnPlayerSettingsSaved);
		nvpEventManager.INSTANCE.Subscribe(GameEvents.OnLoginAsPlayerRequested, OnLoginAsPlayerRequested);
		nvpEventManager.INSTANCE.Subscribe(GameEvents.OnNakama_SessionCreated, OnSessionCreated);
		nvpEventManager.INSTANCE.Subscribe(GameEvents.OnStartMatchMakingRequested, OnStartMatchMakingRequested);
		nvpEventManager.INSTANCE.Subscribe(GameEvents.OnNakama_MatchStarted, OnNakama_MatchStarted);

	}

    void UnsubscribeFromEvents(){
		nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnGameInitialized, OnGameInitialized);
		nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnEditPlayerSettingsRequested, OnEditPlayerSettingsRequested);
		nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnPlayerSettingsSaved, OnPlayerSettingsSaved);
		nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnLoginAsPlayerRequested, OnLoginAsPlayerRequested);
		nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnNakama_SessionCreated, OnSessionCreated);
		nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnStartMatchMakingRequested, OnStartMatchMakingRequested);
		nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnNakama_MatchStarted, OnNakama_MatchStarted);
	}
}
