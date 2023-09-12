using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atack : MonoBehaviour
{
    private void Update()
    {
        atack();
    }

    public GameObject obj;
    public void atack()
    {
        if (obj == true)
        {
            obj.SetActive(true);
        }
    }
}
