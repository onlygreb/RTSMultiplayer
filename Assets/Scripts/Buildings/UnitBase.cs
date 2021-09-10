using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class UnitBase : NetworkBehaviour
{
    [SerializeField]
    private Health health = null;

    public static event Action<UnitBase> ServerOnBaseSpawned;
    public static event Action<UnitBase> ServerOnBaseDespawned;
    public static event Action<int> ServerOnPlayerDie;

    #region Server

    public override void OnStartServer()
    {
        health.serverOnDie += ServerHandleDie;

        ServerOnBaseSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        health.serverOnDie -= ServerHandleDie;

        ServerOnBaseDespawned?.Invoke(this);
    }

    [Server]
    private void ServerHandleDie()
    {
        ServerOnPlayerDie?.Invoke(connectionToClient.connectionId);
        NetworkServer.Destroy(gameObject);
    }

    #endregion

    #region Client

    #endregion
}