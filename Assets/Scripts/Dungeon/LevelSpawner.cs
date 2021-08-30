using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class LevelSpawner : MonoBehaviour
{
	
	public GameObject player;
	public GameObject fade;
	
	public int floor;
	
	[SerializeField] private DungeonManager[] dmThemes;
    public DungeonManager dm;
    [SerializeField] private DungeonGenerator generator;
    private GameObject tile, walledTile, playerSpawn, nextFloorPortal, enemySpawn, propLocation;

    public List<Vector3> destinations {get; set;}
    
    void Awake()
    {
		dm = dmThemes[0];
		UpdateDM();
		
        generator = GetComponent<DungeonGenerator>();
        GenerateBaseLevel();
        GenerateCeiling();
        GenerateEnemyDestinations();
        GeneratePlatforms();
        GenerateNavMesh();
    }

    void Start()
    {
        GeneratePlayerSpawn();
        MovePlayerToSpawn();
        GenerateChests();
        GenerateProps();
        GenerateEnemy();
        GenerateNextFloorPortal();
    }


    private void GenerateNavMesh() {
        GameObject.FindGameObjectWithTag("Big Rock").GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    private void GenerateEnemyDestinations() {
        destinations = new List<Vector3>();
        List<GameObject> tiles = new List<GameObject>();
        tiles.AddRange(GameObject.FindGameObjectsWithTag("Tile"));

        for (int i = 0; i < tiles.Count; i++) {
            destinations.Add(tiles[i].transform.position);
        }

        Debug.Log(destinations.Count);
    }

    private void GenerateBaseLevel()
    {
        generator.name = dm.dungeonName;
        generator.tilePrefab = dm.floorPrefab;
        generator.doorPrefabs = dm.doorPrefabs;
        generator.wallPrefabs = dm.wallPrefabs;
        generator.cornerPrefabs = dm.cornerPrefabs;

        generator.EasyGen(dm.totalLines, dm.maxTilesPerLine, dm.changeDirChance, dm.doorSpawnChance, dm.roomSpawnChance,
            dm.allowParallelLinesToTouch, dm.generateContinuous, dm.generateRooms, dm.generateWalls, dm.generateCorners,
            dm.onlySpawnDoorsAtHallsEnd, dm.minRoomLength, dm.maxRoomLength, dm.minRoomWidth, dm.maxRoomWidth,
            dm.roomMinDistanceEndOfLine, dm.roomMinDistanceFromLast);
    }

    private void GeneratePlatforms()
    {
        GameObject dungeon = generator.dungeonParent.gameObject;
        bool safeToSpawn = true;
        int wallCount = generator.allWalls.Count;
        int localPlatformLimit = dm.platformLimit + wallCount;
        Debug.Log("platform limit: " + localPlatformLimit);

        for (int i = 0; i < localPlatformLimit; i++)
        {
            do 
            { 
                tile = dungeon.transform.GetChild((int)UnityEngine.Random.Range(0, dungeon.transform.childCount)).gameObject;
                if (tile.transform.childCount > 1) {
                    foreach (Transform child in tile.transform)
                    {
                        if (child.gameObject.tag == "Big Rock") {
                            safeToSpawn = false;
                            break;
                        }
                    }
                }
            } while (tile.GetComponent("DummyTileComponent") as DummyTileComponent == null && !safeToSpawn);
            Destroy(tile.GetComponent("DummyTileComponent") as DummyTileComponent);
            
            if (safeToSpawn) propLocation = Instantiate(dm.propPrefab[(int)UnityEngine.Random.Range(dm.propPrefab.Length - dm.uniquePlatforms, dm.propPrefab.Length)], tile.transform, false);
            safeToSpawn = true;
        }
    }

    private void GenerateProps()
    {
        GameObject dungeon = generator.dungeonParent.gameObject;
        int tileCount = generator.spawnedTiles.Count;
        int localClutterLimit = dm.clutterLimit + tileCount;
        Debug.Log("clutter limit: " + localClutterLimit);
        bool isOvergrownLevel = dm.floorPrefab.name == "DungeonTile_Toxin";

        bool safeToSpawn =  false;
        Vector3 propSpawnCoordinates = new Vector3(0, 0, 0);
        Quaternion propSpawnRotation = new Quaternion(0f, 0f, 0f, 0f);
        GameObject tempPlatform = null;
        float height = 0f;

        for (int i = 0; i < localClutterLimit; i++)
        {
            do 
            { 
                tile = dungeon.transform.GetChild((int)UnityEngine.Random.Range(0, dungeon.transform.childCount)).gameObject;
                if (tile.transform.childCount > 0) {
                    foreach (Transform child in tile.transform)
                    {
                        if (isOvergrownLevel) 
                        {
                            safeToSpawn = true;
                            height = child.gameObject.transform.GetComponent<BoxCollider>().size.y * 2;
                            tempPlatform = child.gameObject;
                            float width = UnityEngine.Random.Range(-4.5f, 4.5f); 
                            float length = UnityEngine.Random.Range(-4.5f, 4.5f);
                            propSpawnCoordinates = new Vector3(child.gameObject.transform.position.x + width, height, child.gameObject.transform.position.z + length);
                            propSpawnRotation = new Quaternion(child.gameObject.transform.rotation.x, child.gameObject.transform.rotation.y, child.gameObject.transform.rotation.z, child.gameObject.transform.rotation.w);
                            break;

                        } else if (child.gameObject.tag == "Big Rock") {
                            safeToSpawn = true;
                            height = child.gameObject.transform.GetComponent<BoxCollider>().size.y * 2;
                            tempPlatform = child.gameObject;
                            float width = UnityEngine.Random.Range(-4.5f, 4.5f); 
                            float length = UnityEngine.Random.Range(-4.5f, 4.5f);
                            propSpawnCoordinates = new Vector3(child.gameObject.transform.position.x + width, height, child.gameObject.transform.position.z + length);
                            propSpawnRotation = new Quaternion(child.gameObject.transform.rotation.x, child.gameObject.transform.rotation.y, child.gameObject.transform.rotation.z, child.gameObject.transform.rotation.w);
                            break;
                        }
                    }
                }
            } while (tile.GetComponent("DummyTileComponent") as DummyTileComponent == null && !safeToSpawn);
            Destroy(tile.GetComponent("DummyTileComponent") as DummyTileComponent);

            if (safeToSpawn) 
            {
                propLocation = Instantiate(dm.propPrefab[(int)UnityEngine.Random.Range(0, dm.propPrefab.Length - dm.uniquePlatforms)], propSpawnCoordinates, propSpawnRotation);
                propLocation.transform.SetParent(tempPlatform.transform);
                propLocation.transform.localPosition = new Vector3(propLocation.transform.localPosition.x, height, propLocation.transform.localPosition.z);
                safeToSpawn = false;
            }

            if (i == 0 && dm.floorLighting != null & dm.ceilingLighting != null) {
                Instantiate(dm.floorLighting, dm.floorLighting.transform.position, dm.floorLighting.transform.rotation);
                Instantiate(dm.ceilingLighting, dm.ceilingLighting.transform.position, dm.ceilingLighting.transform.rotation);
            }
        }
    }

    
    private void GenerateChests()
    {
        GameObject dungeon = generator.dungeonParent.gameObject;
        bool safeToSpawn =  false;
        Vector3 chestSpawnPosition = new Vector3(0, 0, 0);
        Quaternion chestSpawnRotation = new Quaternion(0f, 0f, 0f, 0f);
        float height = 0f;
        GameObject tempPlatform = null;

        for (int i = 0; i < dm.chestLimit; i++)
        {
            do 
            { 
                tile = dungeon.transform.GetChild((int)UnityEngine.Random.Range(0, dungeon.transform.childCount)).gameObject;
                if (tile.transform.childCount > 0) {
                    foreach (Transform child in tile.transform)
                    {
                        if (child.gameObject.tag == "Big Rock") {
                            safeToSpawn = true;
                            tempPlatform = child.gameObject;
                            height = child.gameObject.transform.GetComponent<BoxCollider>().size.y * 2;
                            chestSpawnPosition = new Vector3(child.transform.position.x, height, child.transform.position.z);
                            chestSpawnRotation = new Quaternion(child.transform.rotation.x, child.transform.rotation.y, child.transform.rotation.z, child.transform.rotation.w);
                            break;
                        }
                    }
                }

            } while (tile.GetComponent("DummyTileComponent") as DummyTileComponent != null && !safeToSpawn);
            Destroy(tile.GetComponent("DummyTileComponent") as DummyTileComponent);

            if (safeToSpawn) {
                GameObject chest = Instantiate(dm.chest, chestSpawnPosition, chestSpawnRotation);
                chest.transform.SetParent(tempPlatform.transform);
                chest.transform.localPosition = new Vector3(chest.transform.localPosition.x, height, chest.transform.localPosition.z);

                safeToSpawn = false;
            }
        }
    }

    private void GeneratePlayerSpawn()
    {
        GameObject dungeon = generator.dungeonParent.gameObject;
        Vector3 propSpawnCoordinates = new Vector3(0, 0, 0);
        Quaternion propSpawnRotation = new Quaternion(0f, 0f, 0f, 0f);
        bool safeToSpawn = false;

        do { 
            tile = dungeon.transform.GetChild((int)UnityEngine.Random.Range(0, dungeon.transform.childCount)).gameObject;
            if (tile.transform.childCount > 2) {
                foreach (Transform child in tile.transform)
                {
                    if (child.gameObject.tag == "Big Rock" || child.gameObject.tag == "Small Rock") {

                        propSpawnCoordinates = new Vector3(child.gameObject.transform.position.x, 5f, child.gameObject.transform.position.z);
                        propSpawnRotation = new Quaternion(child.gameObject.transform.rotation.x, child.gameObject.transform.rotation.y, child.gameObject.transform.rotation.z, child.gameObject.transform.rotation.w);
                        safeToSpawn = true;
                        break;
                    }
                }
            }
        } while (!safeToSpawn);

        playerSpawn = Instantiate(dm.playerPortal, propSpawnCoordinates, propSpawnRotation);
        Debug.Log("Player Spawn is at " + playerSpawn.transform.position);
    }

    private void GenerateCeiling()
    {
        GameObject dungeon = generator.dungeonParent.gameObject;
        for (int i = 0; i < dungeon.transform.childCount; i++)
        {
            GameObject ceiling = Instantiate(dm.ceiling, dungeon.transform.GetChild(i).transform, false);
            ceiling.transform.localPosition = new Vector3(0, 10.0f, 0);
        }
    }

    private void GenerateEnemy()
    {
        GameObject dungeon = generator.dungeonParent.gameObject;
        int wallCount = generator.allWalls.Count;
        int localEnemyLimit = dm.enemyPopulationLimit + (int)(wallCount / 5);
        Debug.Log("enemy count is: " + localEnemyLimit);


        for (int i = 0; i < localEnemyLimit; i++)
        {
            tile = dungeon.transform.GetChild((int)UnityEngine.Random.Range(0, dungeon.transform.childCount)).gameObject;
            
            enemySpawn = Instantiate(dm.enemySpawner, tile.transform, false);
            enemySpawn.transform.localPosition = new Vector3(0, 0.25f, 0);

            enemySpawn.GetComponent<EnemySpawner>().EnemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyPool>();
            enemySpawn.GetComponent<EnemySpawner>().parent = dungeon;
            enemySpawn.GetComponent<EnemySpawner>().player = GameObject.Find("Player").gameObject;
        }
    }

    private void GenerateNextFloorPortal()
    {
        GameObject dungeon = generator.dungeonParent.gameObject;
        bool safeToSpawn = false;
        Vector3 portalSpawnPosition = new Vector3(0, 0, 0);
        Quaternion portalSpawnRotation = new Quaternion(0f, 0f, 0f, 0f);
        float height = 0f;

        do 
        {
            tile = dungeon.transform.GetChild((int)UnityEngine.Random.Range(0, dungeon.transform.childCount)).gameObject;
            if (tile.transform.childCount > 0) {
                foreach (Transform child in tile.transform)
                {
                    if (child.gameObject.tag == "Big Rock") {
                        safeToSpawn = true;
                        child.gameObject.tag = "Portal Rock";
                        height = child.gameObject.transform.GetComponent<BoxCollider>().size.y * 2;
                        portalSpawnPosition = new Vector3(child.gameObject.transform.position.x, child.gameObject.transform.position.y + 3.5f, child.gameObject.transform.position.z);
                        portalSpawnRotation = new Quaternion(child.gameObject.transform.rotation.x, child.gameObject.transform.rotation.y, child.gameObject.transform.rotation.z, child.gameObject.transform.rotation.w);
                        break;
                    }
                }
            }
        } while (!safeToSpawn);

        if (safeToSpawn) {
            nextFloorPortal = Instantiate(dm.nextFloorPortal, portalSpawnPosition, portalSpawnRotation);
            nextFloorPortal.transform.SetParent(tile.transform);
            safeToSpawn = false;
        }
    }


    private void MovePlayerToSpawn()
    {
        GameObject player = GameObject.Find("/Player/");
		player.SetActive(false);
		player.transform.SetPositionAndRotation(playerSpawn.transform.position, playerSpawn.transform.rotation);
        player.SetActive(true);
		
		Debug.Log("Player has entered next level at position " + GameObject.Find("/Player/").transform.position);
    }

    private Boolean IsFacingWall(GameObject thisObject)
    {
        RaycastHit hit;
        
        if (Physics.Raycast(thisObject.transform.position, thisObject.transform.forward, out hit))
        {
            if (hit.collider.CompareTag("Wall")) return true;
        }
        return false;
    }
	
	private void UpdateDM() {
		if(floor > 0 && floor < 6)  { dm = dmThemes[0]; }
		if(floor > 5 && floor < 11) { dm = dmThemes[1]; }
		if(floor > 10 && floor < 16) { dm = dmThemes[2]; }
	}
	
    IEnumerator GoToNextLevel()
    {
		floor++;
		UpdateDM();
		
		fade.GetComponent<Fade>().FadeOut();
		
		yield return new WaitForSeconds(0.5f);
        generator.DeleteDungeon();

        yield return new WaitForSeconds(0.2f);
        GenerateBaseLevel();

        yield return new WaitForSeconds(0.2f);
        GenerateCeiling();

        GenerateEnemyDestinations();

        yield return new WaitForSeconds(0.2f);
        GeneratePlatforms();

        yield return new WaitForSeconds(0.5f);
        GenerateNavMesh();

        yield return new WaitForSeconds(0.2f);
        GeneratePlayerSpawn();

        yield return new WaitForSeconds(0.2f);
        MovePlayerToSpawn();

        yield return new WaitForSeconds(0.2f);
        GenerateChests();

        yield return new WaitForSeconds(0.2f);
        GenerateProps();

        yield return new WaitForSeconds(0.2f);
        GenerateEnemy();

        yield return new WaitForSeconds(0.2f);
        GenerateNextFloorPortal();

		yield return new WaitForSeconds(0.25f);
		fade.GetComponent<Fade>().FadeIn();
    }
	
	public int GetFloor() {
		return floor;
	}

}
