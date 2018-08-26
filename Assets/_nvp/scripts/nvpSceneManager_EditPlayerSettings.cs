using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using newvisionsproject.managers.events;
using System.Threading;

public class nvpSceneManager_EditPlayerSettings : MonoBehaviour {

	// +++ fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	private nvpNetworkManager _networkManager;
    private nvpSceneManager _sceneManager;




	// +++ editor fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[SerializeField] InputField _email;
	[SerializeField] InputField _password;
	[SerializeField] InputField _userName;
	[SerializeField] int _playerIndex;





	// +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void Start () {
		Init();

        SubscribeToEvents();
	}




	// +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public async void OnSaveClicked()
    {
        Debug.Log("OnSaveClicked called");

        UpdateLocalPlayerPrefs();

		await UpdateServerSettings();

        _sceneManager.LoadScene("menuMain");
    }

    void OnDestroy(){
        UnSubscribeFromEvents();
    }

    void OnNakama_SessionCreated(object sender, object eventArgs){
        Debug.Log("Session created for storing user settings");
    }




    // +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void Init(){

		// get references
		_networkManager = GameObject.Find("managers").GetComponent<nvpNetworkManager>();
		_sceneManager = GameObject.Find("managers").GetComponent<nvpSceneManager>();

		// get player prefs
		if(_playerIndex == 1){
			_userName.text = PlayerPrefs.GetString("Player1Name");
			_email.text = PlayerPrefs.GetString("Player1Email");
			_password.text = PlayerPrefs.GetString("Player1Password");
		}
		else {			
			_userName.text = PlayerPrefs.GetString("Player2Name");
			_email.text = PlayerPrefs.GetString("Player2Email");
			_password.text = PlayerPrefs.GetString("Player2Password");
		}
	}

    private void UpdateLocalPlayerPrefs()
    {
        if (_playerIndex == 1)
        {
            PlayerPrefs.SetString("Player1Name", _userName.text);
            PlayerPrefs.SetString("Player1Email", _email.text);
            PlayerPrefs.SetString("Player1Password", _password.text);
        }
        else
        {
            PlayerPrefs.SetString("Player2Name", _userName.text);
            PlayerPrefs.SetString("Player2Email", _email.text);
            PlayerPrefs.SetString("Player2Password", _password.text);
        }
    }

	async Task UpdateServerSettings(){
		await _networkManager.LoginPlayerAsync(_playerIndex);
	}

    private void SubscribeToEvents()
    {
        nvpEventManager.INSTANCE.Subscribe(GameEvents.OnNakama_SessionCreated, OnNakama_SessionCreated);
    }

    private void UnSubscribeFromEvents()
    {
        nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnNakama_SessionCreated, OnNakama_SessionCreated);
    }
}
