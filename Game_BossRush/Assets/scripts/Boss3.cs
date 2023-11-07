using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Boss3 : MonoBehaviour
{
    public float speed = 2.0f;
    public float walkTime;
    public bool walkRight = true;
    public float attackRange = 2.0f;
    public float attackCooldown = 2.0f;
    
    public int health;

    private bool enraged;
    private Transform playerTransform;
    private float lastAttackTime;
    private float timer;
    private bool isAtk;
    
    private Rigidbody2D rig;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
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
        AttackPlayer();
    }

    private void Enrage()
    {
        anim.SetTrigger("Run");
        enraged = true;
        //mudar o comportamento após ficar enfurecido
    }

    private void Wander()
    {
        anim.SetTrigger("Walk");
        timer += Time.deltaTime;

        if (timer >= walkTime)
        {
            walkRight = !walkRight;
            timer = 0f;
        }

        if (walkRight)
        {
            transform.eulerAngles = new Vector3(0, 0);
            rig.velocity = Vector3.right * speed;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180);
            rig.velocity = Vector3.left * speed;
        }
    }

    private void AttackPlayer()
    {
        //Lógica de ataque agressivo
        if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            StartCoroutine("ATK");
        }
    }

    IEnumerator ATK()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            isAtk = true;
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(1f);
        }
        
    }

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
