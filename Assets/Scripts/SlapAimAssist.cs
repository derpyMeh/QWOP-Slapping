using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlapAimAssist : MonoBehaviour
{
    private Vector3 m_Input;
    
    [SerializeField] private Rigidbody slapHandRigidbody;
    [SerializeField] private Transform targetRB;
    
    public float m_Speed = 5f;
    private float distanceLASTFrame;
    private float distanceTHISFrame;

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
    }

    void FixedUpdate()
    {
        
        //Store user input as a movement vector
        m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //Apply the movement vector to the current position, which is
        //multiplied by deltaTime and speed for a smooth MovePosition
        slapHandRigidbody.MovePosition(targetRB.position + m_Input * Time.deltaTime * m_Speed);         //This needs to be moved down in an if statement

        distanceLASTFrame = Vector3.Distance(slapHandRigidbody.position, targetRB.position);
        
    }

    private void LateUpdate()
    {
        distanceTHISFrame = Vector3.Distance(slapHandRigidbody.position, targetRB.position);

        if (distanceTHISFrame < distanceLASTFrame)
        {
            //slapHandRigidbody.MovePosition(targetRB.position + m_Input * Time.deltaTime * m_Speed); //MOVED
        }
    }
}
