using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class TeamColorSetter : NetworkBehaviour
{
    [SerializeField]
    private Renderer[] colorRenderers = new Renderer[0];

    [SerializeField]
    private int[] renderersID = new int[0];

    [SyncVar(hook = nameof(HandleTeamColorUpdated))]
    private Color teamColor = new Color();

    #region Server

    public override void OnStartServer()
    {
        RTSPlayer player = connectionToClient.identity.GetComponent<RTSPlayer>();

        teamColor = player.GetTeamColor();
    }

    #endregion

    #region Client

    private void HandleTeamColorUpdated(Color oldColor, Color newColor)
    {
        for (int i = 0; i < colorRenderers.Length; i++)
        {
            colorRenderers[i].materials[renderersID[i]].SetColor("_BaseColor", newColor);
        }
        // foreach (Renderer r in colorRenderers)
        // {
        //     r.material.SetColor("_BaseColor", newColor);
        // }
    }

    #endregion
}