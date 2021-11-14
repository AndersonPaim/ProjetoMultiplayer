using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _anim;

    public void Shoot()
    {
        //TODO PASSAR SCRIPTABLE OBJECTS COM OS STATS DA BULLET
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _rb.velocity = transform.right * 20;
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        _anim = gameObject.GetComponent<Animator>();
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        _anim.SetTrigger("Explosion");
    }
}
