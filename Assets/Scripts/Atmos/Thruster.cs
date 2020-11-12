using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thruster : MonoBehaviour
{
    // vars
    public Rigidbody rigid;
    public float thrust = 0f;
    public float throttle = 0f;


    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        rigid.AddForceAtPosition(thrust*throttle*Vector3.forward, transform.position);
    }
}
