using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    public Transform target;
    float targetHorizontal;
    float targetVertical;
    ConfigurableJoint cjoint;
    Quaternion startRotation;

    float speed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        cjoint = GetComponent<ConfigurableJoint>();
        startRotation = transform.rotation;
        targetHorizontal = startRotation.z;
        targetVertical = startRotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseXvalue = Input.GetAxis("Mouse X") * speed;
        float mouseYvalue = Input.GetAxis("Mouse Y") * speed;

        if (mouseXvalue != 0)
        {
            print("Mouse X movement: " + mouseXvalue);
            targetHorizontal += mouseXvalue;
        }
        if (mouseYvalue != 0)
        {
            print("Mouse Y movement: " + mouseYvalue);
            targetVertical += mouseYvalue;
        }

        target.transform.Rotate(0, mouseYvalue, mouseXvalue, Space.Self);

        ConfigurableJointExtensions.SetTargetRotationLocal(cjoint, target.rotation, startRotation);
    }
}
