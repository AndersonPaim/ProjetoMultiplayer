using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rb;

    public void Shoot()
    {
        //TODO PASSAR SCRIPTABLE OBJECTS COM OS STATS DA BULLET
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _rb.velocity = transform.right * 20;
    }
}
