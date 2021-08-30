using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Building { Tavern, Smithy, Armory, ItemShop, Bank };

/****************************************************************************************************
 * 
 */
public class BuildingController : MonoBehaviour
{
    #region - Variables -

    private Camera cam;

    private GameObject grid; // grid that objects can be placed on
    private BuildingGridController gridScript; // grid's script
    public int[] adjacentBuildings; // adjacent buildings to this building

    public string name;
    public Building type; // type of building
    public int level; // upgrade level of building
    public int value = 0; // value if this building
    private int currentUpgradeCost; // current upgrade cost of this
    public int gridX, gridY; // position on grid

    public GameObject menu; // menu
    private GameObject panel; // menu panel

    private Material material;

    #endregion

    #region - Start -

    // Start is called before the first frame update
    void Start()
    {
        InitializeInfo();
    }

    #endregion

    // Initialize building info - has to be called again in BuildingGridController.loadBuildings() to properly update
    public void InitializeInfo()
    {
        if ((cam = GameObject.Find("/Camera").GetComponent<Camera>()) == null)
        {
            Debug.Log("Error: BuildingController could not find camera.");
        }
        if ((grid = GameObject.Find("/Grid")) == null)
        {
            Debug.Log("Error: BuildingController could not find building grid.");
        }
        gridScript = grid.GetComponent<BuildingGridController>();
        if ((menu = GameObject.Find("/BuildingMenu")) == null)
        {
            Debug.Log("Error: BuildingController could not find building menu.");
        }
        material = GetComponent<MeshRenderer>().material;
        level = PlayerPrefs.GetInt(gridScript.BuildingLevelsPref + gridX + "" + gridY);
        if (level < 1) level = 1;
        if (PlayerPrefs.GetInt(gridScript.BuildingsPref + gridX + "" + gridY) >= 0)
        {
            type = (Building)PlayerPrefs.GetInt(gridScript.BuildingsPref + gridX + "" + gridY);
        }
        SetName();
        UpdateGoldValue();
        UpdateAdjacentBuildings();
    }

    // Detect when the player clicks on this building
    private void OnMouseDown()
    {
        if (gridScript.mode == BuildingGridController.Mode.Move)
        {
            DisplayMenu();
        }
    }

    private void SetName()
    {
        switch (type)
        {
            case Building.Tavern: name = "Tavern"; break;
            case Building.Smithy: name = "Smithy"; break;
            case Building.Armory: name = "Armory"; break;
            case Building.ItemShop: name = "Item Shop"; break;
            case Building.Bank: name = "Bank"; break;
        }
    }

    // Find adjacent buildings to this building and record their types.
    public void UpdateAdjacentBuildings()
    {
        adjacentBuildings = new int[4];
        for (int i = 0; i < adjacentBuildings.Length; i++) adjacentBuildings[i] = -1; // -1 represents an empty slot
        // Debug.Log(name + " at X: " + gridX + ", Y:" + gridY);
        if (gridX > 0 && gridScript.gridObjects[gridX - 1, gridY] != null) // left
        {
            adjacentBuildings[0] = (int)gridScript.gridObjects[gridX - 1, gridY].GetComponent<BuildingController>().type;
        }
        if (gridY > 0 && gridScript.gridObjects[gridX, gridY - 1] != null) // bottom
        {
            adjacentBuildings[1] = (int)gridScript.gridObjects[gridX, gridY - 1].GetComponent<BuildingController>().type;
        }
        if (gridX < gridScript.gridRows - 1 && gridScript.gridObjects[gridX + 1, gridY] != null) //  right
        {
            adjacentBuildings[2] = (int)gridScript.gridObjects[gridX + 1, gridY].GetComponent<BuildingController>().type;
        }
        if (gridY < gridScript.gridRows - 1 && gridScript.gridObjects[gridX, gridY + 1] != null) // top
        {
            adjacentBuildings[3] = (int)gridScript.gridObjects[gridX, gridY + 1].GetComponent<BuildingController>().type;
        }
    }

    // Update building's value
    private void UpdateGoldValue()
    {
        currentUpgradeCost = gridScript.buildingCosts[(int)type] * level;
        value += currentUpgradeCost;
        if (value > gridScript.buildingCosts[(int)type]) value /= 2;
    }

