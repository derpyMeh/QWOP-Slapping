using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlappingVictim : MonoBehaviour
{
    [SerializeField] Rigidbody headRB;
    [SerializeField] Rigidbody bodyRB;
    [SerializeField] Rigidbody rightUpperArmRB;
    [SerializeField] Rigidbody leftUpperArmRB;
    [SerializeField] Rigidbody rightLowerArmRB;
    [SerializeField] Rigidbody leftLowerArmRB;
    [SerializeField] Rigidbody rightHandRB;
    [SerializeField] Rigidbody leftHandRB;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        headRB.isKinematic = true;
        bodyRB.isKinematic = true;
        rightUpperArmRB.isKinematic = true;
        rightLowerArmRB.isKinematic = true;
        rightHandRB.isKinematic = true;
        leftUpperArmRB.isKinematic = true;
        leftLowerArmRB.isKinematic = true;
        leftHandRB.isKinematic = true;
    }
}
