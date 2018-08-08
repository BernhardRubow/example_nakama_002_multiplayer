﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using newvisionsproject.managers.events;

public class nvpSceneManager_EditPlayerSettings : MonoBehaviour {

	// +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[SerializeField] InputField _email;
	[SerializeField] InputField _password;
	[SerializeField] int _playerIndex;



	// +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void Start () {
		Init();
	}
	
	void Update () {
		
	}




	// +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public void OnSaveClicked(){
		Debug.Log("OnSaveClicked called");
		if(_playerIndex == 1){
			PlayerPrefs.SetString("Player1Email",_email.text);
			PlayerPrefs.SetString("Player1Password", _password.text);
		}
		else {			
			PlayerPrefs.SetString("Player2Email",_email.text);
			PlayerPrefs.SetString("Player2Password", _password.text);
		}
		nvpEventManager.INSTANCE.InvokeEvent(GameEvents.OnPlayerSettingsSaved, this, null);
	}




	// +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void Init(){
		if(_playerIndex == 1){
			_email.text = PlayerPrefs.GetString("Player1Email");
			_password.text = PlayerPrefs.GetString("Player1Password");
		}
		else {			
			_email.text = PlayerPrefs.GetString("Player2Email");
			_password.text = PlayerPrefs.GetString("Player2Password");
		}
	}
}
