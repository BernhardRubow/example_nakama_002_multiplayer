using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using newvisionsproject.managers.events;

public class nvpGameManager : MonoBehaviour
{

    // +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public static Dictionary<int, string> SCENES;



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

		nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnGameInitialized, this, null);
	}
}
