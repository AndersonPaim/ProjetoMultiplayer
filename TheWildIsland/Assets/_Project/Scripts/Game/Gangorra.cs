using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gangorra : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;

    private void Start()
    {
        StartCoroutine(StartRotation());
    }
    
    private IEnumerator StartRotation()
    {
        yield return new WaitForSeconds(1);
        _rb.freezeRotation = false;
    }
}
