using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets._sts.scripts;
using Assets._sts.scripts.messages;
using newvisionsproject.managers.events;

public class nvpMultiplayerManager : MonoBehaviour
{

    // +++ public fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++  
     
    // +++ editor fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // +++ private fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    private nvpNetworkManager _networkManager;
    private List<IRemoteMessageHandler> _messageHandlers = new List<IRemoteMessageHandler>(); 


    // +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void Start()
    {
        Init();
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

    void OnMessageReceived(object s, object e)
    {
        foreach (var remote in _messageHandlers)
        {
            remote.HandleMessage(e);
        }
    }




    // +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    private void Init()
    {

        // collect scene references
        _networkManager = GameObject.Find("managers").GetComponent<nvpNetworkManager>();
        if (_networkManager == null) Debug.LogError("No Network Manager");
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

    void SubcribeToEvents()
    {
        nvpEventManager.INSTANCE.Subscribe(GameEvents.OnMessageReceived, OnMessageReceived);
    }

    void UnsubscribeFromEvents()
    {
        nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnMessageReceived, OnMessageReceived);
    }

    public void AddMessageHandler(IRemoteMessageHandler messageHandler){
        _messageHandlers.Add(messageHandler);
    }

    public void RemoveMessageHandler(IRemoteMessageHandler messageHandler){
        _messageHandlers.Remove(messageHandler);
    }
}