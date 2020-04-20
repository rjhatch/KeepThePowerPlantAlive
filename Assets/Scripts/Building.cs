using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Building
{
    public string Name;
    public GameObject Preview, PreFab;
    public int WoodCost, CopperCost, CoalCost;
}
