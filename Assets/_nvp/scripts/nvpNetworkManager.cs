using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using newvisionsproject.managers.events;
using Nakama;
using Nakama.TinyJson;

public class nvpNetworkManager : MonoBehaviour {

	// +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	private int _playerId;
	private IClient _client;
    private ISocket _socket;
    private IChannel _channel;
    private List<IUserPresence> _connectedUsers;
    private ISession _session;
    private IUserPresence _self;
    private IMatchmakerMatched _matchMakerMatch;
    private IMatchmakerTicket _matchMakerTicket;
    private IMatch _match;

    private string _id;
    private string _userName;
    private string _password;
    private int _minPlayers = 2;
    private int _maxPlayers = 4;



	// +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void Start () {
		SubscribeToEvents();
	}

    void Destroy () {
		UnsubscribeFromEvents();
	}




    // +++ nakama event handler +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    private async void OnMatchmakerMatched(object s, IMatchmakerMatched matched){

        _matchMakerMatch = matched;
        nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnNakama_MatchFound, this, _matchMakerMatch);
    }

    private void OnConnect(object sender, EventArgs e){
        nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnNakama_SocketConnected, this, _socket);
    }

    private void OnDisconnect(object sender, EventArgs e){
        nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnNakama_SocketDisconnected, this, null);
    }

    private void OnMatchPresence(object s, IMatchPresenceEvent presenceChange){
        _connectedUsers.AddRange(presenceChange.Joins);
        foreach (var leave in presenceChange.Leaves)
        {
            _connectedUsers.RemoveAll(item => item.SessionId.Equals(leave.SessionId));
        };
        // Remove ourself from connected opponents.
        _connectedUsers.RemoveAll(item => {
            return _self != null && item.SessionId.Equals(_self.SessionId);
        });
    }

    private void OnMatchState(object s, IMatchState msm){

    }



    // +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    private void LoginPlayer(object s, object e)
    {
        Debug.Log("LoginPlayer called");
        _playerId = (int)e;
        this.LoadPlayerSettings();

        this.StartNakameClient();
    }

    private async void OnMakeMatchRequested(object s, object e)
    {
        // Init List of connected users
        _connectedUsers = new List<IUserPresence>(0);

        // create socket
        _socket = _client.CreateWebSocket();

        _socket.OnMatchmakerMatched += OnMatchmakerMatched;
        _socket.OnConnect += OnConnect;
        _socket.OnDisconnect += OnDisconnect;
        _socket.OnMatchPresence += OnMatchPresence;
        _socket.OnMatchState += OnMatchState;

        // wait for socket connection
        await _socket.ConnectAsync(_session);

        // MatchMaker Parameters
        var query = "*";

        _matchMakerTicket = await _socket.AddMatchmakerAsync(query, _minPlayers, _maxPlayers);

        nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnNakama_MatchMakerTicketReceived, this, _matchMakerTicket);

    }

    private async void OnJoinMatchRequested(object arg1, object arg2)
    {
        Debug.Log("awaiting match");
        // await joining match
        _match = await _socket.JoinMatchAsync(_matchMakerMatch);
        Debug.Log("match joined");

        // persisting own presence
        _self = _match.Self;
        _connectedUsers.AddRange(_match.Presences);

        nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnNakama_MatchStarted, this, _match);   
    }




    // +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    private async void StartNakameClient(){
        
        // get the nakama client
        _client = new Client("defaultkey", nvpGameManager.HOST, nvpGameManager.PORT, false);

        // create a session
        _session = await _client.AuthenticateEmailAsync(_userName, _password);
        nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnNakama_SessionCreated, this, _session);
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
        nvpEventManager.INSTANCE.Subscribe(GameEvents.OnLoginAsPlayerRequested, LoginPlayer);
        nvpEventManager.INSTANCE.Subscribe(GameEvents.OnMakeMatchRequested, OnMakeMatchRequested);
        nvpEventManager.INSTANCE.Subscribe(GameEvents.OnJoinMatchRequested, OnJoinMatchRequested);
    }

    private void UnsubscribeFromEvents()
    {
        nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnLoginAsPlayerRequested, LoginPlayer);
        nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnMakeMatchRequested, OnMakeMatchRequested);
        nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnJoinMatchRequested, OnJoinMatchRequested);
    }
}

