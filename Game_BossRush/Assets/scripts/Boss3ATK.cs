using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3ATK : MonoBehaviour
{
    private BoxCollider2D colliderAtkBoss3;
    // Start is called before the first frame update
    void Start()
    {
        colliderAtkBoss3 = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Boss3.isFlipped)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (Boss3.isFlipped == false)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
}
