using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LegsController : MonoBehaviour
{
    public bool isWalking = false;
    JointDrive jointDrive;

    public GameObject thigh_l;
    public Transform thigh_l_target;
    ConfigurableJoint thigh_l_joint;
    Quaternion thigh_l_startRotation;
    float current_thigh_l_XRotation;
    float current_thigh_l_YRotation;
    float current_thigh_l_ZRotation;

    public GameObject thigh_r;
    public Transform thigh_r_target;
    ConfigurableJoint thigh_r_joint;
    Quaternion thigh_r_startRotation;
    float current_thigh_r_XRotation;
    float current_thigh_r_YRotation;
    float current_thigh_r_ZRotation;

    float speed = 30.0f;

    public float maxX = 90f;
    public float minX = -90f;
    public float maxY = 90f;
    public float minY = -90f;
    public float maxZ = 90f;
    public float minZ = -90f;

    // Start is called before the first frame update
    void Start()
    {
        // Left Setup
        thigh_l_joint = thigh_l.GetComponent<ConfigurableJoint>();
        thigh_l_startRotation = thigh_l.transform.rotation;
        current_thigh_l_XRotation = thigh_l_startRotation.x;
        current_thigh_l_YRotation = thigh_l_startRotation.y;
        current_thigh_l_ZRotation = thigh_l_startRotation.z;

        // Right Setup
        thigh_r_joint = thigh_r.GetComponent<ConfigurableJoint>();
        thigh_r_startRotation = thigh_r.transform.rotation;
        current_thigh_r_XRotation = thigh_r_startRotation.x;
        current_thigh_r_YRotation = thigh_r_startRotation.y;
        current_thigh_r_ZRotation = thigh_r_startRotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            // Left Controls
            if (Input.GetKey(KeyCode.Q))
            {
                current_thigh_l_ZRotation += (speed * Time.deltaTime);
                current_thigh_l_ZRotation = Mathf.Clamp(current_thigh_l_ZRotation, minZ, maxZ);
            }

            if (Input.GetKey(KeyCode.W))
            {
                current_thigh_l_ZRotation -= (speed * Time.deltaTime);
                current_thigh_l_ZRotation = Mathf.Clamp(current_thigh_l_ZRotation, minZ, maxZ);
            }

            // Right Controls
            if (Input.GetKey(KeyCode.O))
            {
                current_thigh_r_ZRotation += (speed * Time.deltaTime);
                current_thigh_r_ZRotation = Mathf.Clamp(current_thigh_r_ZRotation, minZ, maxZ);
            }

            if (Input.GetKey(KeyCode.P))
            {
                current_thigh_r_ZRotation -= (speed * Time.deltaTime);
                current_thigh_r_ZRotation = Mathf.Clamp(current_thigh_r_ZRotation, minZ, maxZ);
            }

            Debug.Log("Left " + current_thigh_l_ZRotation);
            Debug.Log("Right " + current_thigh_r_ZRotation);

            // Set Left Rotation
            SetCurrentRotation(thigh_l_target, current_thigh_l_XRotation, current_thigh_l_YRotation, current_thigh_l_ZRotation);
            ConfigurableJointExtensions.SetTargetRotationLocal(thigh_l_joint, thigh_l_target.rotation, thigh_l_startRotation);

            // Set Right Rotation
            SetCurrentRotation(thigh_r_target, current_thigh_r_XRotation, current_thigh_r_YRotation, current_thigh_r_ZRotation);
            ConfigurableJointExtensions.SetTargetRotationLocal(thigh_r_joint, thigh_r_target.rotation, thigh_r_startRotation);
        }
    }

    void SetCurrentRotation(Transform target, float xRot, float yRot, float zRot)
    {
        xRot = Mathf.Clamp(xRot, minX, maxX);
        yRot = Mathf.Clamp(yRot, minY, maxY);
        zRot = Mathf.Clamp(zRot, minZ, maxZ);
        target.localRotation = Quaternion.Euler(xRot, yRot, zRot);
    }

    public void ResetTargetRotation(Quaternion startRotation, float xRot, float yRot, float zRot)
    {
        xRot = startRotation.x;
        yRot = startRotation.y;
        zRot = startRotation.z;
    }

    void SetSpringValues(ConfigurableJoint cjoint, float newSpringValue)
    {
        JointDrive jointXDrive = cjoint.angularXDrive;
        jointXDrive.positionSpring = newSpringValue;
        cjoint.angularXDrive = jointXDrive;

        JointDrive jointYZDrive = cjoint.angularYZDrive;
        jointYZDrive.positionSpring = newSpringValue;
        cjoint.angularYZDrive = jointYZDrive;
    }
}
