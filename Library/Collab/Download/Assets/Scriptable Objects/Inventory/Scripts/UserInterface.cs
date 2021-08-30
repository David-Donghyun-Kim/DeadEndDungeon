using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public abstract class UserInterface : MonoBehaviour
{
    public InventoryObject inventory;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
    void Awake()
    {
        for (int i = 0; i < inventory.getSlots.Length; i++)
        {
            inventory.getSlots[i].parent = this;
            inventory.getSlots[i].OnAfterUpdate += OnSlotUpdate;
        }

        CreateSlots();

        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    private void OnSlotUpdate(InventorySlot _slot)
    {
        if (_slot.item.ID >= 0) {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.GetItemObject.uiDisplay;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = _slot.amount == 1 ? "" : _slot.amount.ToString("n0");
            _slot.slotDisplay.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = _slot.GetItemObject.description;
        } else {
            if (_slot.slotDisplay != null) {
                _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
                _slot.slotDisplay.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    public abstract void CreateSlots();

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
        Debug.Log(obj.name);
        obj.transform.GetChild(2).gameObject.SetActive(true);
    }
    public void OnExit(GameObject obj)
    {
        obj.transform.GetChild(2).gameObject.SetActive(false);
        MouseData.slotHoveredOver = null;
    }
    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }
    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }
    public void OnDragStart(GameObject obj)
    {
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }
    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        
        if(slotsOnInterface[obj].item.ID >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].GetItemObject.uiDisplay;
            img.raycastTarget = false;
        }
        return tempItem;
    }

    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);

        if (MouseData.interfaceMouseIsOver == null)
        {
            slotsOnInterface[obj].RemoveItem();
            return;
        }

        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
        }
    }

    public void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
    }
}