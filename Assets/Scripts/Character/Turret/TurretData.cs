using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretData : UnitData
{

    protected override void Start()
    {
        base.Start();
        chooseUIPrefab = Resources.Load<GameObject>("Prefabs/UIPrefabs/TurretChooseObj");
    }

}
