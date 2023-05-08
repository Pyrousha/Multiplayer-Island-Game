using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int defaultWidth = 10;
    [SerializeField] private Transform tileParent;

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
        }
    }
}
