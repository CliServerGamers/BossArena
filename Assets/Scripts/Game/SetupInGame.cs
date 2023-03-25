using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BossArena.game
{
    /// <summary>
    /// Once the local localPlayer is in a localLobby and that localLobby has entered the In-Game state, this will load in whatever is necessary to actually run the game part.
    /// This will exist in the game scene so that it can hold references to scene objects that spawned prefab instances will need.
    /// </summary>
    public class SetupInGame : NetworkBehaviour
    {
        //[SerializeField]
        //GameObject m_IngameRunnerPrefab = default;
        //[SerializeField]
        //private GameObject[] m_disableWhileInGame = default;

        [SerializeField]
        private InGameRunner m_inGameRunner;

        private bool m_doesNeedCleanup = false;
        private bool m_hasConnectedViaNGO = false;

        private void Awake()
        {
            m_doesNeedCleanup = true;
            //SetMenuVisibility(false);
            Debug.Log(GetComponent<NetworkObject>().GetHashCode());
#pragma warning disable 4014
            LocalLobby lobby = GameManager.Instance.LocalLobby;
            LocalPlayer localPlayer = GameManager.Instance.m_localUser;
            //RelayManager.Instance.StartNetwork(lobby, localPlayer);
            m_inGameRunner.Initialize(OnConnectionVerified, lobby.PlayerCount, OnGameBegin, OnGameEnd,
              localPlayer);
#pragma warning restore 4014
        }

        void OnConnectionVerified()
        {
            m_hasConnectedViaNGO = true;
        }


        public void OnGameBegin()
        {
            if (!m_hasConnectedViaNGO)
            {
                // If this localPlayer hasn't successfully connected via NGO, forcibly exit the minigame.
                LogHandlerSettings.Instance.SpawnErrorPopup("Failed to join the game.");
                OnGameEnd();
            }
        }

        /// <summary>
        /// Return to the localLobby after the game, whether due to the game ending or due to a failed connection.
        /// </summary>
        public void OnGameEnd()
        {
            if (m_doesNeedCleanup)
            {
                //RelayManager.Instance.Disconnect();
                Destroy(m_inGameRunner
                    .transform.parent
                    .gameObject);
                // Since this destroys the NetworkManager, that will kick off cleaning up networked objects.
                m_doesNeedCleanup = false;
            }
        }
    }
}