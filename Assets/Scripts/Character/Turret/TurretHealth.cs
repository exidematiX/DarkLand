using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHealth : Health
{
    public static Action<Turret> OnTurretKilled;
    public static Action<Turret> OnTurretHit;
    private Turret _turret;
    protected override void Start()
    {
        base.Start();
        _turret = GetComponent<Turret>();
    }

    protected override void Update()
    {
        base.Update();
    }


    protected override void CreateHealthBar()
    {
        base.CreateHealthBar();
    }


    public override void DealDamage(float damageReceived)
    {
        CurrentHealth -= damageReceived;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }
        else
        {
            OnTurretHit?.Invoke(_turret);
        }
    }


    protected override void Die()
    {
        OnTurretKilled?.Invoke(_turret);
        // 使炮塔消失或执行消失动画
        FieldOfView.Instance.originsDic.Remove(gameObject);
        Destroy(gameObject);
    }
}
