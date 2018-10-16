using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
[CreateAssetMenu]
public class Zone : ScriptableObject
{
    public string zoneName;

    public TileBase[] zoneTiles;

    private List<Vector2Int> bounds;

    private System.Random rnd = new System.Random();

    private const int ScaleFromMap = 1;

    /// <summary>
    /// Initialises the Zone
    /// </summary>
    /// <param name="tiles">the list of tiles the zone contains.</param>
    public void Initialise(List<Vector2Int> tiles)
    {
        bounds = tiles;
    }

    public void Generate(Tilemap map)
    {
        foreach(Vector2Int tile in bounds)
        {
            for (int x = 0; x < ScaleFromMap; x++)
            {
                for (int y = 0; y < ScaleFromMap; y++)
                {
                    map.SetTile(new Vector3Int(tile.x * ScaleFromMap + x,tile.y * ScaleFromMap + y,0), zoneTiles[rnd.Next(0, zoneTiles.Length)]);       
                }
            }
            
        }
    }
}
