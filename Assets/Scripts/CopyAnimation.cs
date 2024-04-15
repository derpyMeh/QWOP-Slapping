using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyAnimation : MonoBehaviour
{
    public Transform targetLimb;
    public bool mirror;
    ConfigurableJoint cjoint;
    Quaternion startRotation;

    // Start is called before the first frame update
    void Start()
    {
        cjoint = GetComponent<ConfigurableJoint>();
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        ConfigurableJointExtensions.SetTargetRotationLocal(cjoint, targetLimb.rotation, startRotation);
        
        //if (!mirror)
        //{
        //    cjoint.targetRotation = targetLimb.rotation;
        //}
        //else
        //{
        //    cjoint.targetRotation = Quaternion.Inverse(targetLimb.rotation);
        //}
    }
}
