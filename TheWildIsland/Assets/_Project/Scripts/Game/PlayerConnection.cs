using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Mirror.Examples.Pong
{
    public class PlayerConnection : NetworkBehaviour
    {
        public delegate void LocalPlayerHandler(Player player);
        public delegate void ReadyHandler(Player player);
        public static LocalPlayerHandler OnLocalPlayer;
        public static ReadyHandler OnAddReady;
        public static ReadyHandler OnRemoveReady;

        [SerializeField] private Player _player;
        [SerializeField] private GameObject _bulletObj;

        public Player LocalPlayer {get; set;}
        public bool IsReady = false;

        private void Start()
        {
            if(hasAuthority)
            {
                LocalPlayer = _player;
                OnLocalPlayer?.Invoke(_player);

                if(isClient)
                {
                    NetworkLobby.OnSetPlayerName += CmdSetPlayerName;
                    Player.OnShoot += CmdShoot;
                }
            }
        }

        private void OnDestroy()
        {
            if(hasAuthority)
            {
                if(isClient)
                {
                    NetworkLobby.OnSetPlayerName -= CmdSetPlayerName;
                    Player.OnShoot -= CmdShoot;
                }
            }
        }

        public void ReadyClick()
        {
            if (IsReady)
            {
                IsReady = false;

                if (isClient && hasAuthority)
                {
                    CmdRemoveReady(LocalPlayer);
                }
                else if (isServer)
                {
                    RpcRemoveReady(LocalPlayer);
                }
            }
            else
            {
                IsReady = true;

                if (isServer)
                {
                    Debug.Log(LocalPlayer);
                    RpcAddReady(LocalPlayer);
                }
                else if (isClient)
                {
                    CmdAddReady(LocalPlayer);
                }

            }
        }

        [Command]
        private void CmdSetPlayerName(Player player, string name)
        {
            player.PlayerName = name;
        }

        [Command]
        public void CmdAddReady(Player player)
        {
            RpcAddReady(player);

            if (isServerOnly)
            {
                OnAddReady?.Invoke(player);
            }
        }

        [Command]
        public void CmdRemoveReady(Player player)
        {
            RpcRemoveReady(player);

            if (isServerOnly)
            {
                OnRemoveReady?.Invoke(player);
            }
        }

        [Command]
        private void CmdShoot(Vector3 pos, Quaternion rot, GameObject bullet)
        {
            GameObject obj = Instantiate(_bulletObj, pos, rot);
            obj.GetComponent<Bullet>().Shoot();
            NetworkServer.Spawn(obj);
        }

        [ClientRpc]
        public void RpcAddReady(Player player)
        {
            OnAddReady?.Invoke(player);
        }

        [ClientRpc]
        public void RpcRemoveReady(Player player)
        {
            OnRemoveReady?.Invoke(player);
        }

    }
}
