using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<Tile> ownedTiles = new List<Tile>();
    private List<Tile> ownedIslands = new List<Tile>();

    public bool IsTileAdjacentToOwnedTiles(Tile _tileToTest)
    {
        foreach (Tile tile in ownedTiles)
        {
            foreach (Tile adjTile in tile.AdjacentTiles)
            {
                if (adjTile == _tileToTest)
                    return true;
            }
        }

        return false;
    }
}
