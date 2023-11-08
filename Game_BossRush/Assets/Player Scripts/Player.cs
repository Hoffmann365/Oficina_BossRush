using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerSkills
{
    normalBow,
    laserBow
}

public class Player : MonoBehaviour
{
    public PlayerSkills currentWeapon = PlayerSkills.normalBow;
    public float speed;
    public float jumpForce;
    public int health;
    public bool onAir;
    public bool unlockWpnSwitch;
    public GameObject arrow;
    public Transform firePoint;
    public Transform healthBar;
    public GameObject healthBarObject;
    
    public static float movement;

    private Vector3 healthBarScale;
    private float healthPercent;
    
    private bool isJumping;
    private bool isFire;
    
    public int bulletBossDmg;
    public int raioBossDmg;
    public BoxCollider2D feet;
    
    
    private Rigidbody2D rig;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //barra de vida
        healthBarScale = healthBar.localScale;
        healthPercent = healthBarScale.x / health;
    }
    

    private void OnTriggerStay2D(Collider2D feet)
    {
        onAir = false;
        isJumping = false;

    }

    private void OnTriggerExit2D(Collider2D feet)
    {
        onAir = true;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("BulletBoss"))
        {
            Damage(bulletBossDmg);
        }
        if (coll.gameObject.CompareTag("RaioBoss"))
        {
            Damage(raioBossDmg);
        }
        
    }

    void UpdateHealthBar()
    {
        healthBarScale.x = healthPercent * health;
        healthBar.localScale = healthBarScale;
    }

    // Update is called once per frame
    void Update()
    {
        
        Shoot();
        Unbug();
        if (unlockWpnSwitch)
        {
            WeaponSwitch();
        }
    }

    void FixedUpdate()
    {
        Move();
        Jump();
        
    }

    void Move()
    {
        movement = Input.GetAxis("Horizontal");

        rig.velocity = new Vector2(movement * speed, rig.velocity.y);
        
        if (movement > 0)
        {
            if(!isJumping)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0,0,0);
            
        }
        if (movement < 0)
        {
            if(!isJumping)
            {
                anim.SetInteger("transition", 1);
            }
            
            transform.eulerAngles = new Vector3(0,180,0);
            
            
            
            
            
        }
        if(movement == 0 && !onAir && !isJumping && !isFire)
        {
            anim.SetInteger("transition", 0);
        }

        if (isFire == true && !onAir)
        {
            movement = 0;
        }

        
        
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && !onAir)
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isJumping = true;
            }
        }
    }
    

    void WeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentWeapon == PlayerSkills.normalBow)
            {
                currentWeapon = PlayerSkills.laserBow;
            }
            else if (currentWeapon == PlayerSkills.laserBow)
            {
                currentWeapon = PlayerSkills.normalBow;
            }
        }
    }

    void Shoot()
    {
        StartCoroutine("BowFire");
    }

    IEnumerator BowFire()
    {
        if (currentWeapon == PlayerSkills.normalBow)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isFire = true;
                anim.SetBool("Attacking1", true);
                anim.SetInteger("transition", 3);
                yield return new WaitForSeconds(2f);
                isFire = false;
            }
        }
        //falta configurar o segundo tiro
        else if (currentWeapon == PlayerSkills.laserBow)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isFire = true;
                anim.SetBool("Attacking2", true);
                anim.SetInteger("transition", 4);
                yield return new WaitForSeconds(2f);
                isFire = false;
            }
        }
    }

    void EndAnimationATK()
    {
        anim.SetBool("Attacking1", false);
        anim.SetBool("Attacking2", false);
        anim.SetInteger("transition", 0);
    }

    void InstArrow()
    {
        GameObject Arrow = Instantiate(arrow, firePoint.position, firePoint.rotation);
        if (transform.rotation.y == 0)
        {
            Arrow.GetComponent<Arrow>().isRight = true;
        }
        if (transform.rotation.y == 180)
        {
            Arrow.GetComponent<Arrow>().isRight = false;
        }
    }

    void Damage(int dmg)
    {
        anim.SetBool("Attacking1", false);
        anim.SetBool("Attacking2", false);
        health -= dmg;
        UpdateHealthBar();
        anim.SetTrigger("hit");
        
        if (transform.rotation.y == 0)
        {
            transform.position += new Vector3(-0.5f, 0, 0);
        }

        if (transform.rotation.y == 180)
        {
            transform.position += new Vector3(0.5f, 0, 0);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        
        anim.SetTrigger("die");
        Destroy(rig);
        Destroy(GetComponent<CapsuleCollider2D>());
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(gameObject, 0.8f);
    }

    void Unbug()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            anim.SetBool("Attacking1", false);
            anim.SetBool("Attacking2", false);
        }
    }
}
