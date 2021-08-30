using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TownMenuController : MonoBehaviour
{
    public GameObject shopCanvas; // shop canvas
    private bool shopCanvasOpen = false;
    private GameObject shop;
    private ShopManager shopScript;

    public GameObject buildPanel; // menu for creating specific buildings
    private bool buildPanelOpen = false;

    public GameObject editPanel; // menu for editing buildings

    public GameObject goldFunds; // slider for showing gold funds
    public GameObject goldText; // text for showing gold funds

    private GameObject grid; // grid that objects can be placed on
    private BuildingGridController gridScript; // grid's script

    // Start is called before the first frame update
    void Start()
    {
        if ((grid = GameObject.Find("/Grid")) == null)
        {
            Debug.Log("Error: TownMenuController could not find building grid.");
        }
        gridScript = grid.GetComponent<BuildingGridController>();
        if ((shop = GameObject.Find("/ShopManager")) == null)
        {
            Debug.Log("Error: TownMenuController could not find shop.");
        }
        shopScript = shop.GetComponent<ShopManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // DeselectEditPanel();
    }

    // Hide edit panel if the player clicks outside of it
    private void DeselectEditPanel()
    {
        if (Input.GetMouseButton(0) && editPanel.activeSelf &&
            !RectTransformUtility.RectangleContainsScreenPoint(
                editPanel.GetComponent<RectTransform>(),
                Input.mousePosition,
                Camera.main))
        {
            editPanel.SetActive(false);
        }
    }

    // Modifies UI gold slider to reflect player's funds
    public void AdjustGoldFunds(int currentGold, int maxGold)
    {
        // Debug.Log("Current gold: " + currentGold + ", max gold: " + maxGold);
        goldFunds.GetComponent<Image>().fillAmount = (float)currentGold / (float)maxGold;
        goldText.GetComponent<TextMeshProUGUI>().text = currentGold + " / " + maxGold;
    }

    // Toggle shop on and off
    public void ToggleShop()
    {
        shopCanvas.SetActive(!shopCanvasOpen);
        shopCanvasOpen = !shopCanvasOpen;
    }

    // Toggle build panel on and off
    public void ToggleBuildPanel()
    {
        buildPanel.SetActive(!buildPanelOpen);
        buildPanelOpen = !buildPanelOpen;
    }

    public void BuildTavern()
    {
        gridScript.CreateBuilding(gridScript.tavernPrefab, Building.Tavern);
    }

    public void BuildSmithy()
    {
        gridScript.CreateBuilding(gridScript.smithyPrefab, Building.Smithy);
    }

    public void BuildArmory()
    {
        gridScript.CreateBuilding(gridScript.armoryPrefab, Building.Armory);
    }

    public void BuildItemShop()
    {
        gridScript.CreateBuilding(gridScript.itemShopPrefab, Building.ItemShop);
    }

    public void BuildBank()
    {
        gridScript.CreateBuilding(gridScript.bankPrefab, Building.Bank);
    }

    public void PurchaseWeapon()
    {
        shopScript.PurchaseWeapon();
    }

    public void PurchaseItem()
    {
        shopScript.PurchaseItem();
    }
}
