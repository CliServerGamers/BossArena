using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BossArena.game
{
    /// <summary>
    /// Once the NetworkManager has been spawned, we need something to manage the game state and setup other in-game objects
    /// that is itself a networked object, to track things like network connect events.
    /// </summary>
    public class InGameRunner : NetworkSingleton<InGameRunner>
    {

        [SerializeField]
        private NetworkedDataStore m_dataStore = default;

        public Action onGameBeginning;
        Action m_onConnectionVerified, m_onGameEnd;
        private int m_expectedPlayerCount; // Used by the host, but we can't call the RPC until the network connection completes.
        private bool? m_canSpawnInGameObjects;
        private float m_timeout = 10;
        private bool m_hasConnected = false;

        private PlayerData
            m_localUserData; // This has an ID that's not necessarily the OwnerClientId, since all clients will see all spawned objects regardless of ownership.

        [SerializeField]
        private GameObject m_PlayerPrefab;
        [SerializeField]
        public ArchetypeList ArchetypeList;


        [field: SerializeField]
        public List<GameObject> PlayerList;

        [field: SerializeField]
        public List<GameObject> ActiveEntityList;

        public void Initialize(Action onConnectionVerified, int expectedPlayerCount, Action onGameBegin,
            Action onGameEnd,
            LocalPlayer localUser)
        {
            Debug.Log("InGameRunner: Initialize");
            m_onConnectionVerified = onConnectionVerified;
            m_expectedPlayerCount = expectedPlayerCount;
            onGameBeginning = onGameBegin;
            m_onGameEnd = onGameEnd;
            m_canSpawnInGameObjects = null;
            m_localUserData = new PlayerData(localUser.DisplayName.Value, 0, localUser.Archetype.Value);
        }

        public override void OnNetworkSpawn()
        {
            Debug.Log("InGameRunner: OnNetworkSpawn");
            if (IsHost)
                FinishInitialize();
            m_localUserData = new PlayerData(m_localUserData.name, NetworkManager.Singleton.LocalClientId, m_localUserData.archetype);
            VerifyConnection_ServerRpc(m_localUserData.id);
        }

        public override void OnNetworkDespawn()
        {
            m_onGameEnd(); // As a backup to ensure in-game objects get cleaned up, if this is disconnected unexpectedly.
        }

        private void FinishInitialize()
        {
            //Spawn boss and stuff
        }

        /// <summary>
        /// To verify the connection, invoke a server RPC call that then invokes a client RPC call. After this, the actual setup occurs.
        /// </summary>
        [ServerRpc(RequireOwnership = false)]
        private void VerifyConnection_ServerRpc(ulong clientId)
        {
            VerifyConnection_ClientRpc(clientId);

        }

        [ClientRpc]
        private void VerifyConnection_ClientRpc(ulong clientId)
        {
            if (clientId == m_localUserData.id)
                VerifyConnectionConfirm_ServerRpc(m_localUserData);
        }

        /// <summary>
        /// Once the connection is confirmed, spawn a player cursor and check if all players have connected.
        /// </summary>
        [ServerRpc(RequireOwnership = false)]
        private void VerifyConnectionConfirm_ServerRpc(PlayerData clientData)
        {

            m_dataStore.AddPlayer(clientData.id, clientData.name, clientData.archetype);
            // The game will begin at this point, or else there's a timeout for booting any unconnected players.
            bool areAllPlayersConnected = NetworkManager.Singleton.ConnectedClients.Count >= m_expectedPlayerCount;
            VerifyConnectionConfirm_ClientRpc(clientData.id, areAllPlayersConnected);
        }

        [ClientRpc]
        private void VerifyConnectionConfirm_ClientRpc(ulong clientId, bool canBeginGame)
        {
            if (clientId == m_localUserData.id)
            {
                m_onConnectionVerified?.Invoke();
                m_hasConnected = true;
            }

            if (canBeginGame && m_hasConnected)
            {
                m_timeout = -1;
                BeginGame();
            }
        }

        /// <summary>
        /// The game will begin either when all players have connected successfully or after a timeout.
        /// </summary>
        void BeginGame()
        {
            m_canSpawnInGameObjects = true;
            GameManager.Instance.BeginGame();
            onGameBeginning?.Invoke();
            //Debug.Log("Spanner");
            //if (NetworkManager.Singleton.LocalClientId != m_localUserData.id) return;
            //if (IsServer)
            //    m_dataStore.GetPlayerData(m_localUserData.id, spawnPlayer);
            //else
            //    m_dataStore.GetPlayerData(m_localUserData.id, spawnPlayerServerRPC);

        }

        //public void Update()
        //{
        //    if (m_timeout >= 0)
        //    {
        //        m_timeout -= Time.deltaTime;
        //        if (m_timeout < 0)
        //            BeginGame();
        //    }

        //}

        /// <summary>
        /// The server determines when the game should end. Once it does, it needs to inform the clients to clean up their networked objects first,
        /// since disconnecting before that happens will prevent them from doing so (since they can't receive despawn events from the disconnected server).
        /// </summary>
        [ClientRpc]
        private void WaitForEndingSequence_ClientRpc()
        {
            //m_scorer.OnGameEnd();
            //m_introOutroRunner.DoOutro(EndGame);
        }

        private void EndGame()
        {
            if (IsHost)
                StartCoroutine(EndGame_ClientsFirst());
        }

        private IEnumerator EndGame_ClientsFirst()
        {
            EndGame_ClientRpc();
            yield return null;
            SendLocalEndGameSignal();
        }

        [ClientRpc]
        private void EndGame_ClientRpc()
        {
            if (IsHost)
                return;
            SendLocalEndGameSignal();
        }

        private void SendLocalEndGameSignal()
        {
            m_onGameEnd();
        }

        //private void spawnPlayer(PlayerData playerData)
        //{
        //    GameObject newPlayer;
        //    newPlayer = (GameObject)Instantiate(m_PlayerPrefab);
        //    NetworkObject playerObj = newPlayer.GetComponent<NetworkObject>();
        //    newPlayer.SetActive(true);
        //    newPlayer.GetComponent<Player>().Archetype = playerData.archetype;
        //    InGameRunner.Instance.PlayerList.Add(newPlayer);
        //    playerObj.SpawnWithOwnership(playerData.id, true);
        //}

        //[ServerRpc(RequireOwnership = false)]
        //private void spawnPlayerServerRPC(PlayerData playerData)
        //{
        //    Debug.Log("Spawning Player RPC");
        //    spawnPlayer(playerData);
        //}
    }
}