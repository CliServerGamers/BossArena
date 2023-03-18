using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BossArena.lobby;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BossArena
{
    /// <summary>
    /// Current state of the local game.
    /// Set as a flag to allow for the Inspector to select multiple valid states for various UI features.
    /// </summary>
    [Flags]
    public enum GameState
    {
        Menu = 1,
        Lobby = 2,
        JoinMenu = 4,
    }

    /// <summary>
    /// Sets up and runs the entire sample.
    /// All the Data that is important gets updated in here, the GameManager in the mainScene has all the references
    /// needed to run the game.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        public LocalLobby LocalLobby => m_LocalLobby;
        //public Action<GameState> onGameStateChanged;
        //public LocalLobbyList LobbyList { get; private set; } = new LocalLobbyList();

        public GameState LocalGameState { get; private set; }
        public LobbyManager LobbyManager { get; private set; }
        //[SerializeField]
        //SetupInGame m_setupInGame;
        //[SerializeField]
        //Countdown m_countdown;

        LocalPlayer m_LocalUser;
        LocalLobby m_LocalLobby;

        LobbyColor m_lobbyColorFilter;

        static GameManager m_GameManagerInstance;

        public static GameManager Instance
        {
            get
            {
                if (m_GameManagerInstance != null)
                    return m_GameManagerInstance;
                m_GameManagerInstance = FindObjectOfType<GameManager>();
                return m_GameManagerInstance;
            }
        }

        /// <summary>Rather than a setter, this is usable in-editor. It won't accept an enum, however.</summary>
        public void SetLobbyColorFilter(int color)
        {
            m_lobbyColorFilter = (LobbyColor)color;
        }

        public async Task<LocalPlayer> AwaitLocalUserInitialization()
        {
            while (m_LocalUser == null)
                await Task.Delay(100);
            return m_LocalUser;
        }

        public async void CreateLobby(string name, bool isPrivate, int maxPlayers = 4)
        {
            try
            {
                var lobby = await LobbyManager.CreateLobbyAsync(
                    name,
                    maxPlayers,
                    isPrivate, m_LocalUser);

                LobbyConverters.RemoteToLocal(lobby, m_LocalLobby);
                await CreateLobby();
            }
            catch (Exception exception)
            {
                //SetGameState(GameState.JoinMenu);
                Debug.LogError($"Error creating lobby : {exception} ");
            }
        }

        public async void JoinLobby(string lobbyID, string lobbyCode)
        {
            try
            {
                var lobby = await LobbyManager.JoinLobbyAsync(lobbyID, lobbyCode,
                    m_LocalUser);

                LobbyConverters.RemoteToLocal(lobby, m_LocalLobby);
                await JoinLobby();
            }
            catch (Exception exception)
            {
                //SetGameState(GameState.JoinMenu);
                Debug.LogError($"Error joining lobby : {exception} ");
            }
        }

        //public async void QueryLobbies()
        //{
        //    LobbyList.QueryState.Value = LobbyQueryState.Fetching;
        //    var qr = await LobbyManager.RetrieveLobbyListAsync(m_lobbyColorFilter);
        //    if (qr == null)
        //    {
        //        return;
        //    }

        //    SetCurrentLobbies(LobbyConverters.QueryToLocalList(qr));
        //}

        public async void QuickJoin()
        {
            var lobby = await LobbyManager.QuickJoinLobbyAsync(m_LocalUser, m_lobbyColorFilter);
            if (lobby != null)
            {
                LobbyConverters.RemoteToLocal(lobby, m_LocalLobby);
                await JoinLobby();
            }
            else
            {
                //SetGameState(GameState.JoinMenu);
            }
        }

        public void SetLocalUserName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                LogHandlerSettings.Instance.SpawnErrorPopup(
                    "Empty Name not allowed."); // Lobby error type, then HTTP error type.
                return;
            }

            m_LocalUser.DisplayName.Value = name;
            SendLocalUserData();
        }

        //public void SetLocalUserEmote(EmoteType emote)
        //{
        //    m_LocalUser.Emote.Value = emote;
        //    SendLocalUserData();
        //}

        public void SetLocalUserStatus(PlayerStatus status)
        {
            m_LocalUser.UserStatus.Value = status;
            SendLocalUserData();
        }

        public void SetLocalLobbyColor(int color)
        {
            if (m_LocalLobby.PlayerCount < 1)
                return;
            m_LocalLobby.LocalLobbyColor.Value = (LobbyColor)color;
            SendLocalLobbyData();
        }

        bool updatingLobby;

        async void SendLocalLobbyData()
        {
            await LobbyManager.UpdateLobbyDataAsync(LobbyConverters.LocalToRemoteLobbyData(m_LocalLobby));
        }

        async void SendLocalUserData()
        {
            await LobbyManager.UpdatePlayerDataAsync(LobbyConverters.LocalToRemoteUserData(m_LocalUser));
        }

        public void UIChangeMenuState(GameState state)
        {
            var isQuittingGame = LocalGameState == GameState.Lobby &&
                m_LocalLobby.LocalLobbyState.Value == LobbyState.InGame;

            if (isQuittingGame)
            {
                //If we were in-game, make sure we stop by the lobby first
                state = GameState.Lobby;
                //ClientQuitGame();
            }
            //SetGameState(state);
        }

        public void HostSetRelayCode(string code)
        {
            m_LocalLobby.RelayCode.Value = code;
            SendLocalLobbyData();
        }

        //Only Host needs to listen to this and change state.
        void OnPlayersReady(int readyCount)
        {
            if (readyCount == m_LocalLobby.PlayerCount &&
                m_LocalLobby.LocalLobbyState.Value != LobbyState.CountDown)
            {
                m_LocalLobby.LocalLobbyState.Value = LobbyState.CountDown;
                SendLocalLobbyData();
            }
            else if (m_LocalLobby.LocalLobbyState.Value == LobbyState.CountDown)
            {
                m_LocalLobby.LocalLobbyState.Value = LobbyState.Lobby;
                SendLocalLobbyData();
            }
        }

        //void OnLobbyStateChanged(LobbyState state)
        //{
        //    if (state == LobbyState.Lobby)
        //        CancelCountDown();
        //    if (state == LobbyState.CountDown)
        //        BeginCountDown();
        //}

        //void BeginCountDown()
        //{
        //    Debug.Log("Beginning Countdown.");
        //    m_countdown.StartCountDown();
        //}

        //void CancelCountDown()
        //{
        //    Debug.Log("Countdown Cancelled.");
        //    m_countdown.CancelCountDown();
        //}

        //public void FinishedCountDown()
        //{
        //    m_LocalUser.UserStatus.Value = PlayerStatus.InGame;
        //    m_LocalLobby.LocalLobbyState.Value = LobbyState.InGame;
        //    m_setupInGame.StartNetworkedGame(m_LocalLobby, m_LocalUser);
        //}

        //public void BeginGame()
        //{
        //    if (m_LocalUser.IsHost.Value)
        //    {
        //        m_LocalLobby.LocalLobbyState.Value = LobbyState.InGame;
        //        m_LocalLobby.Locked.Value = true;
        //        SendLocalLobbyData();
        //    }
        //}

        //public void ClientQuitGame()
        //{
        //    EndGame();
        //    m_setupInGame?.OnGameEnd();
        //}

        //public void EndGame()
        //{
        //    if (m_LocalUser.IsHost.Value)
        //    {
        //        m_LocalLobby.LocalLobbyState.Value = LobbyState.Lobby;
        //        m_LocalLobby.Locked.Value = false;
        //        SendLocalLobbyData();
        //    }

        //    SetLobbyView();
        //}

        #region Setup

        async void Awake()
        {
            Application.wantsToQuit += OnWantToQuit;
            m_LocalUser = new LocalPlayer("", 0, false, "LocalPlayer");
            m_LocalLobby = new LocalLobby { LocalLobbyState = { Value = LobbyState.Lobby } };
            LobbyManager = new LobbyManager();

            await InitializeServices();
            AuthenticatePlayer();
        }

        async Task InitializeServices()
        {
            string serviceProfileName = "player";
            await Auth.Authenticate(serviceProfileName);
        }

        void AuthenticatePlayer()
        {
            var localId = AuthenticationService.Instance.PlayerId;
            var randomName = NameGenerator.GetName(localId);

            m_LocalUser.ID.Value = localId;
            m_LocalUser.DisplayName.Value = randomName;
        }

        #endregion

        //void SetGameState(GameState state)
        //{
        //    var isLeavingLobby = (state == GameState.Menu || state == GameState.JoinMenu) &&
        //        LocalGameState == GameState.Lobby;
        //    LocalGameState = state;

        //    Debug.Log($"Switching Game State to : {LocalGameState}");

        //    if (isLeavingLobby)
        //        LeaveLobby();
        //    onGameStateChanged.Invoke(LocalGameState);
        //}

        void SetCurrentLobbies(IEnumerable<LocalLobby> lobbies)
        {
            var newLobbyDict = new Dictionary<string, LocalLobby>();
            foreach (var lobby in lobbies)
                newLobbyDict.Add(lobby.LobbyID.Value, lobby);

            //LobbyList.CurrentLobbies = newLobbyDict;
            //LobbyList.QueryState.Value = LobbyQueryState.Fetched;
        }

        async Task CreateLobby()
        {
            m_LocalUser.IsHost.Value = true;
            m_LocalLobby.onUserReadyChange = OnPlayersReady;
            try
            {
                await BindLobby();
            }
            catch (Exception exception)
            {
                Debug.LogError($"Couldn't join Lobby: {exception}");
            }
        }

        async Task JoinLobby()
        {
            //Trigger UI Even when same value
            m_LocalUser.IsHost.ForceSet(false);
            await BindLobby();
        }

        async Task BindLobby()
        {
            await LobbyManager.BindLocalLobbyToRemote(m_LocalLobby.LobbyID.Value, m_LocalLobby);
            //m_LocalLobby.LocalLobbyState.onChanged += OnLobbyStateChanged;
            SetLobbyView();
        }

        public void LeaveLobby()
        {
            m_LocalUser.ResetState();
#pragma warning disable 4014
            LobbyManager.LeaveLobbyAsync();
#pragma warning restore 4014
            ResetLocalLobby();
        }


        IEnumerator RetryConnection(Action doConnection, string lobbyId)
        {
            yield return new WaitForSeconds(5);
            if (m_LocalLobby != null && m_LocalLobby.LobbyID.Value == lobbyId && !string.IsNullOrEmpty(lobbyId)
               ) // Ensure we didn't leave the lobby during this waiting period.
                doConnection?.Invoke();
        }

        void SetLobbyView()
        {
            Debug.Log($"Setting Lobby user state {GameState.Lobby}");
            //SetGameState(GameState.Lobby);
            SetLocalUserStatus(PlayerStatus.Lobby);
            SceneManager.LoadScene("LobbyScene", LoadSceneMode.Additive);
        }

        void ResetLocalLobby()
        {
            m_LocalLobby.ResetLobby();
            m_LocalLobby.RelayServer = null;
        }

        #region Teardown

        /// <summary>
        /// In builds, if we are in a lobby and try to send a Leave request on application quit, it won't go through if we're quitting on the same frame.
        /// So, we need to delay just briefly to let the request happen (though we don't need to wait for the result).
        /// </summary>
        IEnumerator LeaveBeforeQuit()
        {
            ForceLeaveAttempt();
            yield return null;
            Application.Quit();
        }

        bool OnWantToQuit()
        {
            bool canQuit = string.IsNullOrEmpty(m_LocalLobby?.LobbyID.Value);
            StartCoroutine(LeaveBeforeQuit());
            return canQuit;
        }

        void OnDestroy()
        {
            ForceLeaveAttempt();
            LobbyManager.Dispose();
        }

        void ForceLeaveAttempt()
        {
            if (!string.IsNullOrEmpty(m_LocalLobby?.LobbyID.Value))
            {
#pragma warning disable 4014
                LobbyManager.LeaveLobbyAsync();
#pragma warning restore 4014
                m_LocalLobby = null;
            }
        }

        #endregion
    }
}