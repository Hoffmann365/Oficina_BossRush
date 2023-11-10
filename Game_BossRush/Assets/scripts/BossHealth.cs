using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public int health = 500;
    public Slider healthBar;

    public bool isInvulnerable = false;

    public int flechaDmg;
    public int raioDmg;
    
    private Animator animator;
    
    public AudioSource audioSource;
    public AudioClip hurtSound;
    public AudioClip deathSound;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        health -= damage;
        healthBar.value = health;
        
        audioSource.PlayOneShot(hurtSound);

        if (health <= 200)
        {
            GetComponent<Animator>().SetBool("isEnraged", true);
        }

        if (health <= 0)
        {
            audioSource.PlayOneShot(deathSound);
            StartCoroutine(DieAnimation());
        }
        else
        {
            StartCoroutine(HurtAnimation());
        }
        
        IEnumerator HurtAnimation()
        {
            animator.SetBool("Hurt", true);
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("Hurt", false);
        }
        
        IEnumerator DieAnimation()
        {
            animator.SetBool("Dead", true);
            yield return new WaitForSeconds(2.0f); // Tempo da animação de morte
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Flecha")
        {
            TakeDamage(flechaDmg);

        }
        

        if (col.gameObject.tag == "RaioPlayer")
        {
            TakeDamage(raioDmg);
        }
    }
    

}
