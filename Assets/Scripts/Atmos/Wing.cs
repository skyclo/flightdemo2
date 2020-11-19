using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wing : MonoBehaviour
{

    /* ---------------------------------------------------------------------------------------------- */
    /*                                         CLASS VARIABLES                                        */
    /* ---------------------------------------------------------------------------------------------- */
    /* ------------------------------------------ Rigidbody ----------------------------------------- */
    public Rigidbody rigid;

    /* ---------------------------------------- Wing Settings --------------------------------------- */
    public float wingArea               = 0f;
    private float angleOfAttack         = 0f;
    private float airDensity            = 1.229f;
    private Vector3 localVelocity;
    
    /* ---------------------------------------- Lift And Drag --------------------------------------- */
    public AnimationCurve liftCurve     = new AnimationCurve(
        new Keyframe(0f, 0.3f), 
        new Keyframe(35f, 1.9f),
        new Keyframe(45f, 0f),
        new Keyframe(50f, 0.5f),
        new Keyframe(90f, 0f),
        new Keyframe(130f, -0.5f),
        new Keyframe(135f, 0f),
        new Keyframe(145f, -1.9f),
        new Keyframe(180f, -0.3f)
    );
    public AnimationCurve dragCurve     = new AnimationCurve(
        new Keyframe(0f, 0.001f),
        new Keyframe(40f, 1.7f),
        new Keyframe(180f, 0.01f)
    );
    private float liftCoefficient       = 0f;
    private float liftForce             = 0f;
    private float dragCoefficient       = 0f;
    private float dragForce             = 0f;
    private Vector3 liftDirection;
    private Vector3 liftVector;
    private Vector3 dragDirection;
    private Vector3 dragVector;

    

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponentInParent<Rigidbody>();
        liftDirection = Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, liftVector*0.0001f, Color.green);
        Debug.DrawRay(transform.position, dragVector*0.0001f, Color.red);
        Debug.Log("Lift Direction: " + liftDirection + "\tLift Force: " + liftForce + "\tCL: " + liftCoefficient + "\tAoA: " + angleOfAttack);
        Debug.Log("Drag Direction: " + dragDirection + "\tDrag Force: " + dragForce + "\tCD: " + dragCoefficient + "\tAoA: " + angleOfAttack);
    }

    void FixedUpdate()
    {
        localVelocity = transform.InverseTransformDirection(rigid.GetPointVelocity(rigid.transform.position));
        localVelocity.x = 0f;
        angleOfAttack = Vector3.Angle(Vector3.forward, localVelocity.normalized);
        liftCoefficient = liftCurve.Evaluate(angleOfAttack);
        dragCoefficient = dragCurve.Evaluate(angleOfAttack);

        liftForce = liftCoefficient * airDensity  * localVelocity.sqrMagnitude * wingArea * 0.5f * -Mathf.Sign(localVelocity.normalized.y);
        liftDirection = Vector3.Cross(rigid.velocity, Vector3.right).normalized;
        liftVector = liftForce * liftDirection;

        dragForce = dragCoefficient * airDensity * localVelocity.sqrMagnitude * wingArea * 0.5f;
        dragDirection = -rigid.velocity.normalized;
        dragVector = dragForce * dragDirection;

        Vector3 applicationPoint = transform.position;
        rigid.AddForceAtPosition(liftVector, applicationPoint);
        rigid.AddForceAtPosition(dragVector, applicationPoint);
    }
}
