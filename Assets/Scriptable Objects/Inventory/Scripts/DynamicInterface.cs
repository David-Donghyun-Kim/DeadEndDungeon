using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DynamicInterface : UserInterface
{
    public GameObject inventoryPrefab;
    public int xStart = 400;
    public int yStart = 115;
    public int horizontalIconOffset;
    public int verticalIconOffset;
    public int numColumns;
    public override void CreateSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.getSlots.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            inventory.getSlots[i].slotDisplay = obj;

            slotsOnInterface.Add(obj, inventory.getSlots[i]);
        }
    }

    private Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + (horizontalIconOffset * (i % numColumns)), yStart + 
        (-verticalIconOffset * (i/numColumns)), 0f);
    }
}
