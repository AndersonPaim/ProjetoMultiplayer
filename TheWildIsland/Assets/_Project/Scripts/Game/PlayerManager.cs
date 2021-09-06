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
    public class PlayerManager : NetworkManager
    {
        public static event Action OnConnectPlayer;
        public static event Action OnDisconnectPlayer;

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            GameObject player = Instantiate(playerPrefab);
            Player p = player.GetComponent<Player>();
            p.SetupPlayer(numPlayers);
            DontDestroyOnLoad(player);
            
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
