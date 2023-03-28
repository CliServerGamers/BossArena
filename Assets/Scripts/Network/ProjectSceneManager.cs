using System;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine;
using BossArena.game;

namespace BossArena
{


    public class ProjectSceneManager : NetworkSingleton<ProjectSceneManager>
    {
        /// INFO: You can remove the #if UNITY_EDITOR code segment and make SceneName public,
        /// but this code assures if the scene name changes you won't have to remember to
        /// manually update it.
#if UNITY_EDITOR
        public UnityEditor.SceneAsset SceneAsset;
        private void OnValidate()
        {
            if (SceneAsset != null)
            {
                m_SceneName = SceneAsset.name;
            }
        }
#endif

        public string testScene;
        [SerializeField]
        private string m_SceneName;
        private Scene m_LoadedScene;

        [SerializeField]
        private GameObject PlayerPrefab;
        [SerializeField]
        private GameObject AutoAttackPrefab;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public bool SceneIsLoaded
        {
            get {
                if (m_LoadedScene.IsValid() && m_LoadedScene.isLoaded)
                {
                    return true;
                }
                return false;
            }
        }

        public override void OnNetworkSpawn()
        {
            Debug.Log($"{this.GetType().Name}: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            if (IsServer)
            {
                NetworkManager.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;

            }

            base.OnNetworkSpawn();
        }

        public void LoadScene(string sceneName)
        {
            var status = NetworkManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            CheckStatus(status);
        }

        private void CheckStatus(SceneEventProgressStatus status, bool isLoading = true)
        {
            var sceneEventAction = isLoading ? "load" : "unload";
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to {sceneEventAction} {m_SceneName} with" +
                    $" a {nameof(SceneEventProgressStatus)}: {status}");
            }
        }

        /// <summary>
        /// Handles processing notifications when subscribed to OnSceneEvent
        /// </summary>
        /// <param name="sceneEvent">class that has information about the scene event</param>
        private void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
        {
            Debug.Log($"{this.GetType().Name}: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            var clientOrServer = sceneEvent.ClientId == NetworkManager.ServerClientId ? "server" : "client";
            switch (sceneEvent.SceneEventType)
            {
                case SceneEventType.LoadComplete:
                    {
                        // We want to handle this for only the server-side
                        if (sceneEvent.ClientId == NetworkManager.ServerClientId)
                        {
                            // *** IMPORTANT ***
                            // Keep track of the loaded scene, you need this to unload it
                            m_LoadedScene = sceneEvent.Scene;
                        }
                        Debug.Log($"Loaded the {sceneEvent.SceneName} scene on " +
                            $"{clientOrServer}-({sceneEvent.ClientId}).");
                        processSceneByName(sceneEvent);
                        break;
                    }
                case SceneEventType.UnloadComplete:
                    {
                        Debug.Log($"Unloaded the {sceneEvent.SceneName} scene on " +
                            $"{clientOrServer}-({sceneEvent.ClientId}).");
                        break;
                    }
                case SceneEventType.LoadEventCompleted:
                    {
                        Debug.Log($"{this.GetType().Name}: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
                        break;
                    }
                case SceneEventType.UnloadEventCompleted:
                    {
                        var loadUnload = sceneEvent.SceneEventType == SceneEventType.LoadEventCompleted ? "Load" : "Unload";
                        Debug.Log($"{loadUnload} event completed for the following client " +
                            $"identifiers:({sceneEvent.ClientsThatCompleted})");
                        if (sceneEvent.ClientsThatTimedOut.Count > 0)
                        {
                            Debug.LogWarning($"{loadUnload} event timed out for the following client " +
                                $"identifiers:({sceneEvent.ClientsThatTimedOut})");
                        }
                        break;
                    }
            }
        }

        private void processSceneByName(SceneEvent sceneEvent)
        {
            Debug.Log($"{this.GetType().Name}: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            if (sceneEvent.SceneName == testScene || sceneEvent.SceneName == "BossTestScene")
            {
                if (IsServer)
                {
                    spawnPlayer(sceneEvent.ClientId);
                }
                else
                {
                    spawnPlayerServerRPC(sceneEvent.ClientId);
                }

                return;
            }
        }

        private void spawnPlayer(ulong clientId)
        {
            Debug.Log($"Spawning Player with clientId: {clientId}");
            GameObject newPlayer;
            newPlayer = (GameObject)Instantiate(PlayerPrefab);
            NetworkObject playerObj = newPlayer.GetComponent<NetworkObject>();
            newPlayer.SetActive(true);
            playerObj.SpawnWithOwnership(clientId, true);
            //GameObject autoAttackObj = Instantiate(AutoAttackPrefab, Vector3.zero, Quaternion.identity);
            //autoAttackObj.GetComponent<NetworkObject>().Spawn();
            //autoAttackObj.transform.parent = playerObj.transform;
            //autoAttackObj.GetComponent<AutoAttack>().Initialize();
            //Player player = newPlayer.GetComponent<Player>();
            //player.SetPlayerID(clientId);
            //player.SetSpawnPosition();
        }

        [ServerRpc(RequireOwnership = false)]
        private void spawnPlayerServerRPC(ulong clientId)
        {
            Debug.Log("Spawning Player RPC");
            spawnPlayer(clientId);
        }

        public void UnloadScene()
        {
            // Assure only the server calls this when the NetworkObject is
            // spawned and the scene is loaded.
            if (!IsServer || !IsSpawned || !m_LoadedScene.IsValid() || !m_LoadedScene.isLoaded)
            {
                return;
            }

            // Unload the scene
            var status = NetworkManager.SceneManager.UnloadScene(m_LoadedScene);
            CheckStatus(status, false);
        }
    }
}