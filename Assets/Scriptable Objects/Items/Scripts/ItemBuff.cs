using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemBuff : IModifier
{
    public BuffType attribute;
    public int value;
    public int min;
    public int max;

    private string[] attributeNames;

    public ItemBuff(int _min, int _max)
    {
        min = _min;
        max = _max;
        // CalculateBonuses();
        GenerateValue();
    }

    public void AddValue(ref int baseValue)
    {
        baseValue += value;
    }

    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }

    public void CalculateBonuses()
    {
        // Debug.Log("Attribute is: " + attribute.ToString());
        min += PlayerPrefs.GetInt(attribute.ToString());
        max += PlayerPrefs.GetInt(attribute.ToString());
    }
}
