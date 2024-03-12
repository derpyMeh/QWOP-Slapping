using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyStabilization : MonoBehaviour
{
    [SerializeField] Transform StabilizingPoint;
    [SerializeField] GameObject Body;
    
    Rigidbody rigidBody;
    
    float forceAmount = 500;
    float rotateAmount = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = Body.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 stabilizingVector = StabilizingPoint.position - Body.transform.position;

        rigidBody.AddForce(stabilizingVector * forceAmount);

        float currentRotationX = Mathf.LerpAngle(Body.transform.rotation.x, StabilizingPoint.rotation.x, rotateAmount);
        float currentRotationY = Mathf.LerpAngle(Body.transform.rotation.y, StabilizingPoint.rotation.y, rotateAmount);
        float currentRotationZ = Mathf.LerpAngle(Body.transform.rotation.z, StabilizingPoint.rotation.z, rotateAmount);
        Body.transform.eulerAngles = new Vector3(currentRotationX, currentRotationY, currentRotationZ);
    }
}
