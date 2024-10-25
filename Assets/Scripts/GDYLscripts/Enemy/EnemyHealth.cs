using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

//public class EnemyHealth : MonoBehaviour
//{
//    public static Action<EnemyAI> OnEnemyKilled;
//    public static Action<EnemyAI> OnEnemyHit;


//    [SerializeField] private GameObject healthBarPrefab;
//    [SerializeField] private Transform barPosition;


//    [SerializeField] private float initialHealth = 10f;
//    [SerializeField] private float maxHealth = 10f;


//    public float CurrentHealth { get; set; }


//    private Image _healthBar;
//    private EnemyAI _enemy;

//    // Start is called before the first frame update
//    void Start()
//    {
//        CreateHealthBar();
//        CurrentHealth = initialHealth;

//        _enemy = GetComponent<EnemyAI>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.P))
//        {
//            DealDamage(5f);
//            Debug.Log("扣血");
//        }
//        _healthBar.fillAmount = Mathf.Lerp(_healthBar.fillAmount,
//            CurrentHealth / maxHealth, Time.deltaTime * 10f);
//    }


//    private void CreateHealthBar()
//    {
//        GameObject newBar = Instantiate(healthBarPrefab, barPosition.position, Quaternion.identity);
//        newBar.transform.SetParent(transform);

//        HealthContainer container = newBar.GetComponent<HealthContainer>();
//        _healthBar = container.FillAmountImage;
//    }

//    public void DealDamage(float damageReceived)
//    {
//        CurrentHealth -= damageReceived;
//        if (CurrentHealth <= 0)
//        {
//            CurrentHealth = 0;
//            Die();
//        }
//        else
//        {
//            OnEnemyHit?.Invoke(_enemy);
//        }
//    }

//    public void ResetHealth()
//    {
//        CurrentHealth = initialHealth;
//        _healthBar.fillAmount = 1f;
//    }


//    private void Die()
//    {
//        OnEnemyKilled?.Invoke(_enemy);
//    }
//}
public class EnemyHealth : Health
{
    public static Action<EnemyAI> OnEnemyKilled;
    public static Action<EnemyAI> OnEnemyHit;
    private EnemyAI _enemy;
    protected override void Start()
    {
        base.Start();
        _enemy = GetComponent<EnemyAI>();
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
            OnEnemyHit?.Invoke(_enemy);
        }
    }


    protected override void Die()
    {
        OnEnemyKilled?.Invoke(_enemy);
    }
}
