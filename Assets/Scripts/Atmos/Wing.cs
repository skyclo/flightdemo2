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
    private Vector3 liftDirection = new Vector3(0f,0f,0f);
    private Vector3 liftVector = new Vector3(0f,0f,0f);
    private Vector3 localVelocity = new Vector3(0f,0f,0f);


    // Start is called before the first frame update
    void Start()
    {
        if (wingArea == 0) {
            wingArea = (dimensions.x + 0.0001f) * (dimensions.y + 0.0001f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        localVelocity = transform.InverseTransformDirection(rigid.GetPointVelocity(transform.position));
        localVelocity.x = 0f;
        angleOfAttack = Vector3.Angle(Vector3.forward, localVelocity);
        liftCoefficient = liftCurve.Evaluate(angleOfAttack);

        liftForce = liftCoefficient * airDensity * localVelocity.sqrMagnitude * wingArea * 0.5f * -Mathf.Sign(localVelocity.y);
        liftDirection = Vector3.Cross(rigid.velocity, transform.right).normalized;

        rigid.AddForceAtPosition(liftForce * liftDirection, transform.position);
    }
}
