using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRagdoll : MonoBehaviour
{
    AudioSource audioSource;
    Rigidbody rb;
    float forceAmount = 1000f;

    public ConfigurableJoint[] joints;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 2)
        {
            audioSource.Play();

            foreach (var joint in joints)
            {
                JointDrive jointXDrive = joint.angularXDrive;
                jointXDrive.positionSpring = 0f;
                joint.angularXDrive = jointXDrive;

                JointDrive jointYZDrive = joint.angularYZDrive;
                jointYZDrive.positionSpring = 0f;
                joint.angularYZDrive = jointYZDrive;
            }

            Debug.Log(collision.GetContact(0).normal);
            rb.AddForce(collision.GetContact(0).normal * collision.relativeVelocity.magnitude * forceAmount);
        }
    }
}
