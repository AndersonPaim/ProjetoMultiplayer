using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Mirror.NetworkBehaviour;

namespace Mirror.Examples.Pong
{

    [AddComponentMenu("")]
    public class GameSystem : NetworkManager
    {
        public static event Action OnConnectPlayer;
        public static event Action OnDisconnectPlayer;

        [SerializeField] private List<Player> _players = new List<Player>();
        
        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            GameObject player = Instantiate(playerPrefab);
            Player p = player.GetComponent<Player>();
            _players.Add(p);
            Debug.Log("NUM PLAYERS: " + numPlayers);
            p.SetupPlayer(numPlayers);
            NetworkServer.AddPlayerForConnection(conn, player);
            OnConnectPlayer?.Invoke();
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            // call base functionality (actually destroys the player)
            base.OnServerDisconnect(conn);
            OnDisconnectPlayer?.Invoke();
        }
    }
}
