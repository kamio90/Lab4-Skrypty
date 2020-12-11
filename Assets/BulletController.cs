using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(transform.forward * (20 * Time.deltaTime));
        Destroy(gameObject, 4.0f);
    }

    
    
    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
