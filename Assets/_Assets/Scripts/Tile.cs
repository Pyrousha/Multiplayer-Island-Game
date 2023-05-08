using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileTypeEnum
    {
        Border,
        Water,
        Island,
        Embargo,
        PlayerTile,
        CannonedTile
    }

    private TileTypeEnum state;

    private Player ownedPlayer;
    private Player lastOwnedPlayer; //Currently only used for cannonballs

    [SerializeField] private List<Tile> adjacentTiles = new List<Tile>();
    public List<Tile> AdjacentTiles => adjacentTiles;

    void OnClicked()
    {
        if (!GameController.Instance.LocalPlayerTurn)
            return;

        switch (state)
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
}
