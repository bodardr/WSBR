using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BetaGenerator : MonoBehaviour {

    //GENERATION PARAMETERS
    public int mapWidth;

    public int mapHeight;

    public string seed;

    System.Random rnd;

    [Range(300,1000)]
    public int minContinentSize;

    [Range(300,100000)]
    public int maxContinentSize;

    [Range(1, 7)]
    public int zoneCount;

    [Range(0, 100)]
    public int fillRatio;

    int[,] map;

    List<List<Vector2Int>> regions = new List<List<Vector2Int>>();

    List<List<Vector2Int>> zoneTiles = new List<List<Vector2Int>>();

    List<Zone> zones = new List<Zone>();

    List<Vector2Int> continent;

    private int minZoneSize;

    public Vector2Int ObjectivePosition { get; private set; }

    public Vector2Int SpawningPosition { get; private set; }

    public void InitialiseSeed()
    {
        rnd = seed == "" ? new System.Random() : new System.Random(seed.GetHashCode());
    }

    public void Generate(Tilemap tilemap)
    {
        bool valid = false;

        int attempts = 0;

        regions = new List<List<Vector2Int>>();

        zoneTiles = new List<List<Vector2Int>>();

        while (!valid && attempts < 20)
        {
            regions = new List<List<Vector2Int>>();

            zoneTiles = new List<List<Vector2Int>>();

            map = new int[mapWidth, mapHeight];

            FillMapRandomly();

            for (int i = 0; i < 4; i++)
            {
                SmoothMap();
            }


            IsolateGround();

            //Verify if the continent is valid with the specified criteria.
            valid = SelectContinent();

            attempts++;
        }

        if (attempts >= 20 && continent == null)
            throw new Exception("Generation incorrect");

        //Calculating minimum zone size.
        minZoneSize = continent.Count / (zoneCount + 1);

        //Finds the zones by picking the continent and seperating it.
        IsolateZones();

        //Creates the zone using the tiles that have been found.
        Zone[] zoneSamples = Resources.LoadAll<Zone>("Zones");

        foreach(List<Vector2Int> zone in zoneTiles)
        {
            Zone newZone = ScriptableObject.Instantiate<Zone>(zoneSamples[rnd.Next(0, zoneSamples.Length)]);
            newZone.Initialise(zone);
            newZone.Generate(tilemap);
            zones.Add(newZone);
        }

        //Finally adds a collider that reflects the ground.
        transform.Find("Map").gameObject.AddComponent<TilemapCollider2D>();

        Vector2Int op = GetObjectivePosition();
        Vector2 opFromTilemap = tilemap.GetCellCenterWorld(new Vector3Int(op.x, op.y, 0));

        ObjectivePosition = new Vector2Int((int)opFromTilemap.x,(int)opFromTilemap.y);

        Vector2Int sp = GetSpawningPosition();
        Vector2 spFromTilemap = tilemap.GetCellCenterWorld(new Vector3Int(sp.x, sp.y, 0));

        SpawningPosition = new Vector2Int((int)spFromTilemap.x, (int)spFromTilemap.y);

        GetSpawningPosition();
    }

    private Vector2Int GetSpawningPosition()
    {
        List<Vector2Int> firstZone = zoneTiles[0];
        return firstZone[firstZone.Count - 1];
    }

    private Vector2Int GetObjectivePosition()
    {
        List<Vector2Int> lastZone = zoneTiles[zoneTiles.Count - 1];
        return lastZone[0];
    }

    private bool SelectContinent()
    {
        List<Vector2Int> potentialContinent = regions.Find(x => minContinentSize < x.Count && x.Count < maxContinentSize);

        if(potentialContinent != null)
        {
            continent = potentialContinent; //We keep the continent.
            return true;
        }

        return false;
    }

    private Vector2Int FindContinentExtremity()
    {
        int[,] zoneFlags = new int[mapWidth, mapHeight];

        foreach (Vector2Int coordinate in continent)
            zoneFlags[coordinate.x, coordinate.y] = (int)TileType.Ground;

        Queue<Vector2Int> coordinatesLeft = new Queue<Vector2Int>();

        coordinatesLeft.Enqueue(continent[0]);
        zoneFlags[continent[0].x,continent[0].y] = (int)TileType.Zone;

        List<Vector2Int> filledContinent = new List<Vector2Int>();

        while (coordinatesLeft.Count > 0)
        {
            Vector2Int currentCoordinates = coordinatesLeft.Dequeue();

            filledContinent.Add(currentCoordinates);

            BoundsInt bounds = new BoundsInt(-1, -1, 0, 3, 3, 1);

            foreach (Vector3Int neighbor in bounds.allPositionsWithin)
            {
                if ((neighbor.x == 0 || neighbor.y == 0) &&
                    CoordinateWithinBounds(neighbor.x + currentCoordinates.x, neighbor.y + currentCoordinates.y))
                {
                    if (map[neighbor.x + currentCoordinates.x, neighbor.y + currentCoordinates.y] == (int)TileType.Ground &&
                        zoneFlags[neighbor.x + currentCoordinates.x, neighbor.y + currentCoordinates.y] == (int)TileType.Ground)
                    {
                        coordinatesLeft.Enqueue(new Vector2Int(neighbor.x + currentCoordinates.x, neighbor.y + currentCoordinates.y));
                        zoneFlags[neighbor.x + currentCoordinates.x, neighbor.y + currentCoordinates.y] = (int)TileType.Zone;
                    }
                }
                else continue;
            }
        }

        return filledContinent[filledContinent.Count - 1];

    }

    private void IsolateGround()
    {
        int[,] zoneFlags = new int[mapWidth, mapHeight];
        
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                zoneFlags[x, y] = map[x, y];
            }
        }

        int zonesLeft = zoneCount;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (zoneFlags[x, y] == (int)TileType.Ground)
                {
                    List<Vector2Int> newZone = GetRegion(x, y, zoneFlags);

                    regions.Add(newZone);

                    foreach(Vector2Int coordinate in newZone)
                    {
                        zoneFlags[coordinate.x, coordinate.y] = (int)TileType.Zone;
                    }
                }
            }
        }
    }

    public void IsolateZones()
    {
        int[,] zoneFlags = new int[mapWidth, mapHeight];

            foreach (Vector2Int coordinate in continent)
                zoneFlags[coordinate.x, coordinate.y] = (int)TileType.Ground;

        zoneTiles.Add(GetZone(FindContinentExtremity(), zoneFlags));
    }

    private List<Vector2Int> GetZone(Vector2Int firstCoordinate, int[,] zoneFlags)
    {
        List<Vector2Int> zone = new List<Vector2Int>();
        Queue<Vector2Int> coordinatesLeft = new Queue<Vector2Int>();
        coordinatesLeft.Enqueue(firstCoordinate);

        zoneFlags[firstCoordinate.x, firstCoordinate.y] = (int)TileType.Zone;

        while (coordinatesLeft.Count > 0 && zone.Count < minZoneSize)
        {
            Vector2Int currentCoordinates = coordinatesLeft.Dequeue();

            zone.Add(currentCoordinates);

            BoundsInt bounds = new BoundsInt(-1, -1, 0, 3, 3, 1);

            foreach (Vector3Int neighbor in bounds.allPositionsWithin)
            {
                if ((neighbor.x == 0 || neighbor.y == 0) &&
                    CoordinateWithinBounds(neighbor.x + currentCoordinates.x, neighbor.y + currentCoordinates.y))
                {
                    if (map[neighbor.x + currentCoordinates.x, neighbor.y + currentCoordinates.y] == (int)TileType.Ground &&
                        zoneFlags[neighbor.x + currentCoordinates.x, neighbor.y + currentCoordinates.y] == (int)TileType.Ground)
                    {
                        coordinatesLeft.Enqueue(new Vector2Int(neighbor.x + currentCoordinates.x, neighbor.y + currentCoordinates.y));
                        zoneFlags[neighbor.x + currentCoordinates.x, neighbor.y + currentCoordinates.y] = (int)TileType.Zone;
                    }
                }
                else continue;
            }
        }

        while (coordinatesLeft.Count > 0)
        {
            List<Vector2Int> potentialZone = GetZone(coordinatesLeft.Dequeue(), zoneFlags);
            if (potentialZone.Count >= minZoneSize)
                zoneTiles.Add(potentialZone);
            else
                zone.AddRange(potentialZone);
        }            
                
        return zone;
    }

    private List<Vector2Int> GetRegion(int x, int y, int[,] zoneFlags)
    {
        List<Vector2Int> zone = new List<Vector2Int>();
        Queue<Vector2Int> coordinatesLeft = new Queue<Vector2Int>();

        coordinatesLeft.Enqueue(new Vector2Int(x, y));
        zoneFlags[x, y] = (int)TileType.Zone;

        while(coordinatesLeft.Count > 0)
        {
            Vector2Int currentCoordinates = coordinatesLeft.Dequeue();

            zone.Add(currentCoordinates);

            BoundsInt bounds = new BoundsInt(-1, -1, 0, 3, 3, 1);

            foreach(Vector3Int neighbor in bounds.allPositionsWithin)
            {
                if ((neighbor.x == 0 || neighbor.y == 0) && 
                    CoordinateWithinBounds(neighbor.x + currentCoordinates.x ,neighbor.y + currentCoordinates.y))
                {
                    if(map[neighbor.x + currentCoordinates.x, neighbor.y + currentCoordinates.y] == (int)TileType.Ground && 
                        zoneFlags[neighbor.x + currentCoordinates.x, neighbor.y + currentCoordinates.y] == (int)TileType.Ground)
                    {
                        coordinatesLeft.Enqueue(new Vector2Int(neighbor.x + currentCoordinates.x, neighbor.y + currentCoordinates.y));
                        zoneFlags[neighbor.x + currentCoordinates.x, neighbor.y + currentCoordinates.y] = (int)TileType.Zone;
                    }
                }
                else continue;
            }
        }

        return zone;
    }

    private bool CoordinateWithinBounds(int x, int y)
    {
        return x > 0 && x < mapWidth && y > 0 && y < mapHeight;
    }

    private void SmoothMap()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                int neighborCount = CountNeighboringTiles(x, y);

                if (neighborCount > 3)
                    map[x, y] = (int)TileType.Ground;
                else if (neighborCount < 3)
                    map[x, y] = (int)TileType.Empty;
            }
        }
    }

    private int CountNeighboringTiles(int x, int y)
    {
        BoundsInt bounds = new BoundsInt(-1, -1, 0, 3, 3, 1);

        int neighborCount = 0;

        foreach(Vector3Int neighbor in bounds.allPositionsWithin)
        {
            if (neighbor.x == x && neighbor.y == y)
                continue;

            if (!CoordinateWithinBounds(x + neighbor.x, y + neighbor.y))
                continue;

            if (map[x + neighbor.x, y + neighbor.y] == (int)TileType.Ground)
                neighborCount++;
        }

        return neighborCount;
    }

    private void FillMapRandomly()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (rnd.Next(0, 100) < fillRatio)
                    map[x, y] = (int)TileType.Ground;
                //else map[x, y] = (int)TileType.Empty; IMPLICIT since default value is 0.
            }
        }
    }

    public enum TileType
    {
        Empty = 0,
        Ground = 1,
        Zone = 2
    }
}
