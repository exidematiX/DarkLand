using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator _animator;
    private EnemyAI _enemy;
    private EnemyHealth _enemyHealth;

    private void Start()
    {
        _animator  = transform.Find("CharacterImage").gameObject.GetComponent<Animator>();
        _enemy = GetComponent<EnemyAI>();
        _enemyHealth = GetComponent<EnemyHealth>();
    }


    private void PlayHurtAnimation()
    {
        //_animator.SetTrigger("Hurt");
    }

    private void PlayDieAnimation()
    {
        //_animator.SetTrigger("Die");

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
        Pooler.ReturnToPool(_enemy.gameObject);
    }

    private void EnemyHit(EnemyAI enemy)
    {
        if (_enemy == enemy)
        {
            StartCoroutine(PlayHurt());
        }
    }





    private void EnemyDead(EnemyAI enemy)
    {
        if (_enemy == enemy)
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
