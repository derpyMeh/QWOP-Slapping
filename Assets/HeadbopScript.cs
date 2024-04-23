using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadbopScript : MonoBehaviour
{
    [SerializeField] private ConfigurableJoint HeadJoint;

    [SerializeField] private Rigidbody chestSpine2RigidBody;
    [SerializeField] private Rigidbody headRigidbody;
        
    [SerializeField] private Collider _collider;
    
    private Transform headRigidBodyTransform;
    // Start is called before the first frame update
    void Start()
    {
        headRigidBodyTransform = headRigidbody.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines();
            headRigidbody.isKinematic = false;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ResetHeadpositionAfterDuration());
        }
    }

    IEnumerator ResetHeadpositionAfterDuration()
    {
        yield return new WaitForSeconds(3);
        headRigidbody.isKinematic = true;
        headRigidbody.transform.position = headRigidBodyTransform.position;
    }
}
