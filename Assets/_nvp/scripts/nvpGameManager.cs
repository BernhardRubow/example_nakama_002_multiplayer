using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using newvisionsproject.managers.events;
using System;

public class nvpGameManager : MonoBehaviour
{

    // +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public static Dictionary<int, string> SCENES;
    public static string HOST;
    public static int PORT;




    // +++ editor fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField] private string host = "0.0.0.0";
    [SerializeField] private int port = 7350;


    // +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void Start()
    {
		InitGame();
    }




    // +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void InitGame(){
		nvpGameManager.SCENES = new Dictionary<int, string>();
		nvpGameManager.SCENES[0] = "menuMain";

        nvpGameManager.HOST = host;
        nvpGameManager.PORT = port;


		nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnGameInitialized, this, null);
	}
}
