using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI_Lobby : NetworkBehaviour
{
    [SyncVar]
    private int playerIndex = -1;

    [Server]
    public void SetPlayerIndex(int _playerIndex)
    {
        playerIndex = _playerIndex;
    }

    [SyncVar(hook = nameof(HandleSteamIDUpdated))]
    private ulong steamID;

    [SerializeField] private RawImage playerIcon;

    [SerializeField] private TextMeshProUGUI playerName;

    protected Callback<AvatarImageLoaded_t> avatarImageLoaded;
    #region Server
    public void SetSteamID(ulong _steamID)
    {
        steamID = _steamID;
    }
    #endregion

    #region Client
    public override void OnStartClient()
    {
        avatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);
        Debug.Log("playerIndex: " + playerIndex);
        playerName.color = GameController.Instance.Colors[playerIndex];

        base.OnStartClient();
    }

    private void OnAvatarImageLoaded(AvatarImageLoaded_t callback)
    {
        if (callback.m_steamID.m_SteamID != steamID)
            return;

        playerIcon.texture = GetSteamImageAsTexture(callback.m_iImage);
    }

    private void HandleSteamIDUpdated(ulong oldSteamID, ulong newSteamID)
    {
        CSteamID steamID = new CSteamID(newSteamID);

        playerName.text = SteamFriends.GetFriendPersonaName(steamID);

        int imageID = SteamFriends.GetLargeFriendAvatar(steamID);
        if (imageID == -1)
            return;

        playerIcon.texture = GetSteamImageAsTexture(imageID);
    }

    private Texture2D GetSteamImageAsTexture(int imageID)
    {
        Texture2D texure = null;

        bool isValid = SteamUtils.GetImageSize(imageID, out uint width, out uint height);

        if (isValid)
        {
            byte[] image = new byte[width * height * 4];

            isValid = SteamUtils.GetImageRGBA(imageID, image, (int)(width * height * 4));

            if (isValid)
            {
                texure = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                texure.LoadRawTextureData(image);
                texure.Apply();
            }
        }

        return texure;
    }
    #endregion
}
