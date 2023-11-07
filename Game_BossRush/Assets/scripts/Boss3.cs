using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Boss3 : MonoBehaviour
{
    public float speed = 2.0f;
    public float attackRange = 2.0f;
    public float attackCooldown = 2.0f;
    
    public int health;
    private bool enraged;
    private Transform playerTransform;
    private float lastAttackTime;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        enraged = false;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        lastAttackTime = Time.time;
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    public void Update()
    {
        if (health <= 50 && !enraged)
        {
            Enrage();
        }

        if (enraged)
        {
            if (CanAttack())
            {
                //comportamento agressivo
                AttackPlayer();
            }
            
        }
        else
        {
            //comportamento padrão
            Wander();
        }
    }

    private void Enrage()
    {
        enraged = true;
        //mudar o comportamento após ficar enfurecido
    }

    private void Wander()
    {
        //Lógica de movimento padrão 
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void AttackPlayer()
    {
        //Lógica de ataque agressivo
        if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            //StartCoroutine("ATK");
        }
    }

    /*IEnumerator ATK()
    {
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
    }*/

    private bool CanAttack()
    {
        return (Time.time - lastAttackTime) >= attackCooldown;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //lógica de colisão com o jogador
            int damage = 10;
            TakeDamage(damage);
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
    }
}
