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
    public Transform LanderContainer;
    public GameObject LanderPrefab;
    public Material[] PlayerMaterials;




    // +++ editor fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField] private Text _playerNumberDisplay;
    [SerializeField] private Text _messageDisplay;




    // +++ private fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    nvpNetworkManager _networkManager;
    string _displayName;
    GameObject _RootCamera;
    GameObject _GameCamera;
    List<IRemoteMessageHandler> _MessageHandlers = new List<IRemoteMessageHandler>();

    // +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void Start()
    {
        Init();
        StartCoroutine(WaitForPlayers());
        SubcribeToEvents();
    }

    // Update is called once per frame
    void Update()
    {

    }




    // +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void OnDestroy()
    {
        UnsubscribeFromEvents();

        _RootCamera.SetActive(true);
        _GameCamera.SetActive(false);
    }

    void OnMessageReceived(object s, object e)
    {
        foreach (var remote in _MessageHandlers)
        {
            remote.HandleMessage(e);
        }

        //string id = s as string;
        //_messageDisplay.text = e as string;
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
        StopAllCoroutines();

        // list players in game in console
        List<string> ids = _networkManager
            .GetConnectedUsers()
            .OrderBy(x => x.UserId)
            .Select(x => x.UserId)
            .ToList();

        // Get Metadata for players
        var result = await _networkManager
            .FetchUsersAsync(ids.ToArray());

        // When loaded show Displayname in console
        int i = 0;
        foreach (IApiUser player in result.Users)
        {
            i++;
            Debug.LogFormat("Player {0}: {1}", i, player.DisplayName);
        }

        // get myself from user metadata
        _displayName = result.Users.Single(x => x.Id == _networkManager.self.UserId).DisplayName;


        var newPlayerSpawn = new Vector3(-5, 16, 0);

        i = 0;
        foreach (var u in result.Users)
        {
			i++;
            Debug.LogFormat("User id '{0}' username '{1}' displayname '{2}'", u.Id, u.Username, u.DisplayName);

            var lander = Instantiate(LanderPrefab, newPlayerSpawn, Quaternion.identity);
            lander.GetComponentInChildren<TextMesh>().text = u.DisplayName;
            lander.transform.parent = LanderContainer;
            newPlayerSpawn.x += 10;
            lander.GetComponent<Renderer>().material = Instantiate(PlayerMaterials[i-1]);


            if (_networkManager.self.Username != u.Username)
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

        _playerNumberDisplay.text = "";
    }






    // +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public void Init()
    {
        _networkManager = GameObject.Find("managers").GetComponent<nvpNetworkManager>();

        _RootCamera = GameObject.Find("Main Camera");
        _GameCamera = GameObject.Find("Game Camera");

        _RootCamera.SetActive(false);
        _GameCamera.SetActive(true);
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
}