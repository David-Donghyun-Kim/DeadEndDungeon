using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/****************************************************************************************************
 * BuildingGridController handles the interaction between a selected building and the in-world grid
 * that it can be placed on.
 * 
 * The building must first be selected via the BuildingController script, which passes control of the
 * object to this script if the player is moving the building. If creating a new building, the player
 * only needs to select the desired prefab from a menu and can then place it in designated locations
 * on the grid. The grid must be square.
 * 
 * DEBUG CONTROLS:
 * Q to switch between Move and Create mode
 */
public class BuildingGridController : MonoBehaviour
{
    public GameObject[,] gridObjects; // grid with references to the actual objects on the grid
    private Vector3[,] gridPositions; // placement grid, holds worldspace centers of each plot
    public int gridRows = 5; // number of rows in grid
    private float gridPlotSize; // size of a single plot
    public float size; // Scales the size of the buildings
    private int closestX, closestY;

    // building prefabs
    public GameObject tavernPrefab;
    public GameObject smithyPrefab;
    public GameObject armoryPrefab;
    public GameObject itemShopPrefab;
    public GameObject bankPrefab;

    // building costs
    public int[] buildingCosts; // tavern, smithy, armory, itemShop, bank

    // bank modifier info
    public int currentGold;
    private int startingMaxGold = 1000;
    private int newBankGoldIncrease = 500;
    private int upgradeBankGoldIncrease = 100;

    // player stuff
    public InventoryObject inventory;
    private int goldInventoryIndex = 5;

    // UI
    public GameObject managementUI;
    private TownMenuController managementUIScript;

    public GameObject selectedBuilding; // current selected building
    public Vector3 originalPos; // selected building's original pos
    private RaycastHit hit; // raycast from player's mouse to screen

    public enum Mode { Move, Create }; // defines whether player is moving or creating buildings
    public Mode mode; // current mode

    public TextMeshProUGUI actionText; // text that displays when the user performs an action

    // player pref names
    public string BuildingsPref = "Buildings";
    public string BuildingLevelsPref = "BuildingLevels";
    public string GoldPref = "Gold";

    // Start is called before the first frame update
    void Start()
    {
        mode = Mode.Move;
        HideActionText();
        buildingCosts = new int[] { 100, 200, 300, 400, 500 };

        // setup for initializing grids
        gridPositions = new Vector3[gridRows, gridRows];
        Bounds gridBounds = GetComponent<Collider>().bounds;
        gridPlotSize = gridBounds.size.x / gridRows;

        // initialize grid of available positions on the grid
        for (int x = 0; x < gridPositions.GetLength(0); x++)
        {
            for (int y = 0; y < gridPositions.GetLength(1); y++)
            {
                if (x != 0)
                {
                    gridPositions[x, y].x = gridPositions[x - 1, y].x + gridPlotSize;
                }
                else
                {
                    gridPositions[x, y].x = gridBounds.min.x + gridPlotSize / 2;
                }
                if (y != 0)
                {
                    gridPositions[x, y].z = gridPositions[x, y - 1].z + gridPlotSize;
                }
                else
                {
                    gridPositions[x, y].z = gridBounds.min.z + gridPlotSize / 2;
                }
            }
        }

        // initialize grid of object references as all null
        gridObjects = new GameObject[gridRows, gridRows];
        for (int x = 0; x < gridObjects.GetLength(0); x++)
        {
            for (int y = 0; y < gridObjects.GetLength(1); y++)
            {
                gridObjects[x, y] = null;
            }
        }

        // update initial gold
        managementUIScript = managementUI.GetComponent<TownMenuController>();
        currentGold = PlayerPrefs.GetInt(GoldPref);
        UpdateGold(currentGold);

        // get gold from inventory, update current gold, and then clear inventory
        List<InventorySlot> goldInInventory = inventory.FindAllItemsonInventory(goldInventoryIndex);
        for (int i = 0; i < goldInInventory.Count; i++)
        {
            UpdateGold(currentGold + goldInInventory[i].amount);
        }
        // inventory.Clear();

        // update 

        // finally, load all buildings from memory
        loadBuildings();

        Debug.Log("Finished setting up town");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // deselect building
        {
            DeselectBuilding();
        }
        if (selectedBuilding != null)
        {
            MoveBuildingToMouse();
            PlaceBuilding();
        }
    }

    // Create a new building by instantiating the desired prefab
    public void CreateBuilding(GameObject prefab, Building type)
    {
        if (buildingCosts[(int)type] <= currentGold) // player has enough to purchase
        {
            mode = Mode.Create;
            DisplayActionText("Place your building on the grid.");
            DeselectBuilding();
            selectedBuilding = Instantiate(prefab);
            selectedBuilding.GetComponent<BuildingController>().type = type;
            selectedBuilding.transform.localScale = new Vector3(size, size, size);
            selectedBuilding.GetComponent<BoxCollider>().size = new Vector3(size, size, size);
        }
        else // player does not have enough to purchase, inform them
        {
            DisplayActionText("You need " + buildingCosts[(int)type] + " gold to buy this!");
        }
    }

