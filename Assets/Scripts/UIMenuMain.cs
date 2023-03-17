using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Netcode;

public class UIMenuMain : MonoBehaviour
{

    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    public async void HostLobby()
    {
        Debug.Log("Hosting Lobby");
        // this allows the UnityMultiplayer and UnityMultiplayerRelay scene to work with and without
        // relay features - if the Unity transport is found and is relay protocol then we redirect all the 
        // traffic through the relay, else it just uses a LAN type (UNET) communication.
        if (RelayManager.Instance.IsRelayEnabled)
            await RelayManager.Instance.SetupRelay();

        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("Host started...");
            //activateRacingUI();
            //enableRaceScene();
        }
        else
        {
            Debug.Log("Unable to start host...");
        }

    }

    public async void JoinLobby()
    {
        Debug.Log("Joinging Lobby");
        NetworkManager.Singleton.StartClient();
    }
}
