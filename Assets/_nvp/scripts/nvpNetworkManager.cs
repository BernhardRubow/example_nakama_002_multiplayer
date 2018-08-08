using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using newvisionsproject.managers.events;
using Nakama;

public class nvpNetworkManager : MonoBehaviour {

	// +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	private int _playerId;
	private IClient _client;
    private ISocket _socket;
    private IChannel _channel;
    private List<IUserPresence> _connectedUsers;
    private ISession _session;
    private string _id;
    private string _userName;
    private string _password;



	// +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void Start () {
		SubscribeToEvents();
	}

    void Destroy () {
		UnsubscribeFromEvents();
	}





    // +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    private void LoginPlayer(object s, object e)
    {
        Debug.Log("LoginPlayer called");
        _playerId = (int)e;
        this.LoadPlayerSettings();

        this.StartNakameClient();
    }




    // +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    private async void StartNakameClient(){
        // Init List of connected users
        _connectedUsers = new List<IUserPresence>(0);

        // get the nakama client
        _client = new Client("defaultkey", nvpGameManager.HOST, nvpGameManager.PORT, false);

        // create a session
        _session = await _client.AuthenticateEmailAsync(_userName, _password);

        nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnSessionCreated, this, null);
    }

    private void LoadPlayerSettings(){
        if(_playerId == 1){
            _userName = PlayerPrefs.GetString("Player1Email");
            _password = PlayerPrefs.GetString("Player1Password");            
        }
        else {
            _userName = PlayerPrefs.GetString("Player2Email");
            _password = PlayerPrefs.GetString("Player2Password");            
        }
    }

    private void SubscribeToEvents()
    {
        nvpEventManager.INSTANCE.SubscribeToEvent(GameEvents.OnLoginAsPlayerRequested, LoginPlayer);
    }

    private void UnsubscribeFromEvents()
    {
        nvpEventManager.INSTANCE.SubscribeToEvent(GameEvents.OnLoginAsPlayerRequested, LoginPlayer);
    }
}

