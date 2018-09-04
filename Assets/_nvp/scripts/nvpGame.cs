using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using newvisionsproject.managers.events;

public class nvpGame : MonoBehaviour
{

    // +++ public fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++    
    // +++ editor fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField] private Text _playerNumberDisplay;
    [SerializeField] private Text _messageDisplay;
    // +++ private fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    nvpNetworkManager _networkManager;
    
    private string _displayName;




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
    void OnDestroy(){
        UnsubscribeFromEvents();
    }
    
    void OnMessageReceived(object s, object e){
        string id = s as string;
        _messageDisplay.text = e as string;
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
        string[] ids = _networkManager.GetConnectedUsers().Select(x => x.UserId).ToArray();
        var result = await _networkManager.FetchUsersAsync(ids);
        _displayName = result.Users.Single(x => x.Id == _networkManager.self.UserId).DisplayName;
        Debug.LogFormat("DisplayName: {0}", _displayName);
        foreach (var u in result.Users)
        {
            Debug.LogFormat("User id '{0}' username '{1}' displayname '{2}'", u.Id, u.Username, u.DisplayName);

            if (_networkManager.self.Username != u.Username)
            {
                Debug.Log("Call SendDataMessage");
                _networkManager.SendDataMessage(string.Format("{0} greets {1}", _displayName, u.DisplayName));
            }
        }
    }






        // +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public void Init()
        {
            _networkManager = GameObject.Find("managers").GetComponent<nvpNetworkManager>();
        }

        void SubcribeToEvents(){
            nvpEventManager.INSTANCE.Subscribe(GameEvents.OnMessageReceived, OnMessageReceived);
        }

        void UnsubscribeFromEvents(){            
            nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnMessageReceived, OnMessageReceived);
        }
    }