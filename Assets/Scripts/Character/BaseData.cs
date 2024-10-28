using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseData : UnitData
{
    protected override void Start()
    {
        base.Start();
        chooseUIPrefab = Resources.Load<GameObject>("Prefabs/UIPrefabs/BaseChooseObj");
    }
}
