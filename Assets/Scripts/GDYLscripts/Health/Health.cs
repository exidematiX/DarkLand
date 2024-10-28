using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class Health : MonoBehaviour
{
    [SerializeField] protected GameObject healthBarPrefab;
    [SerializeField] protected Transform barPosition;


    [SerializeField] protected float initialHealth = 10f;
    [SerializeField] protected float maxHealth = 10f;

    public float CurrentHealth { get; set; }


    private Image _healthBar;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        CreateHealthBar();
        CurrentHealth = initialHealth;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            DealDamage(5f);
            Debug.Log("扣血");
        }
        _healthBar.fillAmount = Mathf.Lerp(_healthBar.fillAmount,
            CurrentHealth / maxHealth, Time.deltaTime * 10f);
    }

    protected virtual void CreateHealthBar()
    {
        GameObject newBar = Instantiate(healthBarPrefab, barPosition.position, Quaternion.identity);
        newBar.transform.SetParent(transform);

        HealthContainer container = newBar.GetComponent<HealthContainer>();
        container.GetComponent<Canvas>().sortingLayerName = "BuildingHide";
        _healthBar = container.FillAmountImage;
    }

    public virtual void DealDamage(float damageReceived)
    {
        CurrentHealth -= damageReceived;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }
        else
        {
            
        }
    }
    public void ResetHealth()
    {
        CurrentHealth = initialHealth;
        _healthBar.fillAmount = 1f;
    }

    protected virtual void Die()
    {
        
    }
}
