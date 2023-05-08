using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int defaultWidth = 10;
    [SerializeField] private Transform tileParent;
    [Header("Settings")]
    [SerializeField] private int numIslandsPerQuadrant_min;
    [SerializeField] private int numIslandsPerQuadrant_max;
    [SerializeField] private bool mirrorIslands;

    private List<Tile> tiles = new List<Tile>();
    public void SpawnTiles(int _width = -1)
    {
        if (_width <= 0)
            _width = defaultWidth;

        while (tileParent.childCount > 0)
        {
            DestroyImmediate(tileParent.GetChild(0).gameObject);
        }
        tiles = new List<Tile>();

        float min = (_width / -2.0f) + 0.5f;
        float max = (_width / 2.0f) + -0.5f;
        for (float i = min; i <= max; i++)
        {
            for (float j = min; j <= max; j++)
            {
                Tile currTile = Instantiate(tilePrefab).GetComponent<Tile>();
                currTile.gameObject.name = "Tile " + tiles.Count;
                currTile.transform.parent = tileParent;
                currTile.transform.position = new Vector3(i, j, 0);
                tiles.Add(currTile);

                for (int k = 0; k < tiles.Count - 1; k++)
                {
                    Tile tileToCompare = tiles[k];
                    if (Vector3.SqrMagnitude(tileToCompare.transform.position - currTile.transform.position) < 2.5)
                    {
                        tileToCompare.AddAdjacentTile(currTile);
                        currTile.AddAdjacentTile(tileToCompare);
                    }
                }
            }
        }

        #region Islands
        int randMin = 1;
        int randMaxExclusive = Mathf.FloorToInt(_width / 2.0f);

        float numIslandsPerQuadrant = numIslandsPerQuadrant_min;
        if (numIslandsPerQuadrant_max > numIslandsPerQuadrant_min)
            numIslandsPerQuadrant = Random.Range(numIslandsPerQuadrant_min, numIslandsPerQuadrant_max + 1);

        List<Vector2Int> islands = new List<Vector2Int>();
        for (int i = 0; i < 1000; i++) //Just in case to prevent infinite looping
        {
            int x = Random.Range(randMin, randMaxExclusive);
            int y = Random.Range(randMin, randMaxExclusive);

            bool islandAlreadyInList = false;
            foreach (Vector2Int existingIsland in islands)
            {
                if (x == existingIsland.x && y == existingIsland.y)
                {
                    islandAlreadyInList = true;
                    break;
                }
            }
            if (islandAlreadyInList)
                continue;

            islands.Add(new Vector2Int(x, y));
            if (islands.Count >= numIslandsPerQuadrant)
                break;
        }

        if (mirrorIslands)
        {
            for (int i = 0; i < numIslandsPerQuadrant; i++)
            {
                islands.Add(new Vector2Int((_width - 1) - islands[i].x, islands[i].y));
            }
            for (int i = 0; i < numIslandsPerQuadrant * 2; i++)
            {
                islands.Add(new Vector2Int(islands[i].x, (_width - 1) - islands[i].y));
            }
        }
        else
        {
            for (int i = 0; i < 1000; i++) //Just in case to prevent infinite looping
            {
                int x = (_width - 1) - Random.Range(randMin, randMaxExclusive);
                int y = Random.Range(randMin, randMaxExclusive);

                bool islandAlreadyInList = false;
                foreach (Vector2Int existingIsland in islands)
                {
                    if (x == existingIsland.x && y == existingIsland.y)
                    {
                        islandAlreadyInList = true;
                        break;
                    }
                }
                if (islandAlreadyInList)
                    continue;

                islands.Add(new Vector2Int(x, y));
                if (islands.Count >= numIslandsPerQuadrant * 2)
                    break;
            }
            for (int i = 0; i < 1000; i++) //Just in case to prevent infinite looping
            {
                int x = Random.Range(randMin, randMaxExclusive);
                int y = (_width - 1) - Random.Range(randMin, randMaxExclusive);

                bool islandAlreadyInList = false;
                foreach (Vector2Int existingIsland in islands)
                {
                    if (x == existingIsland.x && y == existingIsland.y)
                    {
                        islandAlreadyInList = true;
                        break;
                    }
                }
                if (islandAlreadyInList)
                    continue;

                islands.Add(new Vector2Int(x, y));
                if (islands.Count >= numIslandsPerQuadrant * 3)
                    break;
            }
            for (int i = 0; i < 1000; i++) //Just in case to prevent infinite looping
            {
                int x = (_width - 1) - Random.Range(randMin, randMaxExclusive);
                int y = (_width - 1) - Random.Range(randMin, randMaxExclusive);

                bool islandAlreadyInList = false;
                foreach (Vector2Int existingIsland in islands)
                {
                    if (x == existingIsland.x && y == existingIsland.y)
                    {
                        islandAlreadyInList = true;
                        break;
                    }
                }
                if (islandAlreadyInList)
                    continue;

                islands.Add(new Vector2Int(x, y));
                if (islands.Count >= numIslandsPerQuadrant * 4)
                    break;
            }
        }

        Debug.Log(islands.ToString());
        //Set island tiles based on coords
        foreach (Vector2Int islandCoord in islands)
        {
            int index = islandCoord.x * _width + islandCoord.y;
            tiles[index].SetTileType(Tile.TileTypeEnum.Island);
        }

        #endregion


        #region Border Tiles
        for (float x = min - 1; x <= max + 3; x += max - min + 2)
        {
            for (float y = min; y <= max; y++)
            {
                Tile currTile = Instantiate(tilePrefab).GetComponent<Tile>();
                currTile.gameObject.name = "Tile " + tiles.Count;
                currTile.transform.parent = tileParent;
                currTile.transform.position = new Vector3(x, y, 0);
                currTile.SetTileType(Tile.TileTypeEnum.Border);
                tiles.Add(currTile);
            }
        }

        for (float y = min - 1; y <= max + 3; y += max - min + 2)
        {
            for (float x = min; x <= max; x++)
            {
                Tile currTile = Instantiate(tilePrefab).GetComponent<Tile>();
                currTile.gameObject.name = "Tile " + tiles.Count;
                currTile.transform.parent = tileParent;
                currTile.transform.position = new Vector3(x, y, 0);
                currTile.SetTileType(Tile.TileTypeEnum.Border);
                tiles.Add(currTile);
            }
        }
        #endregion

        //Corner Tiles
        Tile topLeft = Instantiate(tilePrefab, new Vector3(min - 1, max + 1, 0), Quaternion.identity).GetComponent<Tile>();
        Tile topRight = Instantiate(tilePrefab, new Vector3(max + 1, max + 1, 0), Quaternion.identity).GetComponent<Tile>();
        Tile bottomLeft = Instantiate(tilePrefab, new Vector3(min - 1, min - 1, 0), Quaternion.identity).GetComponent<Tile>();
        Tile bottomRight = Instantiate(tilePrefab, new Vector3(max + 1, min - 1, 0), Quaternion.identity).GetComponent<Tile>();

        bottomLeft.AddAdjacentTile(tiles[0]);
        topLeft.AddAdjacentTile(tiles[_width - 1]);
        bottomRight.AddAdjacentTile(tiles[_width * _width - _width]);
        topRight.AddAdjacentTile(tiles[_width * _width - 1]);

        tiles.Add(topLeft);
        tiles.Add(topRight);
        tiles.Add(bottomLeft);
        tiles.Add(bottomRight);

        for (int i = tiles.Count - 4; i < tiles.Count; i++)
        {
            tiles[i].transform.parent = tileParent;
            tiles[i].SetTileType(Tile.TileTypeEnum.PlayerTile);
            tiles[i].SetColor(GameController.Instance.Colors[i - (tiles.Count - 4)]);
        }
    }
}
