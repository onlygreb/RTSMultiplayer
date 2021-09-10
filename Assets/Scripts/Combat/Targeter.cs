using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Targeter : NetworkBehaviour
{
    [SerializeField]
    private Targetable target;

    public Targetable GetTarget()
    {
        return target;
    }

    #region Server

    public override void OnStartServer()
    {
        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
    }

    public override void OnStopServer()
    {
        GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
    }

    [Server]
    private void ServerHandleGameOver()
    {
        ClearTarget();
    }

    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        if (!targetGameObject.TryGetComponent<Targetable>(out Targetable newTarget)) return;

        this.target = newTarget;
    }

    [Server]
    public void ClearTarget()
    {
        target = null;
    }

    #endregion
}