using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.Examples.Pong
{

    public class Bullet : NetworkBehaviour
    {
        public int tipoArma;
       // public WeaponBalancer[] _weaponBalancer;
        public WeaponBalancer _weaponBalancer;
        private Rigidbody2D _rb;
        private Animator _anim;

        public void Shoot()
        {
            //TODO PASSAR SCRIPTABLE OBJECTS COM OS STATS DA BULLET
            _rb = gameObject.GetComponent<Rigidbody2D>();
            _rb.velocity = transform.right * _weaponBalancer.velocity;


            _rb.mass = _weaponBalancer.weight;
            Debug.Log("tipoArma_" + tipoArma);
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
            Knockback();
        }

        private void Knockback()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _weaponBalancer.blastRadius);

            foreach (Collider2D collider in colliders)
            {
                Player player = collider.GetComponent<Player>();

                if (player != null)
                {
                    if(player.Connection.hasAuthority)
                    {
                        Vector2 direction = collider.transform.position - transform.position;
                        collider.GetComponent<Rigidbody2D>().AddForce(direction * _weaponBalancer.knockback);
                    }
                }
            }
        }


    }
}
