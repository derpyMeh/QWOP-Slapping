using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlapControls : MonoBehaviour
{
    [SerializeField] Rigidbody rightUpperRB;
    [SerializeField] Rigidbody rightLowerRB;
    [SerializeField] Rigidbody rightHandRB;
    
    float horizontalAxis = 0;
    float verticalAxis = 0;
    float scrollY = 0;
    float forceAmount = 100;
    float liftAmount = 200;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        verticalAxis = Input.GetAxis("Mouse Y");
        horizontalAxis = Input.GetAxis("Mouse X");
        scrollY = Input.mouseScrollDelta.y;

        if (horizontalAxis != 0 | verticalAxis != 0)
        {
            Debug.Log($"X:{horizontalAxis}, Y:{verticalAxis}");
        }

        // Hand Force:
        Vector3 movementDirection = new Vector3(0, verticalAxis, -horizontalAxis);

        rightHandRB.AddForce(movementDirection * forceAmount);

        // Up and Down:
        if (scrollY > 0)
        {
            Vector3 liftDirection = new Vector3(0, 1, 0);
            rightUpperRB.AddForce(liftDirection * liftAmount);
            rightLowerRB.AddForce(liftDirection * liftAmount);
            rightHandRB.AddForce(liftDirection * liftAmount);
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E");
        }
    }
}
