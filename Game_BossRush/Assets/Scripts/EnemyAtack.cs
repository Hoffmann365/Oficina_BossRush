using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAtack : MonoBehaviour
{
    
    [Header("Variaveis: ")]
    public float life = 40f;
    public float currentHealth;
    public float speed = 2;
    public float TimeForMove = 1.5f;
    public float timer;
    public float walktime;
    public float bulletSpeed = 10f;
    public float fireRate = 5f;
    public float timeSinceLastShot = 5f;
    public bool stage1 = true;
    public bool stage2;
    public bool walkRight = true;
    public bool Isdead;
    public bool isfire;
    public bool parado;

    [Header("Componentes: ")]
    public Rigidbody2D rig;
    public Animator anim;
    public GameObject bulletPrefab;
    public GameObject bullet2;
    public Transform firePoint;
    public Transform player;
    public Transform firepoint2;
    public AudioClip[] audios;
    public AudioSource source;
    public Slider healthSlider;
    
    private void Start()
    {
        source = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        currentHealth = life;
        UpdateHealthBar();
    }
    void Update()
    {
        if (Isdead == false)
        {
            dead();
            RotationsAndCalculos();
        }

        if (isfire == true)
        {
            TimeForMove -= Time.deltaTime;
            if (TimeForMove <= 0)
            {
                isfire = false;
                TimeForMove = 1.5f;
            }
        }

    }
    private void FixedUpdate()
    {
        if (Isdead == false && parado == false)
        {
            move();
        }
    }

    void Shoot()
    {
        isfire = true;
        anim.SetInteger("Transition", 3);
        PlaySound(1);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.right * bulletSpeed;
        Destroy(bullet, 2f);
    }
    void Shoot2()
    {
        isfire = true;
        anim.SetInteger("Transition", 4);
        PlaySound(0);
        GameObject bullet2 = Instantiate(this.bullet2, firepoint2.position, firepoint2.rotation);
        Destroy(bullet2, 2f);
    }
    void move()
    {
        if (stage1 == true)
        {
            timer += Time.deltaTime;
        
            if (timer >= walktime)
            {
                walkRight = !walkRight;
                timer = 0f;
            }
            if (walkRight)
            {
                if (isfire == false)
                {
                    anim.SetInteger("Transition", 1);
                }
                transform.eulerAngles = new Vector2(0,0);
                rig.velocity = Vector2.right * speed;
            }

            if (!walkRight)
            {
                if (isfire == false)
                {
                    anim.SetInteger("Transition", 1);
                }
                transform.eulerAngles = new Vector2(0,180);
                rig.velocity = Vector2.left * speed;
            }
        }

        if (stage2 == true)
        {
            speed = 10f;
            timer = 0f;
            walktime = 0f;
            walkRight = true;
            if (isfire == false)
            {
                anim.SetInteger("Transition", 1);
            }
            transform.eulerAngles = new Vector2(0,180);
            rig.velocity = Vector2.right * speed;

        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "BalaPlayer")
        {
            life--;
            currentHealth = Mathf.Clamp(currentHealth, 0f, life);
            UpdateHealthBar();
        }
    }
    void dead()
    {
        if (life <= 0)
        {
            PlaySound(2);
            Isdead = true;
            anim.SetTrigger("Dead");
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(gameObject, 5f);
            
        }
    }
    void RotationsAndCalculos()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not found.");
            return;
        }
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.Normalize();
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        if (stage1 == true)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
        
        timeSinceLastShot -= Time.deltaTime;
        if (timeSinceLastShot <=0 && stage1 == true)
        {
            Shoot();
            timeSinceLastShot = 4f;
        }
        if (timeSinceLastShot <=0 && stage2 == true)
        {
            Shoot2();
            timeSinceLastShot = 5f;
        }

        if (life <= 20)
        {
            stage1 = false;
            stage2 = true;
        }
    }
    void PlaySound(int sound)
    {
        if (sound >= 0 && sound < audios.Length)
        {
            source.clip = audios[sound];
            source.Play();
        }
    }
    private void UpdateHealthBar()
    {
        healthSlider.value = currentHealth;
    }
    
}