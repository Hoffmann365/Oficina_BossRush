using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rig;

    public float speed;
    
    public bool isRight;
    
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isRight)
        {
            rig.velocity = Vector2.right * speed;
        }
        else
        {
            rig.velocity = Vector2.left * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Boss1"))
        {
            Destroy(gameObject);
        }
        if (coll.gameObject.CompareTag("Boss3"))
        {
            Destroy(gameObject);
        }
    }
}
