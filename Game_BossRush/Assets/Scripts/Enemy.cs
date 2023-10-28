using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Variaveis: ")] public int life = 40;
    public bool stage1 = true;
    public bool stage2;
    public bool walkRight = true;
    public bool IsFire;
    public float speed = 2;
    public float timer;
    public float walktime;
    public float bulletSpeed = 10f;
    public float fireRate = 5f;
    public float timeSinceLastShot = 8f;

    [Header("Componentes: ")] public Rigidbody2D rig;
    public Animator anim;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform player;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (life <= 20)
        {
            stage2 = true;
            stage1 = false;
            anim.SetInteger("Transition",6 );
            speed = 6;
            walktime = 2;
            timer = 2;
        }
        AtackEnemy();
    }

    void Shoot()
    {
        IsFire = true;
        anim.SetInteger("Transition", 3);
        // Criar uma bala
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // Aplicar força à bala
        rb.velocity = firePoint.right * bulletSpeed;
        IsFire = false;

        // Destruir a bala após um tempo (ajuste conforme necessário)
        Destroy(bullet, 2f);
    }


    private void FixedUpdate()
    {
        move();
    }

    void move()
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
            transform.eulerAngles = new Vector2(0, 0);
            rig.velocity = Vector2.right * speed;
        }

        if (!walkRight)
        {
            anim.SetInteger("Transition", 1);
            transform.eulerAngles = new Vector2(0, 180);
            rig.velocity = Vector2.left * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "BalaPlayer")
        {
            life--;
        }
    }

    void AtackEnemy()
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
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        timeSinceLastShot -= Time.deltaTime;
        if (timeSinceLastShot <= 0)
        {
            Shoot();
            timeSinceLastShot = 8f;
        }
    }
}

