using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{

    Collider m_Collider;
    public GameObject enemyGameObject;
    public Animator enemyAnimator;
    public float health = 50f;
    public bool enemyIsDying = false;
    AnimatorClipInfo[] currentEnemyClip;
    string enemyClipName;


    public void Start()
    {
        //enemyAnimator = enemyGameObject.GetComponent<Animator>();
        m_Collider = GetComponent<Collider>();
    }

    public void Update()
    {
        //currentEnemyClip = enemyAnimator.GetCurrentAnimatorClipInfo(0);
        //enemyClipName = currentEnemyClip[0].clip.name;
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        //if (this.enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false)
        //{
         //   enemyAnimator.Play("Damage");
        //}
        if (health <= 0f)
        {
            Die();
        }
    }


    void Die()
    {
       // enemyAnimator.Play("Death");
        enemyIsDying = true;
        m_Collider.enabled = !m_Collider.enabled;
        Invoke("EnemyDeath", 1f);
    }

    void EnemyDeath()
    {
        Destroy(this.gameObject);
    }
}

