using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets._sts.scripts.messages;
using UnityEngine;
using newvisionsproject.managers.events;
using Nakama;
using Nakama.TinyJson;

public class nvpNetworkManager : MonoBehaviour
{

    // +++ nakama fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++	
    public IUserPresence self;
    private IClient _client;
    private ISocket _socket;
    private IChannel _channel;
    private List<IUserPresence> _connectedUsers;
    private ISession _session;
    private IMatchmakerMatched _matchMakerMatch;
    private IMatchmakerTicket _matchMakerTicket;
    private IMatch _match;
    private string _matchId;




    // +++ private fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    private int _playerId;
    private string _id;
    private string _userName;
    private string _password;
    private int _minPlayers = 2;
    private int _maxPlayers = 4;




    // +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void Start()
    {

    }




    // +++ nakama event handler +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    private void OnMatchmakerMatched(object s, IMatchmakerMatched matched)
    {

        _matchMakerMatch = matched;
        nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnNakama_MatchFound, this, _matchMakerMatch);
    }

    private void OnConnect(object sender, EventArgs e)
    {
        nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnNakama_SocketConnected, this, _socket);
    }

    private void OnDisconnect(object sender, EventArgs e)
    {
        nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnNakama_SocketDisconnected, this, null);
    }

    private void OnMatchPresence(object s, IMatchPresenceEvent presenceChange)
    {
        _connectedUsers.AddRange(presenceChange.Joins);
        foreach (var leave in presenceChange.Leaves)
        {
            _connectedUsers.RemoveAll(item => item.SessionId.Equals(leave.SessionId));
        };
    }

    private void OnMatchState(object s, IMatchState state)
    {
        // code for evaluating game messages
        var content = System.Text.Encoding.UTF8.GetString(state.State);
	    object message = null;
	    switch (state.OpCode)
        {
			case PositionUpdateMessage.OpCode:
				message = content.FromJson<PositionUpdateMessage>();
				break;

			case RocketFiredMessage.OpCode:
				message = content.FromJson<RocketFiredMessage>();
				break;

            //default:
            //    Debug.LogFormat("User {0} sent {1}", state.UserPresence.UserId, content);
            //    nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnMessageReceived, state.UserPresence.UserId, content);
            //    break;
        }

	    if (message == null) return;
		nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnMessageReceived, state.UserPresence.UserId, message);
    }




    // +++ private class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    private async Task StartNakameClient()
    {

        // get the nakama client
        _client = new Client("defaultkey", nvpGameManager.HOST, nvpGameManager.PORT, false);

        // create a session
        _session = await _client.AuthenticateEmailAsync(_userName, _password);
    }

    private void LoadPlayerSettings(int playerId)
    {
        if (playerId == 1)
        {
            _userName = PlayerPrefs.GetString("Player1Email");
            _password = PlayerPrefs.GetString("Player1Password");
        }
        else
        {
            _userName = PlayerPrefs.GetString("Player2Email");
            _password = PlayerPrefs.GetString("Player2Password");
        }
    }




    // +++ Async API ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public async Task LoginPlayerAsync(int playerId)
    {
        this.LoadPlayerSettings(playerId);

        await this.StartNakameClient();
    }

    public async Task CreateMatchMakerMatchAsync()
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
    }

    public async Task JoinMatchAsync()
    {
        Debug.Log("awaiting match");
        // await joining match
        _match = await _socket.JoinMatchAsync(_matchMakerMatch);
        Debug.Log("match joined");

        // persisting own presence
        self = _match.Self;
        _matchId = _match.Id;

        Debug.LogFormat("MatchId: {0}", _matchId);

        _connectedUsers.AddRange(_match.Presences);
    }

    public async Task UpdatePlayerSettingsAsync(string userName)
    {
        await _client.UpdateAccountAsync(_session, null, userName, null, null, null);
    }

    public List<IUserPresence> GetConnectedUsers() => _connectedUsers;

    public async Task<IApiUsers> FetchUsersAsync(string[] ids)
    {
        IApiUsers result = await _client.GetUsersAsync(_session, ids);
        return result;
    }

    public void SendDataMessage<T>(int opCode, T msg)
    {
        // using Nakama.TinyJson;
        var id = _matchId;
        //var newState = new Dictionary<string, string> { { "msg", msg } }.ToJson();
        _socket.SendMatchState(id, opCode, msg.ToJson());
    }
}

