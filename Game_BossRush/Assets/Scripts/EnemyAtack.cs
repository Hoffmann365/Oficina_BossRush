using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtack : MonoBehaviour
{
    
    [Header("Variaveis: ")]
    public int life = 40;
    public bool stage1 = true;
    public bool stage2;
    public bool walkRight = true;
    public bool Isdead;
    public bool isfire;
    public bool parado;
    public float speed = 2;
    public float timer;
    public float walktime;
    public float bulletSpeed = 10f;
    public float fireRate = 5f;
    public float timeSinceLastShot = 5f;
    
    [Header("Componentes: ")]
    public Rigidbody2D rig;
    public Animator anim;
    public GameObject bulletPrefab;
    public GameObject bullet2;
    public Transform firePoint;
    public Transform player;
    public Transform firepoint2;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (Isdead == false)
        {
            dead();
            RotationsAndCalculos();
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
        anim.SetInteger("Transition", 3);
        // Criar uma bala
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // Aplicar força à bala
        rb.velocity = firePoint.right * bulletSpeed;

        // Destruir a bala após um tempo (ajuste conforme necessário)
        Destroy(bullet, 2f);
    }
    void Shoot2()
    {
        anim.SetInteger("Transition", 4);
            // Criar uma bala
        GameObject bullet2 = Instantiate(this.bullet2, firepoint2.position, firepoint2.rotation);
            
            // Destruir a bala após um tempo (ajuste conforme necessário)
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
                anim.SetInteger("Transition", 1);
                transform.eulerAngles = new Vector2(0,0);
                rig.velocity = Vector2.right * speed;
            }

            if (!walkRight)
            {
                anim.SetInteger("Transition", 1);
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
            anim.SetInteger("Transition", 6);
            transform.eulerAngles = new Vector2(0,180);
            rig.velocity = Vector2.right * speed;

        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        // Tira vida do Boss ao colider com objeto com a tag player
        if (col.gameObject.tag == "BalaPlayer")
        {
            life -=10;
        }
    }

    void dead()
    {
        //Verificar se o Boss Ainda está vivo
        if (life <= 0)
        {
            Isdead = true;
            anim.SetTrigger("Dead");
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(gameObject, 5f);
            
        }
    }

    void RotationsAndCalculos()
    {
        // Verifique se o jogador está disponível (atribua o jogador ao campo 'player' de alguma forma).
        if (player == null)
        {
            Debug.LogWarning("Player not found.");
            return;
        }

        // Calcule a direção do jogador em relação ao boss.
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.Normalize();

        // Calcule o ângulo da direção para o jogador.
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        // Rotação do boss para enfrentar o jogador.
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
}