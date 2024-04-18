using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    public Transform target;
    ConfigurableJoint cjoint;
    Quaternion startRotation;

    float speed = 350.0f;

    // Start is called before the first frame update
    void Start()
    {
        cjoint = GetComponent<ConfigurableJoint>();
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseXvalue = Input.GetAxis("Mouse X") * Time.deltaTime * speed;
        float mouseYvalue = Input.GetAxis("Mouse Y") * Time.deltaTime * speed;

        if (mouseXvalue != 0 && (target.rotation.eulerAngles.z < 120 || target.rotation.eulerAngles.z > -70))
        {
            //print("Mouse X movement: " + mouseXvalue);
            target.transform.Rotate(0, 0, mouseXvalue, Space.Self);
        }
        if (mouseYvalue != 0 && (target.rotation.eulerAngles.y < 50 || target.rotation.eulerAngles.y > -50))
        {
            //print("Mouse Y movement: " + mouseYvalue);
            target.transform.Rotate(0, mouseYvalue, 0, Space.Self);
        }

        // target.transform.Rotate(0, mouseYvalue, mouseXvalue, Space.Self);

        ConfigurableJointExtensions.SetTargetRotationLocal(cjoint, target.rotation, startRotation);
    }
}
