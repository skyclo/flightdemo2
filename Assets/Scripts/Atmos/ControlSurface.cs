using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSurface : MonoBehaviour
{
    // vars
    public Rigidbody rigid;

    public enum ControlTypesEnum
    {
        Pitch,
        Roll,
        Yaw
    };
    public ControlTypesEnum controlType;
    public Wing wing;

    public float maxDeflection = 0f;
    public float minDeflection = 0f;
    public float restingDeflection = 0f;
    public float maxPossibleDeflection = 0f;
    private float targetDeflection = 0f;
    private float targetAngle = 0f;
    private float currentDeflection = 0f;
    private float maxTorque = 6000f;
    
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
        if (Input.GetButton("PitchUp")) {
            targetDeflection = maxDeflection;
        } else if (Input.GetButton("PitchDown")) {
            targetDeflection = minDeflection;
        } else {
            targetDeflection = restingDeflection;
        }

        targetAngle = targetDeflection;

        maxPossibleDeflection = Mathf.Rad2Deg * Mathf.Asin(maxTorque / (rigid.velocity.sqrMagnitude * wing.wingArea));
        if (!float.IsNaN(maxPossibleDeflection)) {
            targetAngle *= Mathf.Clamp01(maxPossibleDeflection);
        }

        currentDeflection = Mathf.MoveTowards(currentDeflection, targetDeflection, 5f*Time.fixedDeltaTime);
        transform.Rotate(Vector3.right, currentDeflection, Space.Self);
    }
}
