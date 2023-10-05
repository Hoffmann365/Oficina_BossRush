using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int life = 40;
    public float culdown = 8f;
    public bool stage1 = true;
    public bool stage2;
    public Transform player;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (culdown <= 0)
        {
            culdown = 8f;
        }
        culdown -= Time.deltaTime;
        fire();
    }

    void fire()
    {
        if (life <= 20)
        {
            stage1 = false;
            stage2 = true;
        }
        
        if (culdown <= 0 && stage1 == true)
        {
            anim.SetInteger("Transition", 3);
        }

        if (culdown <= 0 && stage2 == true)
        {
            anim.SetInteger("Transition", 4);
        }
    }
}
