using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RTSNetworkManager : NetworkManager
{
    [SerializeField]
    private GameObject unitBasePrefab = null;
    [SerializeField]
    private GameOverHandler gameOverHandlerPrefab = null;

    public static event Action ClientOnConnected;
    public static event Action ClientOnDisconnected;

    private bool isGameInProgress = false;

    public List<RTSPlayer> players { get; } = new List<RTSPlayer>();

    #region Server

    public override void OnServerConnect(NetworkConnection conn)
    {
        if (!isGameInProgress) return;

        conn.Disconnect();
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        RTSPlayer player = conn.identity.GetComponent<RTSPlayer>();
        players.Remove(player);

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        players.Clear();
        isGameInProgress = false;
    }

    public void StartGame()
    {
        if (players.Count < 2) return;

        isGameInProgress = true;

        ServerChangeScene("Scene_Map01");
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        RTSPlayer player = conn.identity.GetComponent<RTSPlayer>();
        players.Add(player);

        player.SetDisplayName($"Player {players.Count}");

        player.SetTeamColor(new Color(
            UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)));

        player.SetPartyOwner(players.Count == 1);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (SceneManager.GetActiveScene().name.StartsWith("Scene_Map"))
        {
            GameOverHandler gameOverHndlerInstance = Instantiate(gameOverHandlerPrefab);

            NetworkServer.Spawn(gameOverHndlerInstance.gameObject);

            foreach (RTSPlayer player in players)
            {
                GameObject unitBaseInstance = Instantiate(unitBasePrefab, GetStartPosition().position,
                    Quaternion.identity);
                NetworkServer.Spawn(unitBaseInstance, player.connectionToClient);
            }
        }
    }

    #endregion

    #region Client

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        ClientOnConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        ClientOnDisconnected?.Invoke();
    }

    public override void OnStopClient()
    {
        players.Clear();
    }

    #endregion




}