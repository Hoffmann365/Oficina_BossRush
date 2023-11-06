using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3 : MonoBehaviour
{
    public float speed;
    public float walkTime;
    public float life = 40f;
    public float currentHealth;

    public int health;

    private float timer;
    private bool walkRight;

    private Rigidbody2D rig;
    // Start is called before the first frame update
    void Start()
    {
        UpdateHealthBar();
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        
        if(timer >= walkTime)
        {
            walkRight = !walkRight;
            timer = 0f;
        }
        if (walkRight)
        {
            transform.eulerAngles = new Vector2(0, 0);
            rig.velocity = Vector2.right * speed;
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 180);
            rig.velocity = Vector2.left * speed;
        }
        
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Flecha")
        {
            life--;
            currentHealth = Math.Clamp(currentHealth, 0f, life);
            UpdateHealthBar();
        }

        if (col.gameObject.tag == "RaioPlayer")
        {
            life -= 2;
            currentHealth = Math.Clamp(currentHealth, 0f, life);
            UpdateHealthBar();
        }

        void dead()
        {
            if (life <= 0)
            {
                Isdead = true;
                Animation.SetTrigger("Dead");
                Destroy(GetComponent<Rigidbody2D>());
                Destroy(GetComponent<BoxCollider>());
                Destroy(gameObject, 5f);
            }
        }
    }
}
