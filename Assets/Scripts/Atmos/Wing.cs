using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wing : MonoBehaviour
{
    // vars
    public float wingArea = 0f;
    public Rigidbody rigid;
    public AnimationCurve liftCurve = new AnimationCurve(
        new Keyframe(0f, 0.0f), 
        new Keyframe(5f, 0.38f), 
        new Keyframe(10f, 0.78f), 
        new Keyframe(15f, 1.1f),
        new Keyframe(20f, 1.38f),
        new Keyframe(25f, 1.55f),
        new Keyframe(30f, 1.76f),
        new Keyframe(35f, 1.84f),
        new Keyframe(40f, 1.82f),
        new Keyframe(45f, 1.8f),
        new Keyframe(90f, 0f),
        new Keyframe(135f, -1.2f),
        new Keyframe(180f, 0f)
    );
    
    private float airDensity = 0f;
    private float angleOfAttack = 0f;
    private float liftCoefficient = 0f;
    private float liftForce = 0f;
    private Vector3 liftDirection;
    private Vector3 liftVector;
    private Vector3 localVelocity;


    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponentInParent<Rigidbody>();
        liftDirection = rigid.transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, liftVector*0.00001f, Color.green);
        Debug.Log("Lift Direction: " + liftDirection + "\tLift Force: " + liftForce + "\tCL: " + liftCoefficient + "\tAoA: " + angleOfAttack);
    }

    void FixedUpdate()
    {
        localVelocity = transform.InverseTransformDirection(rigid.GetPointVelocity(rigid.transform.position));
        localVelocity.x = 0f;
        angleOfAttack = Vector3.Angle(rigid.transform.forward, localVelocity.normalized);
        liftCoefficient = liftCurve.Evaluate(angleOfAttack);

        liftForce = liftCoefficient /* * airDensity  */* localVelocity.sqrMagnitude * wingArea /* 0.5f */ * -Mathf.Sign(localVelocity.normalized.y);
        liftDirection = Vector3.Cross(rigid.velocity, rigid.transform.right).normalized;

        liftVector = liftForce * liftDirection;

        Vector3 applicationPoint = transform.position;
        rigid.AddForceAtPosition(liftVector, applicationPoint);
    }
}
