using UnityEngine;

namespace Mirror.Examples.Pong
{
    // Custom NetworkManager that simply assigns the correct racket positions when
    // spawning players. The built in RoundRobin spawn method wouldn't work after
    // someone reconnects (both players would be on the same side).
    [AddComponentMenu("")]
    public class GameSystem : NetworkManager
    {
        public Transform leftRacketSpawn;
        public Transform rightRacketSpawn;
        GameObject ball;

        private bool _hasStarted = false;
        
        private void OnGUI()
        {
            Debug.Log(IsServer);
            
            if (!IsServer)
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


        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            // add player at correct spawn position
            Transform start = numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;
            GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
            NetworkServer.AddPlayerForConnection(conn, player);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            // destroy ball
            if (ball != null)
                NetworkServer.Destroy(ball);

            // call base functionality (actually destroys the player)
            base.OnServerDisconnect(conn);
        }

        private void OnMatchBegin()
        {
            if (numPlayers != 2)
            {
                Debug.LogError("Need two connected players!");
                return;
            }
            
            _hasStarted = true;
            
            ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
            NetworkServer.Spawn(ball);
        }
    }
}
