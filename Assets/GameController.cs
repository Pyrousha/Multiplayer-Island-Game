using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    private Player localPlayer;
    public Player LocalPlayer => localPlayer;

    private Player currPlayer;

    public bool LocalPlayerTurn => (localPlayer == currPlayer);

    [SerializeField] private List<PlayerUI> playerUIs;

    [SerializeField] private Color[] playerColors;
    public Color[] Colors => playerColors;
}
