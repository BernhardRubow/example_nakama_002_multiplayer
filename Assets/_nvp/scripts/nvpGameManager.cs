using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using newvisionsproject.managers.events;

public class nvpGameManager : MonoBehaviour
{

    // +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public static Dictionary<int, string> SCENES;
    public static string HOST;
    public static int PORT;



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

        nvpGameManager.HOST = "192.168.160.151";
        nvpGameManager.PORT = 7350;


		nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnGameInitialized, this, null);
	}
}
