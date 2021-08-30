using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField]
    private Transform _selection;
    private float distance = 13;
    void Update()
    {
        if (_selection != null) {
            _selection.GetComponent<Outline>().enabled = false;
            _selection = null;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << LayerMask.NameToLayer("Loot");
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            var selection = hit.collider.transform.gameObject;
            if (selection.GetComponent<MeshRenderer>() != null)
                selection.GetComponent<Outline>().enabled = true;
            _selection = selection.transform;
        }
    }
}
