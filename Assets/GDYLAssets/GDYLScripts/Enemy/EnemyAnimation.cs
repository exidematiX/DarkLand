using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator _animator;
    private Enemy _enemy;
    private EnemyHealth _enemyHealth;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
        _enemyHealth = GetComponent<EnemyHealth>();
    }


    private void PlayHurtAnimation()
    {
        _animator.SetTrigger("Hurt");
    }
    
    private void PlayDieAnimation()
    {
        _animator.SetTrigger("Die");

    }

    private float GetCurrentAnimationLenght()
    {
        float animationLenght = _animator.GetCurrentAnimatorStateInfo(0).length;
        return animationLenght;
    }


    private IEnumerator PlayHurt()
    {
        _enemy.StopMovement();
        PlayHurtAnimation();

        yield return new WaitForSeconds(GetCurrentAnimationLenght());
        _enemy.ResetMovement();
    }


    private IEnumerator PlayDead()
    {
        _enemy.StopMovement();
        PlayDieAnimation();

        yield return new WaitForSeconds(GetCurrentAnimationLenght() + 0.3f);
        _enemy.ResetMovement();
        _enemyHealth.ResetHealth();
        ObjectPooler.ReturnToPool(_enemy.gameObject);
    }

    private void EnemyHit(Enemy enemy)
    {
        if(_enemy == enemy)
        {
            StartCoroutine(PlayHurt());
        }
    }





    private void EnemyDead(Enemy enemy)
    {
        if(_enemy == enemy) 
        {
            StartCoroutine(PlayDead());
        }
    }


    private void OnEnable()
    {
        EnemyHealth.OnEnemyHit += EnemyHit;
        EnemyHealth.OnEnemyKilled += EnemyDead;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyHit -= EnemyHit;
        EnemyHealth.OnEnemyKilled -= EnemyDead;
    }

}
