using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlapAimAssist : MonoBehaviour
{
    private Vector3 m_Input;
    
    [SerializeField] private Rigidbody slapHandRigidbody;
    [SerializeField] private Transform targetRB;
    
    [SerializeField] private ArmController armControllerScript;
    
    public float m_Speed = 5f;
    private float distanceLASTFrame;
    private float distanceTHISFrame;
    [SerializeField] private float multiplier;

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        distanceLASTFrame = 0;
    }

    void FixedUpdate()
    {
        
        //_rigid2d.AddForce(transform.up * _value2);
        //Store user input as a movement vector
        //m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        //Apply the movement vector to the current position, which is
        //multiplied by deltaTime and speed for a smooth MovePosition
        //slapHandRigidbody.MovePosition(targetRB.position + m_Input * Time.deltaTime * m_Speed);         //This needs to be moved down in an if statement
        
        //distanceLASTFrame = Vector3.Distance(slapHandRigidbody.position, targetRB.position);
    }

    private void LateUpdate()
    {
       
    }

    private void Update()
    {
        distanceTHISFrame = Vector3.Distance(slapHandRigidbody.position, targetRB.position);
                
        //Debug.Log("XXX: " + armControllerScript.mouseXvalue);
                
        if (distanceTHISFrame < distanceLASTFrame && (armControllerScript.mouseXvalue > 0.015))
        {
            slapHandRigidbody.AddForce((targetRB.transform.position - slapHandRigidbody.transform.position).normalized * multiplier);
            /*if (armControllerScript.mouseXvalue >)
            {

            }*/
            //slapHandRigidbody.MovePosition(targetRB.position + m_Input * Time.deltaTime * m_Speed); //MOVED
                    
        }
        
        distanceLASTFrame = Vector3.Distance(slapHandRigidbody.position, targetRB.position);
    }
}
