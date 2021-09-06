using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Game
{
    public class PlayerConnection : NetworkBehaviour
    {
        public static event Action OnAddReady;
        public static event Action OnRemoveReady;

        public bool IsReady = false;

        public void ReadyClick()
        {
            Debug.Log("ReadyClick");
            if (IsReady)
            {
                IsReady = false;

                if (isClient && hasAuthority)
                {
                    CmdRemoveReady();
                }
                else if (isServer)
                {
                    RpcRemoveReady();
                }
            }
            else
            {
                IsReady = true;

                Debug.Log(NetworkClient.localPlayer.hasAuthority);
                if (isServer)
                {
                    RpcAddReady();
                }
                else if (isClient)
                {
                    CmdAddReady();
                }

            }
        }

        [Command]
        public void CmdAddReady()
        {
            RpcAddReady();

            if (isServerOnly)
            {
                OnAddReady?.Invoke();
                ;
            }
        }

        [Command]
        public void CmdRemoveReady()
        {
            RpcRemoveReady();

            if (isServerOnly)
            {
                OnRemoveReady?.Invoke();
            }
        }

        [ClientRpc]
        public void RpcAddReady()
        {
            OnAddReady?.Invoke();
            ;
        }

        [ClientRpc]
        public void RpcRemoveReady()
        {
            OnRemoveReady?.Invoke();
        }

    }
}
