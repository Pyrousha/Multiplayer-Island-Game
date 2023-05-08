using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Steamworks;
using UnityEngine;

public class SteamLobby : MonoBehaviour
{
    [SerializeField] private GameObject button;
    private NetworkManager networkManager;

    private const string HostAddressKey = "HostAddress";

    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    private Callback<LobbyEnter_t> lobbyEntered;

    public static CSteamID LobbyID { get; private set; }

    private void Awake()
    {
        networkManager = GetComponent<NetworkManager>();
    }

    private void Start()
    {
        if (!SteamManager.Initialized)
            return;

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    public void HostLobby()
    {
        button.SetActive(false);

        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkManager.maxConnections);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            button.SetActive(true);
            return;
        }

        LobbyID = new CSteamID(callback.m_ulSteamIDLobby);

        networkManager.StartHost();

        SteamMatchmaking.SetLobbyData(LobbyID, HostAddressKey, SteamUser.GetSteamID().ToString());
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (NetworkServer.active)
            return;

        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);

        networkManager.networkAddress = hostAddress;
        networkManager.StartClient();

        button.SetActive(false);
    }
}
