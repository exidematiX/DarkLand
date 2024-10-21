using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyFx : MonoBehaviour
{
    [SerializeField] private Transform textDamageSpawnPosition;


    private Enemy _enemy;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        if (_enemy == null)
        {
            Debug.LogError("Enemy component not found on this GameObject!", gameObject);
        }
    }

    private void EnemyHit(Enemy enemy,float damage)
    {
        if(enemy == null)
        {
            Debug.LogError("Enemy is null.");
            return;
        }

        if(_enemy == enemy)
        {
            GameObject newInstance = DamageTextManager.Instance.Pooler.GetInstanceFromPool();
            //TextMeshProUGUI damageText = newInstance.GetComponent<DamageText>().DmgText;
            //damageText.text = damage.ToString(); // 示例：如果你想限制到两位小数
            //Debug.Log(damageText.text);



            if (newInstance == null)
            {
                Debug.LogError("Failed to get an instance from the pool.");
                return;
            }

            DamageText damageTextComponent = newInstance.GetComponent<DamageText>();
            if (damageTextComponent == null)
            {
                Debug.LogError("DamageText component missing on the pooled instance.");
                return;
            }

            TextMeshProUGUI damageText = damageTextComponent.DmgText;
            if (damageText == null)
            {
                Debug.LogError("TextMeshProUGUI component not found.");
                return;
            }
            newInstance.transform.SetParent(textDamageSpawnPosition);
            newInstance.transform.position = textDamageSpawnPosition.position;
            newInstance.SetActive(true);
        }
    }

    private void OnEnable()
    {
        Projectile.OnEnemyHit += EnemyHit;
    }

    private void OnDisable()
    {
        Projectile.OnEnemyHit -= EnemyHit;
    }

}