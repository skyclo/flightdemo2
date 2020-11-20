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
    private Vector3 targetEulerAngle;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
            targetDeflection = maxDeflection;
        } else if (Input.GetButton("PitchDown")) {
            targetDeflection = minDeflection;
        } else {
            targetDeflection = restingDeflection;
        }

        targetEulerAngle = new Vector3(targetDeflection, 0, 0);

        if (Vector3.Distance(targetEulerAngle, transform.localEulerAngles) > 0.1f) {
            transform.localEulerAngles = targetEulerAngle;
            /* transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, targetEulerAngle, 1f*Time.fixedDeltaTime);
            transform.localEulerAngles *= Mathf.Sign(targetDeflection); */ //TODO
        } else {
            transform.localEulerAngles = targetEulerAngle;
        }

        /* Debug.Log("Target: " + targetDeflection + "\t Current: " + transform.localEulerAngles.ToString() + "\t EAngles: " + transform.eulerAngles.ToString()); */
    }
}
