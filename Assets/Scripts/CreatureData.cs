using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureData : UnitData
{
    public float atkValue = 30f;
    public float atkRange = 2f;

    protected override void Start()
    {
        base.Start();
        chooseUIPrefab = Resources.Load<GameObject>("Prefabs/UIPrefabs/FarmerChooseObj");
    }    
}
