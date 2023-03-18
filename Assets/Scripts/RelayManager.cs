using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace BossArena
{
    public class RelayManager : Singleton<RelayManager>
    {
        [SerializeField]
        private string environment = "production";

        [SerializeField]
        private int maxNumberOfConnections = 10;

        public bool IsRelayEnabled => Transport != null && Transport.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;

        public UnityTransport Transport => NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();

        private string roomCode;

        public async Task<RelayHostData> SetupRelay()
        {
            Debug.Log("Relay Server Starting With Max Connections: {maxNumberOfConnections}");

            InitializationOptions options = new InitializationOptions()
                .SetEnvironmentName(environment);

            await UnityServices.InitializeAsync(options);

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            Allocation allocation = await Relay.Instance.CreateAllocationAsync(maxNumberOfConnections);

            RelayHostData relayHostData = new RelayHostData
            {
                Key = allocation.Key,
                Port = (ushort)allocation.RelayServer.Port,
                AllocationID = allocation.AllocationId,
                AllocationIDBytes = allocation.AllocationIdBytes,
                IPv4Address = allocation.RelayServer.IpV4,
                ConnectionData = allocation.ConnectionData
            };

            relayHostData.JoinCode = await Relay.Instance.GetJoinCodeAsync(relayHostData.AllocationID);
            roomCode = relayHostData.JoinCode;

            Transport.SetRelayServerData(relayHostData.IPv4Address, relayHostData.Port, relayHostData.AllocationIDBytes,
                    relayHostData.Key, relayHostData.ConnectionData);

            Debug.Log($"Relay Server Generated Join Code: {relayHostData.JoinCode}");

            return relayHostData;
        }

        public async Task<RelayJoinData> JoinRelay(string joinCode)
        {
            roomCode = joinCode.ToUpper();
            Debug.Log($"Client Joining Game With Join Code: {roomCode}");

            InitializationOptions options = new InitializationOptions()
                .SetEnvironmentName(environment);

            await UnityServices.InitializeAsync(options);

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            JoinAllocation allocation = await Relay.Instance.JoinAllocationAsync(roomCode);

            RelayJoinData relayJoinData = new RelayJoinData
            {
                Key = allocation.Key,
                Port = (ushort)allocation.RelayServer.Port,
                AllocationID = allocation.AllocationId,
                AllocationIDBytes = allocation.AllocationIdBytes,
                ConnectionData = allocation.ConnectionData,
                HostConnectionData = allocation.HostConnectionData,
                IPv4Address = allocation.RelayServer.IpV4,
                JoinCode = roomCode
            };

            Transport.SetRelayServerData(relayJoinData.IPv4Address, relayJoinData.Port, relayJoinData.AllocationIDBytes,
                relayJoinData.Key, relayJoinData.ConnectionData, relayJoinData.HostConnectionData);

            Debug.Log($"Client Joined Game With Join Code: {roomCode}");

            return relayJoinData;
        }

        public string GetRoomCode()
        {
            return roomCode;
        }
    }
}