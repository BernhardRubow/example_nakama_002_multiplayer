using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using newvisionsproject.managers.events;
using System;

public class nvpSceneManager_MatchMaking : MonoBehaviour {

	// +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[SerializeField] private GameObject _sceneUI;
	private nvpNetworkManager _networkManager;
	private nvpSceneManager _sceneManager;


	// +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	async void Start () {
		Init();

		SubscribeToEvents();

		// Start Matchmaking		
		await _networkManager.CreateMatchMakerMatchAsync();
		Debug.Log("Match matched");
		_sceneUI.SetActive(true);
	}

    void Update () {
		
	}

	void OnDestroy(){
		UnsubscribeFromEvents();
	}




    // +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public async void OnJoinMatchClicked(){
		await _networkManager.JoinMatchAsync();

		_sceneManager.LoadScene("gameMain");
	}

    private void OnNakama_SocketConnected(object arg1, object arg2)
    {
        Debug.Log("Socket Connected");
    }

    private void OnNakama_SocketDisconnected(object arg1, object arg2)
    {
        Debug.Log("Socket Disconnected");
    }




    // +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	private void Init(){
		// get references
		_networkManager = GameObject.Find("managers").GetComponent<nvpNetworkManager>();
		_sceneManager = GameObject.Find("managers").GetComponent<nvpSceneManager>();
	}

    private void SubscribeToEvents()
    {
		nvpEventManager.INSTANCE.Subscribe(GameEvents.OnNakama_SocketConnected, OnNakama_SocketConnected);
		nvpEventManager.INSTANCE.Subscribe(GameEvents.OnNakama_SocketDisconnected, OnNakama_SocketDisconnected);
    }

    private void UnsubscribeFromEvents()
    {
        nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnNakama_SocketConnected, OnNakama_SocketConnected);
		nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnNakama_SocketDisconnected, OnNakama_SocketDisconnected);
    }
}