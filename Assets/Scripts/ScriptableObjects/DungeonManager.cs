using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonManager")]
public class DungeonManager : ScriptableObject
{
    public string dungeonName;
    public GameObject floorPrefab;
    public GameObject[] wallPrefabs, cornerPrefabs, doorPrefabs, propPrefab;
    public GameObject playerPortal;
    public GameObject nextFloorPortal;
    public GameObject enemySpawner;
    public GameObject ceiling;
    public GameObject chest;
    public GameObject floorLighting;
    public GameObject ceilingLighting;
    public int totalLines;
    public int maxTilesPerLine;
    public int changeDirChance;
    public int doorSpawnChance;
    public int roomSpawnChance;
    public bool allowParallelLinesToTouch;
    public bool generateContinuous;
    public bool generateRooms;
    public bool generateWalls;
    public bool generateCorners;
    public bool onlySpawnDoorsAtHallsEnd;
    public int minRoomLength;
    public int maxRoomLength;
    public int minRoomWidth;
    public int maxRoomWidth;
    public int roomMinDistanceEndOfLine;
    public int roomMinDistanceFromLast;
    public int clutterLimit;
    public int platformLimit;
    public int uniquePlatforms;
    public int enemyPopulationLimit;
    public int chestLimit;

}
