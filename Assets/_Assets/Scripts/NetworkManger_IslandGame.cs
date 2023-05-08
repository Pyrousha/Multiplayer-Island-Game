using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class NetworkManger_IslandGame : NetworkManager
{
    [SerializeField] private Transform playerParent;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        CSteamID steamID = SteamMatchmaking.GetLobbyMemberByIndex(SteamLobby.LobbyID, numPlayers - 1);

        PlayerUI_Lobby playerUI = conn.identity.GetComponent<PlayerUI_Lobby>();

        playerUI.SetPlayerIndex(numPlayers - 1);
        playerUI.SetSteamID(steamID.m_SteamID);
    }
}
