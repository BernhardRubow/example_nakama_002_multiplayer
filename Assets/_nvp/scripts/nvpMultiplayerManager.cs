using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets._sts.scripts;
using Assets._sts.scripts.messages;

public class nvpMultiplayerManager : MonoBehaviour
{

    // +++ public fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++    
    // +++ editor fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // +++ private fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    private nvpNetworkManager _networkManager;



    // +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }




    // +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
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
}