using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class nvpGame : MonoBehaviour
{

    // +++ public fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++    
    // +++ editor fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // +++ private fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    nvpNetworkManager _networkManager;




    // +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void Start()
    {
        Init();
		StartCoroutine(WaitForPlayers());
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    // +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	IEnumerator WaitForPlayers(){
		while(_networkManager.GetConnectedUsers().Count != 2) yield return new WaitForSeconds(1f);
		DisplayUsersOnline();
	}

    private async void DisplayUsersOnline()
    {
		StopAllCoroutines();
        string[] ids = _networkManager.GetConnectedUsers().Select(x => x.UserId).ToArray();
		var result = await _networkManager.FetchUsersAsync(ids);
		foreach (var u in result.Users)
		{
			Debug.LogFormat("User id '{0}' username '{1}' displayname '{2}'", u.Id, u.Username, u.DisplayName);
		}
    }






    // +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public void Init()
    {
        _networkManager = GameObject.Find("managers").GetComponent<nvpNetworkManager>();
    }
}