using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Atmos : MonoBehaviour
{
    public Rigidbody rigid;
    public Canvas ui;
    public float wingArea = 0f;
    public float maxThrust = 0f;
    public float airDensity = 0f;
    
    private Vector3 localVel = new Vector3(0f,0f,0f);
    private float angleOfAttack = 0f;
    private float liftCoefficient = 0f;
    private float liftForce = 0f;
    private Vector3 liftDirection = Vector3.up;

    private AnimationCurve liftCurve = new AnimationCurve(
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

    void Awake()
    {
        rigid = GetComponentInParent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        /* Debug.Log("AOA: " + angleOfAttack);
        Debug.Log("CL: " + liftCoefficient);
        Debug.Log("LForce: " + liftForce); */
        Debug.DrawRay(transform.position, liftForce*liftDirection, Color.blue);
    }

    private void FixedUpdate()
    {
        localVel = transform.InverseTransformDirection(rigid.GetPointVelocity(transform.position));
        localVel.x = 0.0f;

        angleOfAttack = Vector3.Angle(Vector3.forward, localVel);
        
        ApplyLiftForce();

        rigid.AddRelativeForce(Vector3.forward*maxThrust);

        

        ui.transform.Find("AOA").GetComponent<Text>().text = "AOA: " + angleOfAttack;
        ui.transform.Find("CL").GetComponent<Text>().text = "CL: " + liftCoefficient;
        ui.transform.Find("LF").GetComponent<Text>().text = "LF: " + liftForce + " (" + liftDirection + ")";
    }

    // Methods
    private void ApplyLiftForce()
    {
        liftCoefficient = liftCurve.Evaluate(angleOfAttack);
        liftForce = localVel.sqrMagnitude*wingArea*liftCoefficient*airDensity*(-Mathf.Sign(localVel.y));
        liftDirection = Vector3.Cross(rigid.velocity, transform.right).normalized;

        rigid.AddForceAtPosition(liftForce*liftDirection, rigid.transform.TransformPoint(rigid.centerOfMass));
    }
}
