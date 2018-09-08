using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets._sts.scripts;
using Assets._sts.scripts.messages;
using UnityEngine;
using UnityEngine.UI;
using newvisionsproject.managers.events;
using Nakama;

public class nvpGame : MonoBehaviour
{
    // +++ public fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++    
    public Transform _landerParent;
    public GameObject LanderPrefab;




    // +++ editor fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField] private Text _playerNumberDisplay;
    [SerializeField] private Text _messageDisplay;    
    [SerializeField] Material[] _playerMaterials;
    [SerializeField] Transform[] _playerSpawnPoints;




    // +++ private fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    nvpNetworkManager _networkManager;
    string _displayName;
    List<IRemoteMessageHandler> _MessageHandlers = new List<IRemoteMessageHandler>();
    GameObject _rootCamera;
    GameObject _gameCamera;




    // +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void Start()
    {
        Init();
        StartCoroutine(WaitForPlayers());
        SubcribeToEvents();
    }




    // +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void OnDestroy()
    {
        UnsubscribeFromEvents();

        // Restore old camera configuration
        _rootCamera.SetActive(true);
        _gameCamera.SetActive(false);
    }

    void OnMessageReceived(object s, object e)
    {
        foreach (var remote in _MessageHandlers)
        {
            remote.HandleMessage(e);
        }
    }

    IEnumerator WaitForPlayers()
    {
        int numberOfPlayers = 0;
        while (numberOfPlayers != 2)
        {
            numberOfPlayers = _networkManager.GetConnectedUsers().Count;
            _playerNumberDisplay.text = string.Format("{0} Players in match online", numberOfPlayers);
            yield return new WaitForSeconds(1f);
        };
        DisplayUsersOnline();
    }

    private async void DisplayUsersOnline()
    {
        IApiUsers result = await GetPlayerMetaData();

        // Spawn players
        SpawnLocalAndRemotePlayer(result);

        // reset players
        _playerNumberDisplay.text = "";
    }




    // +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public void Init()
    {
        _networkManager = GameObject.Find("managers").GetComponent<nvpNetworkManager>();

        // enable scene camera
        _rootCamera = GameObject.Find("Main Camera");
        _gameCamera = GameObject.Find("Game Camera");        
        _rootCamera.SetActive(false);
        _gameCamera.SetActive(true);
    }

    void SubcribeToEvents()
    {
        nvpEventManager.INSTANCE.Subscribe(GameEvents.OnMessageReceived, OnMessageReceived);
    }

    void UnsubscribeFromEvents()
    {
        nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnMessageReceived, OnMessageReceived);
    }

    public void SendPosition(Vector3 position)
    {
        var message = new PositionUpdateMessage(position);
        _networkManager.SendDataMessage(PositionUpdateMessage.OpCode, message);
    }

    public void SendRocket(Vector3 position, Vector3 direction)
    {
        var message = new RocketFiredMessage(position, direction);
        _networkManager.SendDataMessage(RocketFiredMessage.OpCode, message);
    }

    private void SpawnLocalAndRemotePlayer(IApiUsers result)
    {
        var users = result.Users.ToList();
        for (int i = 0, n = 2; i < n; i++)
        {
            var user = users[i];
            Debug.LogFormat("User id '{0}' username '{1}' displayname '{2}'", user.Id, user.Username, user.DisplayName);

            var lander = Instantiate(
                LanderPrefab,
                _playerSpawnPoints[i].position,
                _playerSpawnPoints[i].rotation);

            lander.GetComponentInChildren<TextMesh>().text = user.DisplayName;
            lander.transform.parent = _landerParent;
            lander.GetComponent<Renderer>().material = Instantiate(_playerMaterials[i]);


            if (_networkManager.self.Username != user.Username)
            {
                var controller = lander.AddComponent<stsRemoteController>();
                _MessageHandlers.Add(controller);

                var fireRocketComponent = lander.AddComponent<stsFireRocket>();
                fireRocketComponent.IsLocalPlayer = false;
                _MessageHandlers.Add(fireRocketComponent);
            }
            else
            {
                lander.AddComponent<stsLanderController>();

                var fireRocketComponent = lander.AddComponent<stsFireRocket>();
                fireRocketComponent.IsLocalPlayer = true;
            }
        }
    }

    private async System.Threading.Tasks.Task<IApiUsers> GetPlayerMetaData()
    {
        // list players in game in console
        List<string> ids = _networkManager
            .GetConnectedUsers()
            .OrderBy(x => x.UserId)
            .Select(x => x.UserId)
            .ToList();

        // Get Metadata for players
        var result = await _networkManager
            .FetchUsersAsync(ids.ToArray());
        return result;
    }

}