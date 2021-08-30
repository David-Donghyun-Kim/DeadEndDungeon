using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ModifiableInt
{
    [SerializeField]
    private int baseValue;
    public int BaseValue { get { return baseValue; } set { baseValue = value; UpdateModifiedValue(); } }

    [SerializeField]
    private int modifiedValue; // actual player attribute value (base + buffs)
    public int ModifiedValue { get { return modifiedValue; } private set { modifiedValue = value; } }

    // list of all buffs affecting this modifiable int (particular stat)
    public List<IModifier> modifiers = new List<IModifier>();

    public delegate void ValueModified();

    public ValueModified OnValueModified;
    
    public ModifiableInt(ValueModified method = null)
    {
        modifiedValue = BaseValue;
        if (method != null)
            OnValueModified += method;
    }

    public void RegisterModEvent(ValueModified method)
    {
        OnValueModified += method;
    }

     public void UnRegisterModEvent(ValueModified method)
    {
        OnValueModified -= method;
    }

     public void UpdateModifiedValue()
    {
        var valueToAdd = 0;
        for (int i = 0; i < modifiers.Count; i++)
        {
            // summing every value of this particular attribute into valueToAdd
            modifiers[i].AddValue(ref valueToAdd);
        }

        ModifiedValue = baseValue + valueToAdd;

        if (OnValueModified != null)
            OnValueModified.Invoke();
    }

    public void AddModifier(IModifier _modifier)
    {
        // ex. adding another piece of gear to character requires adding additional attribute buffs
        modifiers.Add(_modifier);
        UpdateModifiedValue();
    }

     public void RemoveModifier(IModifier _modifier)
    {
        modifiers.Remove(_modifier);
        UpdateModifiedValue();
    }
}