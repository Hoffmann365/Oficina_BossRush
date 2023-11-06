using System;
using System.Collections;
using System.Collections.Generic;
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
    public bool onAir;
    public bool unlockWpnSwitch;
    
    private float movement;
    private bool isJumping;
    private bool isFire;
    
    private Rigidbody2D rig;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        onAir = false;
        isJumping = false;
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        onAir = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Shoot();
        if (unlockWpnSwitch)
        {
            WeaponSwitch();
        }
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
        StartCoroutine("Arrow");
    }

    IEnumerator Arrow()
    {
        //falta configurar pra sair flecha no tiro (só animação rodando)
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
}
