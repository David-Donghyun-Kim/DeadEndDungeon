using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public Player parent;
    public BuffType type;
    public ModifiableInt value;

    public void SetParent (Player _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttributeModified);
    }

    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}