using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlatformSpawn : NetworkBehaviour
{
    [SerializeField] private GameObject _platform;

    private void Awake()
    {
        CmdSpawnPlatform();
    }

    [Command]
    private void CmdSpawnPlatform()
    {
        GameObject obj = Instantiate(_platform, gameObject.transform);
        NetworkServer.Spawn(obj);
    }

}