    // Displays a menu for the player to choose whether to move, upgrade, or sell this building
    private void DisplayMenu()
    {
        gridScript.DisplayActionText("Selected lvl. " + level + " " + name);
        panel = menu.transform.GetChild(0).gameObject;
        panel.SetActive(true);
        // panel.transform.position = cam.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, panel.transform.position.z));

        // visual glowing emission cue of selected building
        ActivateGlow();

        // add listeners to buttons
        Button upgrade = panel.transform.GetChild(0).gameObject.GetComponent<Button>(),
            move = panel.transform.GetChild(1).gameObject.GetComponent<Button>(),
            sell = panel.transform.GetChild(2).gameObject.GetComponent<Button>();
        upgrade.onClick.RemoveAllListeners();
        move.onClick.RemoveAllListeners();
        sell.onClick.RemoveAllListeners();
        upgrade.onClick.AddListener(UpgradeBuilding);
        move.onClick.AddListener(MoveBuilding);
        sell.onClick.AddListener(SellBuilding);

        // DEBUG
        Debug.Log("I'm a " + type + " at " + gridX + ", " + gridY + "." + "The buildings next to me (left, bottom, right, top) are: "
            + (Building)adjacentBuildings[0] + ", "
            + (Building)adjacentBuildings[1] + ", "
            + (Building)adjacentBuildings[2] + ", "
            + (Building)adjacentBuildings[3]);
    }

    #region - Cosmetic Effects -

    public void ActivateGlow()
    {
        DeactivateGlow();
        StopCoroutine("GlowFade");
        material.EnableKeyword("_EMISSION");
        StartCoroutine("GlowFade");
    }

    public void DeactivateGlow()
    {
        material.DisableKeyword("_EMISSION");
    }

    IEnumerator GlowFade()
    {
        for (float a = 1f; a > 0; a -= 0.005f)
        {
            material.SetColor("_EmissionColor", Color.white * a);
            yield return null;
        }
        DeactivateGlow();
    }

    #endregion

    #region - Buttons -

    public void MoveBuilding()
    {
        gridScript.DisplayActionText("Place your building on the grid.");

        Vector3 pos = transform.position;
        gridScript.originalPos = pos; // save original position of this building
        GetComponent<BoxCollider>().enabled = false; // temporarily disable box collider for smoother mouse movement

        // clear states
        gridScript.gridObjects[gridX, gridY] = null;
        gridScript.UpdateAdjacentBuildings(gridX, gridY);
        gridScript.ClearBuildingPref(gridX, gridY);

        gridScript.selectedBuilding = gameObject; // pass control to grid
        panel.SetActive(false);
    }

    public void UpgradeBuilding()
    {
        if (value <= gridScript.currentGold)
        {
            level++;
            ActivateGlow();
            gridScript.SaveBuildingPref(gridX, gridY, type, level);
            gridScript.UpdateGold(gridScript.currentGold - value);
            gridScript.DisplayActionText("You spent " + value + " gold to upgrade " + name + " to lvl. " + level);
            UpdateGoldValue();
            gridScript.UpdateGold(gridScript.currentGold); // update again to get correct max value if bank
        }
        else
        {
            gridScript.DisplayActionText("You need " + value + " gold to upgrade this!");
        }

    }

    public void SellBuilding()
    {
        gridScript.DisplayActionText("Sold lvl. " + level + " " + name + " for " + value / 2 + " gold");
        gridScript.gridObjects[gridX, gridY] = null; // free up this space on the grid
        gridScript.UpdateGold(gridScript.currentGold + value / 2); // update gold
        gridScript.UpdateAdjacentBuildings(gridX, gridY); // update buildings adjacent to this building
        gridScript.UpdateCharacterBonuses(); // update character bonuses after removing this building from grid
        gridScript.ClearBuildingPref(gridX, gridY); // free up this space on grid but in saved settings
        Destroy(this.gameObject); // destroy
        panel.SetActive(false);
    }

    public void CloseMenu()
    {
        DeactivateGlow();
        panel.SetActive(false);
    }

    #endregion
}
