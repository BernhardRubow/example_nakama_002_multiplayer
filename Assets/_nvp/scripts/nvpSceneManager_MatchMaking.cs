using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using newvisionsproject.managers.events;
using System;

public class nvpSceneManager_MatchMaking : MonoBehaviour {

	// +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[SerializeField] private GameObject _sceneUI;

	private List<Action> _deferedActions = new List<Action>();

	// +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void Start () {
		SubscribeToEvents();

		// Start Matchmaking
		nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnMakeMatchRequested, this, null);
		
	}

    void Update () {
		if(_deferedActions.Count > 0){
			foreach(var a in _deferedActions) a();
			_deferedActions.Clear();
		}
	}

	void OnDestroy(){
		UnsubscribeFromEvents();
	}




    // +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public void OnJoinMatchClicked(){
		nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnJoinMatchRequested, this, null);
	}

    private void OnNakama_SocketConnected(object arg1, object arg2)
    {
        Debug.Log("Socket Connected");
    }

    private void OnNakama_SocketDisconnected(object arg1, object arg2)
    {
        Debug.Log("Socket Disconnected");
    }

    private void OnNakama_MatchFound(object arg1, object arg2)
    {
		_deferedActions.Add(() => _sceneUI.SetActive(true));
    }




    // +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    private void SubscribeToEvents()
    {
		nvpEventManager.INSTANCE.Subscribe(GameEvents.OnNakama_SocketConnected, OnNakama_SocketConnected);
		nvpEventManager.INSTANCE.Subscribe(GameEvents.OnNakama_SocketDisconnected, OnNakama_SocketDisconnected);
		nvpEventManager.INSTANCE.Subscribe(GameEvents.OnNakama_MatchFound, OnNakama_MatchFound);
    }

    private void UnsubscribeFromEvents()
    {
        nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnNakama_SocketConnected, OnNakama_SocketConnected);
		nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnNakama_SocketDisconnected, OnNakama_SocketDisconnected);
		nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnNakama_MatchFound, OnNakama_MatchFound);
    }
}