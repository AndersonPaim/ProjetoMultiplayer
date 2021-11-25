using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponBalancer", menuName = "WeaponBalancer")]
public class WeaponBalancer : ScriptableObject
{
    public float damage;
    public float blastRadius;
    public float knockback;
    public float weight;
    public float velocity;
    public float reloading;
    public Sprite sprite;
}