using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Mirror;
using Mirror.Examples.Pong;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkLobby : NetworkBehaviour
{
	public static event Action OnStartGame;

	[SerializeField] private Button _readyButton;
	[SerializeField] private List<Toggle> _playersConfirmed;
	
	[SyncVar]
	private int _readyPlayers;
	private Player _localPlayer;

	private void Start()
	{
		Initialize();
	}

	private void Initialize()
	{
		_readyButton.onClick.AddListener(HandleReadyButtonClicked);
		PlayerConnection.OnAddReady += AddReady;
		PlayerConnection.OnRemoveReady += RemoveReady;
	}

	private void HandleReadyButtonClicked()
	{
		NetworkClient.localPlayer.GetComponent<PlayerConnection>().ReadyClick();
	}

	private void AddReady()
	{
		_readyPlayers++;

		if (_readyPlayers == 1)
		{
			_playersConfirmed[0].isOn = true;
		}
		else if (_readyPlayers == 2)
		{
			_playersConfirmed[1].isOn = true;
			StartCoroutine(StartGame());
		}
	}

	private void RemoveReady()
	{
		_readyPlayers--;
		
		if (_readyPlayers == 0)
		{
			_playersConfirmed[0].isOn = false;
		}
		else if (_readyPlayers == 1)
		{
			_playersConfirmed[1].isOn = false;
			StopCoroutine(StartGame());
		}
	}

	private IEnumerator StartGame()
	{
		yield return new WaitForSeconds(2);
		OnStartGame?.Invoke();
		SceneManager.LoadScene("Game");
	}
}
