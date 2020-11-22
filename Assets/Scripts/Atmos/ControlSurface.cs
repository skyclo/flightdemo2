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

    public float positiveDeployAngle = 0f;
    private Quaternion positiveDeployRotation;
    public float negativeDeployAngle = 0f;
    private Quaternion negativeDeployRotation;
    public float restingAngle = 0f;
    private Quaternion restingRotation;
    private Quaternion targetRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        if (controlType.Equals(ControlTypesEnum.Pitch)) {
            positiveDeployRotation = Quaternion.Euler(positiveDeployAngle, 0f, 0f);
            negativeDeployRotation = Quaternion.Euler(negativeDeployAngle, 0f, 0f);
            restingRotation = Quaternion.Euler(restingAngle, 0f, 0f);
        } else {
            throw new System.Exception("Invalid Control Type Assigned.");
        }
    }

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
            targetRotation = positiveDeployRotation;
        } else if (Input.GetButton("PitchDown")) {
            targetRotation = negativeDeployRotation;
        } else {
            targetRotation = restingRotation;
        }
        
        if (Quaternion.Angle(transform.localRotation, targetRotation) > 1f) {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, 2.5f*Time.fixedDeltaTime);
        } else {
            transform.localRotation = targetRotation;
        }
    }
}
