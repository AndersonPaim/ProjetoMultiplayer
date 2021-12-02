using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformRotate : MonoBehaviour
{

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _vAng;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _rb.angularVelocity = _vAng;
        
    }
}
