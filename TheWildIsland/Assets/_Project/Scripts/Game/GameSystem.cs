using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.Pong;
using UnityEngine;

public class GameSystem : NetworkBehaviour
{
    [SerializeField] private PlayerManager _playerManager;
    
    private bool _hasStarted = false;
    
    private void OnGUI()
    {
        if (!isServer)
        {
            return;
        }

        Vector2 pos = new Vector2(100, Screen.height - 200);
        Vector2 size = new Vector2(200, 100);

        if (!_hasStarted)
        {
            if (GUI.Button(new Rect(pos, size), "Begin Match"))
            {
                OnMatchBegin();
            }
        }
    }
    
    private void OnMatchBegin()
    {
        if (_playerManager.numPlayers != 2)
        {
            Debug.LogError("Need two connected players!");
            return;
        }
            
        _hasStarted = true;
            
        //_playerManager.ball = Instantiate(_playerManager.spawnPrefabs.Find(prefab => prefab.name == "Ball"));
        //NetworkServer.Spawn(_playerManager.ball);
    }

}
