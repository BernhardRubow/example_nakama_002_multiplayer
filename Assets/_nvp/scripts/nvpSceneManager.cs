using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using newvisionsproject.managers.events;

public class nvpSceneManager : MonoBehaviour {

	// +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	private Stack<string> _scenes = new Stack<string>();



	// +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void Start () {
		SubscribeToEvents();
	}




	// +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void OnDestroy(){
		UnsubscribeFromEvents();
	}

	void OnGameInitialized(object s, object e){
		LoadSceneToStack("menuMain");
	}

	void OnEditPlayerSettingsRequested(object s, object e){
		int playerIndex = (int)e;
		if(playerIndex == 1){
			this.LoadSceneToStack("menuEditPlayer1Settings");
		}
		else{
			this.LoadSceneToStack("menuEditPlayer2Settings");
		}
	}

	void OnPlayerSettingsSaved(object s, object e){
		this.CloseCurrentScene();
	}




	// +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void LoadSceneToStack(string sceneName){

		if(_scenes.Count > 0) SceneManager.UnloadSceneAsync(_scenes.Peek());
		_scenes.Push(sceneName);
		SceneManager.LoadScene(_scenes.Peek(), LoadSceneMode.Additive);
	}

	void CloseCurrentScene(){
		var scene = _scenes.Pop();
		SceneManager.UnloadSceneAsync(scene);
		SceneManager.LoadScene(_scenes.Peek(), LoadSceneMode.Additive);
	}



	void SubscribeToEvents(){
		nvpEventManager.INSTANCE.SubscribeToEvent(GameEvents.OnGameInitialized, OnGameInitialized);
		nvpEventManager.INSTANCE.SubscribeToEvent(GameEvents.OnEditPlayerSettingsRequested, OnEditPlayerSettingsRequested);
		nvpEventManager.INSTANCE.SubscribeToEvent(GameEvents.OnPlayerSettingsSaved, OnPlayerSettingsSaved);

	}

	void UnsubscribeFromEvents(){
		nvpEventManager.INSTANCE.UnsubscribeFromEvent(GameEvents.OnGameInitialized, OnGameInitialized);
		nvpEventManager.INSTANCE.UnsubscribeFromEvent(GameEvents.OnEditPlayerSettingsRequested, OnEditPlayerSettingsRequested);
		nvpEventManager.INSTANCE.UnsubscribeFromEvent(GameEvents.OnPlayerSettingsSaved, OnPlayerSettingsSaved);
	}
}
