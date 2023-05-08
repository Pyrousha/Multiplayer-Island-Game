using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprRend;


    public enum TileTypeEnum
    {
        Border,
        Water,
        Island,
        PlayerTile,
        Embargo,
        CannonedTile
    }

    private TileTypeEnum typeType;

    private Player ownedPlayer;
    private Player lastOwnedPlayer; //Currently only used for cannonballs

    [SerializeField] private List<Tile> adjacentTiles = new List<Tile>();
    public List<Tile> AdjacentTiles => adjacentTiles;


    public void SetTileType(TileTypeEnum _type)
    {
        typeType = _type;
        sprRend.sprite = GameController.Instance.TileSprites[(int)_type];
    }

    public void SetOwnedPlayer(Player _player)
    {
        if (_player == ownedPlayer)
            return;

        lastOwnedPlayer = ownedPlayer;
        ownedPlayer = _player;
    }

    void OnClicked()
    {
        if (!GameController.Instance.LocalPlayerTurn)
            return;

        switch (typeType)
        {
            case TileTypeEnum.Border:
                break;
            case TileTypeEnum.Water:
                break;
            case TileTypeEnum.Island:
                break;
            case TileTypeEnum.Embargo:
                break;
            case TileTypeEnum.PlayerTile:
                break;
            case TileTypeEnum.CannonedTile:
                break;
        }
    }

    public void AddAdjacentTile(Tile _adjTile)
    {
        if (adjacentTiles.Contains(_adjTile))
        {
            Debug.LogError("Cannot add existing tile to addjacent ones");
            return;
        }
        adjacentTiles.Add(_adjTile);
    }

    public void SetColor(Color _color)
    {
        sprRend.color = _color;
    }
}
