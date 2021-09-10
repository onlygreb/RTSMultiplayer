using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    [SerializeField]
    private int resourceCost = 10;
    [SerializeField]
    private UnitMovement unitMovement = null;
    [SerializeField]
    private Targeter targeter = null;
    [SerializeField]
    private Health health = null;

    [SerializeField]
    private UnityEvent onSelected = null;
    [SerializeField]
    private UnityEvent onDeselected = null;

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;

    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDespawned;

    public int GetResourceCost()
    {
        return resourceCost;
    }

    public UnitMovement GetUnitMovement()
    {
        return this.unitMovement;
    }

    public Targeter GetTargeter()
    {
        return this.targeter;
    }

    #region Server

    public override void OnStartServer()
    {
        ServerOnUnitSpawned?.Invoke(this);
        health.serverOnDie += ServerHandleDie;
    }

    public override void OnStopServer()
    {
        ServerOnUnitDespawned?.Invoke(this);
        health.serverOnDie -= ServerHandleDie;
    }

    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    #endregion

    #region Client

    public override void OnStartAuthority()
    {
        AuthorityOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!hasAuthority) return;
        AuthorityOnUnitSpawned?.Invoke(this);
    }

    [Client]
    public void Select()
    {
        if (!hasAuthority) return;
        onSelected?.Invoke();
    }

    [Client]
    public void Deselect()
    {
        if (!hasAuthority) return;
        onDeselected?.Invoke();
    }

    #endregion
}