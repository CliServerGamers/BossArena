using System;
using UnityEngine;
using Unity.Netcode;
using BossArena.game;

/// <summary>
/// Only attach this example component to the NetworkManager GameObject.
/// This will provide you with a single location to register for client 
/// connect and disconnect events.  
/// </summary>
public class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager Singleton { get; internal set; }

    [SerializeField]
    private GameObject PlayerPrefab;
    [SerializeField]
    private GameObject AutoAttackPrefab;

    public enum ConnectionStatus
    {
        Connected,
        Disconnected
    }

    /// <summary>
    /// This action is invoked whenever a client connects or disconnects from the game.
    ///   The first parameter is the ID of the client (ulong).
    ///   The second parameter is whether that client is connecting or disconnecting.
    /// </summary>
    public event Action<ulong, ConnectionStatus> OnClientConnectionNotification;

    private void Awake()
    {
        if (Singleton != null)
        {
            // As long as you aren't creating multiple NetworkManager instances, throw an exception.
            // (***the current position of the callstack will stop here***)
            throw new Exception($"Detected more than one instance of {nameof(ConnectionManager)}! " +
                $"Do you have more than one component attached to a {nameof(GameObject)}");
        }
        Singleton = this;
    }

    private void Start()
    {
        if (Singleton != this)
        {
            return; // so things don't get even more broken if this is a duplicate >:(
        }

        if (NetworkManager.Singleton == null)
        {
            // Can't listen to something that doesn't exist >:(
            throw new Exception($"There is no {nameof(NetworkManager)} for the {nameof(ConnectionManager)} to do stuff with! " +
                $"Please add a {nameof(NetworkManager)} to the scene.");
        }

        NetworkManager.Singleton.OnServerStarted += OnServerStartedCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;

    }

    private void OnDestroy()
    {
        // Since the NetworkManager can potentially be destroyed before this component, only 
        // remove the subscriptions if that singleton still exists.
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnServerStarted -= OnServerStartedCallback;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
        }
    }

    private void OnServerStartedCallback()
    {
        Debug.Log("Server started");
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        OnClientConnectionNotification?.Invoke(clientId, ConnectionStatus.Connected);
        Debug.Log($"{clientId} just connected...");

        if (NetworkManager.Singleton.IsServer)
        {
            //spawnPlayer(clientId);
        }
        else
        {
            Debug.Log("Generating Client Track");
        }
    }

    private void OnClientDisconnectCallback(ulong clientId)
    {
        OnClientConnectionNotification?.Invoke(clientId, ConnectionStatus.Disconnected);
    }

    public void SpawnPlayer(ulong clientId)
    {
        Debug.Log("Spawning Player");
        GameObject newPlayer;
        newPlayer = (GameObject) Instantiate(PlayerPrefab);
        NetworkObject playerObj = newPlayer.GetComponent<NetworkObject>();
        newPlayer.SetActive(true);
        playerObj.SpawnAsPlayerObject(clientId, true);
        GameObject autoAttackObj = Instantiate(AutoAttackPrefab, Vector3.zero, Quaternion.identity);
        autoAttackObj.GetComponent<NetworkObject>().Spawn();
        autoAttackObj.transform.parent = playerObj.transform;
        autoAttackObj.GetComponent<AutoAttack>().Initialize();
        //Player player = newPlayer.GetComponent<Player>();
        //player.SetPlayerID(clientId);
        //player.SetSpawnPosition();
    }
}
