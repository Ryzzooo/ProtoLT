using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlayerAttack : MonoBehaviour
{
    public Transform attackOrigin;
    public float attackRadius = 1f;
    public LayerMask enemyMask;
 
    public float cooldownTime = .5f;
    private float cooldownTimer = 0f;
 
    public int Damage = 1;
 
    public Animator anim;
    string attack_param = "attack";
 
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (cooldownTimer <= 0)
        {
            if (Input.GetKey(KeyCode.K))
            {
                // Example of playing attack animation
                anim.SetTrigger(attack_param);
                 soundmanager.Instance.PlaySound("CatAttack");
                Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(attackOrigin.position, attackRadius, enemyMask);
                foreach (var enemy in enemiesInRange)
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(Damage);
                }
                cooldownTimer = cooldownTime;
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
 
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);
    }
}