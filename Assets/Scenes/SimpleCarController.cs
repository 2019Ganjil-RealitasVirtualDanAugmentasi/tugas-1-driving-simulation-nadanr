using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;

    public bool motor;
    public bool steering;
    
}

public class SimpleCarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public Transform steerTransform;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    private float initialRotationX, initialRotationY, initialRotationZ;

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;

        initialRotationX = steerTransform.localRotation.eulerAngles.x;
        initialRotationY = steerTransform.localRotation.eulerAngles.y;
        initialRotationZ = steerTransform.localRotation.eulerAngles.z;
    }

    public void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        steerTransform.transform.localEulerAngles = new Vector3((Input.GetAxis("Horizontal")) * -5f, (Input.GetAxis("Horizontal") * 10f), (Input.GetAxis("Horizontal") * -50f));
        //steerTransform.localRotation = Quaternion.Euler(new Vector3(initialRotationZ + (40 * Input.GetAxis("Horizontal")), initialRotationY, initialRotationX));

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            // ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            // ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    private void Update(){
        
    }
}