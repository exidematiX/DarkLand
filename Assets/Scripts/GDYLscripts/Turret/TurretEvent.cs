using System.Collections;
using UnityEngine;

public class TurretEvent : MonoBehaviour
{
    private Turret _turret;


   

    private IEnumerator PlayHurt()
    {
        // 这里是受伤动画或者效果逻辑
        Debug.Log("Turret hit");
        yield return new WaitForSeconds(0f);
    }

    private IEnumerator PlayDead()
    {
        // 这里是死亡动画或者效果逻辑
        Debug.Log("Turret dead");
        yield return new WaitForSeconds(0.2f);
    }

    private void Start()
    {
        _turret = GetComponent<Turret>();
    }

    private void TurretHit(Turret turret)
    {
        if (_turret == turret)
        {
            StartCoroutine(PlayHurt());
        }
    }

    private void TurretDead(Turret turret)
    {
        if (_turret == turret)
        {
            StartCoroutine(PlayDead());
        }
    
    }

    private void OnEnable()
    {
        TurretHealth.OnTurretHit += TurretHit;
        TurretHealth.OnTurretKilled += TurretDead;
    }

    private void OnDisable()
    {
        TurretHealth.OnTurretHit -= TurretHit;
        TurretHealth.OnTurretKilled -= TurretDead;
    }
}
