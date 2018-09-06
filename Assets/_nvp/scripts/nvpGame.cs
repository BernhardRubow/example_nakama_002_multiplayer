using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets._sts.scripts;
using Assets._sts.scripts.messages;
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
	private GameObject _RootCamera;
	private GameObject _GameCamera;

	public Transform LanderContainer;
	public GameObject LanderPrefab;
	public Material LocalPlayerMaterial;
	public Material RemotePlayerMaterial;

	private List<IRemoteMessageHandler> _MessageHandlers = new List<IRemoteMessageHandler>();

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
	void OnDestroy()
	{
		UnsubscribeFromEvents();

		_RootCamera.SetActive(true);
		_GameCamera.SetActive(false);
	}

	void OnMessageReceived(object s, object e)
	{
		foreach (var remote in _MessageHandlers)
		{
			remote.HandleMessage(e);
		}

		//string id = s as string;
		//_messageDisplay.text = e as string;
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

		var newPlayerSpawn = new Vector3(-5, 16, 0);

		foreach (var u in result.Users)
		{
			Debug.LogFormat("User id '{0}' username '{1}' displayname '{2}'", u.Id, u.Username, u.DisplayName);

			var lander = Instantiate(LanderPrefab, newPlayerSpawn, Quaternion.identity);
			lander.GetComponentInChildren<TextMesh>().text = u.DisplayName;
			lander.transform.parent = LanderContainer;
			newPlayerSpawn.x += 10;

			if (_networkManager.self.Username != u.Username)
			{
				Debug.Log("Call SendDataMessage");
				_networkManager.SendDataMessage(1, string.Format("{0} greets {1}", _displayName, u.DisplayName));

				var controller = lander.AddComponent<stsRemoteController>();
				lander.GetComponent<Renderer>().material = Instantiate(RemotePlayerMaterial);
				_MessageHandlers.Add(controller);
				
				var fireRocketComponent = lander.AddComponent<stsFireRocket>();
				fireRocketComponent.IsLocalPlayer = false;
				_MessageHandlers.Add(fireRocketComponent);
			}
			else
			{
				lander.AddComponent<stsLanderController>();
				lander.GetComponent<Renderer>().material = Instantiate(LocalPlayerMaterial);

				var fireRocketComponent = lander.AddComponent<stsFireRocket>();
				fireRocketComponent.IsLocalPlayer = true;
			}
		}

		_playerNumberDisplay.text = "";
	}






	// +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public void Init()
	{
		_networkManager = GameObject.Find("managers").GetComponent<nvpNetworkManager>();

		_RootCamera = GameObject.Find("Main Camera");
		_GameCamera = GameObject.Find("Game Camera");

		_RootCamera.SetActive(false);
		_GameCamera.SetActive(true);
	}

	void SubcribeToEvents()
	{
		nvpEventManager.INSTANCE.Subscribe(GameEvents.OnMessageReceived, OnMessageReceived);
	}

	void UnsubscribeFromEvents()
	{
		nvpEventManager.INSTANCE.Unsubscribe(GameEvents.OnMessageReceived, OnMessageReceived);
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