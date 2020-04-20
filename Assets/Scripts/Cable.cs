using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Cable
{
    public GameObject CableObject, OtherPoweredObject;

    public Cable(GameObject cable, GameObject otherPowered)
    {
        this.CableObject = cable;
        this.OtherPoweredObject = otherPowered;
    }
}
