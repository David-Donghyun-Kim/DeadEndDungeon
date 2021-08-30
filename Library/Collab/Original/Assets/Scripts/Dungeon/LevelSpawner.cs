using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSpawner : MonoBehaviour
{
	
	public GameObject player;
	public GameObject fade;
	
	public int floor;
	
	[SerializeField] private DungeonManager[] dmThemes;
    public DungeonManager dm;
    [SerializeField] private DungeonGenerator generator;
    private GameObject tile, walledTile, playerSpawn, nextFloorPortal, enemySpawn, propLocation;
    // Start is called before the first frame update
    void Awake()
    {
		
		dm = dmThemes[0];
		UpdateDM();
		
        generator = GetComponent<DungeonGenerator>();

        GenerateBaseLevel();

        GeneratePlayerSpawn();
        MovePlayerToSpawn();
        GenerateCeiling();
        GenerateProps();
        if (dm.chest != null) { GenerateChests(); }
        GenerateEnemy();
        GenerateNextFloorPortal();
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

    private void GenerateProps()
    {
        GameObject dungeon = generator.dungeonParent.gameObject;
        for (int i = 0; i < dm.clutterLimit; i++)
        {
            do { tile = dungeon.transform.GetChild((int)UnityEngine.Random.Range(0, dungeon.transform.childCount)).gameObject; }
            while (tile.GetComponent("DummyTileComponent") as DummyTileComponent == null);
            Destroy(tile.GetComponent("DummyTileComponent") as DummyTileComponent);

            propLocation = Instantiate(dm.propPrefab[(int)UnityEngine.Random.Range(0, dm.propPrefab.Length)], tile.transform, false);
        }
    }

    private void GeneratePlayerSpawn()
    {
        GameObject dungeon = generator.dungeonParent.gameObject;
        do { walledTile = dungeon.transform.GetChild((int)UnityEngine.Random.Range(0, dungeon.transform.childCount)).gameObject; }
        while (walledTile.transform.childCount > 0 && walledTile.transform.GetChild(0).tag.CompareTo("Wall") == 0);

        playerSpawn = Instantiate(dm.playerPortal, walledTile.transform, false);
        playerSpawn.transform.localPosition = new Vector3(0, 2, 0);
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
        for (int i = 0; i < dm.enemyPopulationLimit; i++)
        {
            tile = dungeon.transform.GetChild((int)UnityEngine.Random.Range(0, dungeon.transform.childCount)).gameObject;

            enemySpawn = Instantiate(dm.enemySpawner, tile.transform, false);
            enemySpawn.transform.localPosition = new Vector3(0, 0.25f, 0);

            enemySpawn.GetComponent<EnemySpawner>().EnemyManager = GameObject.Find("/EnemyManager/").GetComponent<EnemyPool>();
            enemySpawn.GetComponent<EnemySpawner>().player = GameObject.Find("/Player/").gameObject;
            enemySpawn.GetComponent<EnemySpawner>().parent = dungeon;
        }
    }

    private void GenerateNextFloorPortal()
    {
        GameObject dungeon = generator.dungeonParent.gameObject;
        tile = dungeon.transform.GetChild((int)UnityEngine.Random.Range(0, dungeon.transform.childCount)).gameObject;
        nextFloorPortal = Instantiate(dm.nextFloorPortal, tile.transform, false);
        nextFloorPortal.transform.localPosition = new Vector3(0, 2, 0);
        
    }

    private void MovePlayerToSpawn()
    {
        GameObject player = GameObject.Find("/Player/");
		player.SetActive(false);
		player.transform.SetPositionAndRotation(playerSpawn.transform.position, playerSpawn.transform.rotation);
        player.SetActive(true);
		
		Debug.Log("Player has entered next level at position " + GameObject.Find("/Player/").transform.position);
    }

    private void GenerateChests()
    {
        GameObject dungeon = generator.dungeonParent.gameObject;
        for (int i = 0; i < dm.chestLimit; i++)
        {
            do { tile = dungeon.transform.GetChild((int)UnityEngine.Random.Range(0, dungeon.transform.childCount)).gameObject; }
            while (tile.GetComponent("DummyTileComponent") as DummyTileComponent == null);
            Destroy(tile.GetComponent("DummyTileComponent") as DummyTileComponent);

            GameObject chest = Instantiate(dm.chest, tile.transform, false);
            
            for(int j = 0; j < 4 && IsFacingWall(chest); j++)
            {
                chest.transform.localRotation.SetLookRotation(new Vector3(0, j * 90, 0));
            }

            if (IsFacingWall(chest))
            {
                Destroy(chest);
                --i;
            }
            
        }
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
        GeneratePlayerSpawn();

        yield return new WaitForSeconds(0.2f);
        GenerateCeiling();
        GenerateProps();
        if (dm.chest != null) { GenerateChests(); }

        yield return new WaitForSeconds(0.2f);
        GenerateNextFloorPortal();

        yield return new WaitForSeconds(0.2f);
        GenerateEnemy();

        yield return new WaitForSeconds(0.2f);
        MovePlayerToSpawn();
		
		yield return new WaitForSeconds(0.25f);
		fade.GetComponent<Fade>().FadeIn();
		
		

    }
	
	public int GetFloor() {
		return floor;
	}

}
