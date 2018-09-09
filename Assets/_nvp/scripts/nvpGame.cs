using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using newvisionsproject.managers.events;
using Nakama;
using Assets._sts.scripts;

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
    nvpMultiplayerManager _multiplayerManager;
    string _displayName;
    
    GameObject _rootCamera;
    GameObject _gameCamera;




    // +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void Start()
    {
        Init();
        StartCoroutine(WaitForPlayers());
    }




    // +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void OnDestroy()
    {

        // Restore old camera configuration
        _rootCamera.SetActive(true);
        _gameCamera.SetActive(false);
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
        IApiUsers playerMetaData = await _networkManager.GetPlayerMetaData();

        // Spawn players
        SpawnLocalAndRemotePlayer(playerMetaData);

        // reset players
        _playerNumberDisplay.text = "";
    }




    // +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public void Init()
    {
        _networkManager = GameObject.Find("managers").GetComponent<nvpNetworkManager>();
        _multiplayerManager = this.gameObject.GetComponent<nvpMultiplayerManager>();
        
        // enable scene camera
        _rootCamera = GameObject.Find("Main Camera");
        _gameCamera = GameObject.Find("Game Camera");        
        _rootCamera.SetActive(false);
        _gameCamera.SetActive(true);
    }

    private void SpawnLocalAndRemotePlayer(IApiUsers playerMetaData)
    {
        var users = playerMetaData.Users.ToList();
        for (int i = 0, n = 2; i < n; i++)
        {
            var user = users[i];
            Debug.LogFormat("User id '{0}' username '{1}' displayname '{2}'", user.Id, user.Username, user.DisplayName);

            var lander = Instantiate(
                LanderPrefab,
                _playerSpawnPoints[i].position,
                _playerSpawnPoints[i].rotation);

            // Make the lander a child of an existing game object in the scene
            lander.transform.parent = _landerParent;

            // configure the lander's appearence
            lander.GetComponentInChildren<TextMesh>().text = user.DisplayName;
            lander.GetComponent<Renderer>().material = Instantiate(_playerMaterials[i]);

            // decide whether the thi script is executed on the local or on remote machine 

            if (_networkManager.self.Username != user.Username)
            {
                // if the player we got from iterating the playerlist has 
                // the same user name as the player on this machine
                // we instantiate the local player
                
                // so we attach the remote player script
                var controller = lander.GetComponent<stsLanderController>();
                controller.isLocalPlayer = false;
                controller.RemoveRigidBody();
                _multiplayerManager.AddMessageHandler(controller);

                var fireRocketComponent = lander.AddComponent<stsFireRocket>();
                _multiplayerManager.AddMessageHandler(fireRocketComponent);
            }
            else
            {
                // if the player we got from iterating the playerlist has 
                // the same user name as the player on this machine
                // we instantiate the local player

                // so we ha to attach the local player script
                var controller = lander.GetComponent<stsLanderController>();
                var fireRocketComponent = lander.AddComponent<stsFireRocket>();
                controller.isLocalPlayer = true;

                // inform all scripts on the lander that they are on the local player
                foreach (var localPlayerAwareScript in lander.GetComponentsInChildren<ILocalPlayerAwareScript>()){
                    localPlayerAwareScript.SetIsLocalPlayer();
                }               
            }
        }
    }
}