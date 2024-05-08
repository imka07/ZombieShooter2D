using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCostManager : ScriptableObject
{
    public WeaponList weaponList;
}

[System.Serializable]
public class WeaponList
{
    public List<Weapon> weapons;
}

[System.Serializable]
public class WeaponCost
{
    public string weaponCost;
}