    // Transform and snap to grid selected building if the mouse "collides" with the grid
    private void MoveBuildingToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && hit.collider.name == "Grid")
        {
            float min = Vector3.Distance(hit.point, gridPositions[closestX, closestY]);
            for (int x = 0; x < gridPositions.GetLength(0); x++)
            {
                for (int y = 0; y < gridPositions.GetLength(1); y++)
                {
                    if (gridObjects[x, y] == null)
                    { // if null, then this plot is free
                        float distance = Vector3.Distance(hit.point, gridPositions[x, y]);
                        if (distance < min && distance < gridPlotSize)
                        {
                            closestX = x;
                            closestY = y;
                        }
                    }
                }
            }
            selectedBuilding.transform.position = gridPositions[closestX, closestY];
        }
    }

    // Place selected building at designated location that object was last moved to via mouse
    private void PlaceBuilding()
    {
        if (Input.GetMouseButtonDown(1) && gridObjects[closestX, closestY] == null) // place onto grid when user clicks
        {
            DisplayActionText("Building placed!");

            // modify building traits
            BuildingController buildingScript = selectedBuilding.GetComponent<BuildingController>();
            buildingScript.gridX = closestX;
            buildingScript.gridY = closestY;
            selectedBuilding.GetComponent<BuildingController>().DeactivateGlow(); // disable glowing cosmetic effect from click
            selectedBuilding.GetComponent<BoxCollider>().enabled = true; // enable box collider to allow player to interact

            // modify game state
            SaveBuildingPref(closestX, closestY, buildingScript.type, buildingScript.level); // save into persistent game state
            gridObjects[closestX, closestY] = selectedBuilding; // track this building on the grid

            // update adjacentBuilding[] arrays of this and old neighbors
            buildingScript.UpdateAdjacentBuildings();
            UpdateAdjacentBuildings(closestX, closestY);

            // update character bonuses from taverns
            UpdateCharacterBonuses();

            // deduct funds from player if new building
            if (mode == Mode.Create) UpdateGold(currentGold - buildingCosts[(int)buildingScript.type]);

            selectedBuilding = null; // "deselect" building
            mode = Mode.Move;
        }
    }

    // If there is a building selected, destroy it to deselect
    private void DeselectBuilding()
    {
        if (selectedBuilding != null)
        {
            if (mode == Mode.Move) // player is moving preexisting building, simply reset
            {
                selectedBuilding.GetComponent<BuildingController>().DeactivateGlow();
                selectedBuilding.transform.position = originalPos;
                ClearBuildingPref(closestX, closestY);
                selectedBuilding = null;
            }
            else // player is creating new building, delete it
            {
                Destroy(selectedBuilding);
                mode = Mode.Move;
            }
        }
    }

    // Given the position of a building, update the adjacentBuildings[] array of surrounding buildings
    public void UpdateAdjacentBuildings(int x, int y)
    {
        if (x > 0 && gridObjects[x - 1, y] != null) // left
        {
            gridObjects[x - 1, y].GetComponent<BuildingController>().UpdateAdjacentBuildings();
        }
        if (y > 0 && gridObjects[x, y - 1] != null) // bottom
        {
            gridObjects[x, y - 1].GetComponent<BuildingController>().UpdateAdjacentBuildings();
        }
        if (x < gridRows - 1 && gridObjects[x + 1, y] != null) //  right
        {
            gridObjects[x + 1, y].GetComponent<BuildingController>().UpdateAdjacentBuildings();
        }
        if (y < gridRows - 1 && gridObjects[x, y + 1] != null) // top
        {
            gridObjects[x, y + 1].GetComponent<BuildingController>().UpdateAdjacentBuildings();
        }
    }

    /************************* Building mechanics *************************/

    // Update gold to parameter provided
    public void UpdateGold(int gold)
    {
        int max = CalculateMaxGold();
        currentGold = gold;
        if (currentGold > max) currentGold = max;
        managementUIScript.AdjustGoldFunds(currentGold, max);
        PlayerPrefs.SetInt(GoldPref, currentGold);
    }

    // Calculate max gold based on number of banks
    private int CalculateMaxGold()
    {
        int gold = startingMaxGold;

        for (int x = 0; x < gridObjects.GetLength(0); x++)
        {
            for (int y = 0; y < gridObjects.GetLength(1); y++)
            {
                if (gridObjects[x, y] != null)
                {
                    BuildingController buildingScript = gridObjects[x, y].GetComponent<BuildingController>();
                    if (buildingScript.type == Building.Bank) gold += buildingScript.value;
                }

            }
        }

        return gold;
    }

    // Look through current taverns on grid and update character bonuses
    public void UpdateCharacterBonuses()
    {
        int[] attributes = new int[] { 0, 0, 0, 0, }; // dex, int, str, sta
        const int dexterity = 0, intellect = 1, strength = 2, stamina = 3; // these are indices for array, NOT values
        const int bonusModifier = 2;

        for (int x = 0; x < gridObjects.GetLength(0); x++)
        {
            for (int y = 0; y < gridObjects.GetLength(1); y++)
            {
                BuildingController buildingScript;
                if (gridObjects[x, y] != null && (buildingScript = gridObjects[x, y].GetComponent<BuildingController>()).type == Building.Tavern)
                {
                    for (int i = 0; i < attributes.Length; i++) attributes[i] += bonusModifier; // flat bonus per tavern
                    for (int i = 0; i < buildingScript.adjacentBuildings.Length; i++) // bonuses for adjacent buildings depending on type
                    {
                        switch (buildingScript.adjacentBuildings[i])
                        {
                            case (int)Building.Smithy: attributes[strength] += bonusModifier; break;
                            case (int)Building.Armory: attributes[stamina] += bonusModifier; break;
                            case (int)Building.ItemShop: attributes[intellect] += bonusModifier; break;
                            case (int)Building.Bank: attributes[dexterity] += bonusModifier; break;
                            default: break;
                        }
                    }
                }
            }
        }
    }

    /************************* Inventory management *************************/

    private int ConvertInventoryGold()
    {

        return 0;
    }

    /************************* Action text *************************/

    // Display the action text temporarily
    public void DisplayActionText(string text)
    {
        StopCoroutine("FadeActionText");
        actionText.text = text;
        actionText.alpha = 1f;
        StartCoroutine("FadeActionText");
    }

    // Fade out the action text
    IEnumerator FadeActionText()
    {
        for (float a = 1f; a > 0; a -= 0.0025f)
        {
            actionText.alpha = a;
            yield return null;
        }
    }

    // Hide the action text
    private void HideActionText()
    {
        StopCoroutine("FadeActionText");
        actionText.alpha = 0;
    }

    /************************* Saving and loading *************************/

    public void SaveBuildingPref(int x, int y, Building type, int level)
    {
        PlayerPrefs.SetInt(BuildingsPref + x + "" + y, (int)type);
        PlayerPrefs.SetInt(BuildingLevelsPref + x + "" + y, level);
        PlayerPrefs.Save();
        // Debug.Log("Saved lvl. " + level + " building at " + x + ", " + y);
    }

    public void ClearBuildingPref(int x, int y)
    {
        PlayerPrefs.SetInt(BuildingsPref + x + "" + y, -1);
        PlayerPrefs.SetInt(BuildingLevelsPref + x + "" + y, -1);
        PlayerPrefs.Save();
        // Debug.Log("Cleared building at " + x + ", " + y);
    }

    private GameObject indexToBuilding(Building type)
    {
        GameObject building = null;
        switch (type)
        {
            case Building.Tavern: building = tavernPrefab; break;
            case Building.Smithy: building = smithyPrefab; break;
            case Building.Armory: building = armoryPrefab; break;
            case Building.ItemShop: building = itemShopPrefab; break;
            case Building.Bank: building = bankPrefab; break;
        }
        return building;
    }

    public void loadBuildings()
    {
        int i = 0;
        while (i < gridRows)
        {
            int j = 0;
            while (j < gridRows)
            {
                int type = PlayerPrefs.GetInt(BuildingsPref + i + "" + j);
                if (type >= (int)Building.Tavern && type <= (int)Building.Bank)
                {
                    selectedBuilding = Instantiate(indexToBuilding((Building)type));

                    BuildingController buildingScript = selectedBuilding.GetComponent<BuildingController>();
                    buildingScript.gridX = i;
                    buildingScript.gridY = j;
                    buildingScript.level = PlayerPrefs.GetInt(BuildingLevelsPref + i + "" + j);
                    buildingScript.type = (Building)type;
                    gridObjects[i, j] = selectedBuilding;

                    selectedBuilding.GetComponent<BoxCollider>().enabled = true;
                    //selectedBuilding.GetComponent<BoxCollider>().size = new Vector3(size,size,size);
                    selectedBuilding.transform.position = gridPositions[i, j];
                    selectedBuilding.transform.localScale = new Vector3(size, size, size);

                    selectedBuilding = null;
                }
                j++;
            }
            i++;
        }
        UpdateGold(currentGold);
    }

    /************************* Debug tools *************************/

    public void ClearMemory()
    {
        int i = 0;
        while (i < gridRows)
        {
            int j = 0;
            while (j < gridRows)
            {
                ClearBuildingPref(i, j);

                j++;
            }
            i++;
        }
        currentGold = 1000;
        PlayerPrefs.SetInt(GoldPref, currentGold);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GainGold()
    {
        UpdateGold(CalculateMaxGold());
    }
}
