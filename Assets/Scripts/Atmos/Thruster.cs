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
    public float initialVelocity = 0f;


    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponentInParent<Rigidbody>();
    }

    void Start()
    {
        if (initialVelocity > 0f) {
            rigid.velocity = new Vector3(0f, 0f, initialVelocity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, (thrust*throttle*rigid.transform.forward)*0.001f, Color.yellow);
        Debug.DrawRay(transform.position, -9.81f*rigid.mass*Vector3.up*0.001f, Color.blue);
    }

    void FixedUpdate()
    {
        rigid.AddForceAtPosition(thrust*throttle*rigid.transform.forward, transform.position);
    }
}
