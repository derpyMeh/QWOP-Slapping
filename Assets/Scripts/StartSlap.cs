using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSlap : MonoBehaviour
{
    public Transform rightHandTarget; // The target transform for the right hand
    public Transform opponentFace; // The target transform for the opponent's face
    public float slapForce = 10f; // The force of the slap

    private bool slapping = false; // Flag to indicate if currently slapping

    void Update()
    {
        // Check for input to initiate slap
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!slapping)
            {
                StartCoroutine(SlapCoroutine());
            }
        }
    }

    IEnumerator SlapCoroutine()
    {
        slapping = true;

        // Calculate direction towards opponent's face
        Vector3 slapDirection = (opponentFace.position - rightHandTarget.position).normalized;
        
        // Calculate rotation to face the opponent
        Quaternion targetRotation = Quaternion.LookRotation(-slapDirection, Vector3.up);

        // Apply rotation to the hand
        rightHandTarget.rotation = targetRotation;
        
        // Apply slap force
        Rigidbody handRb = rightHandTarget.GetComponent<Rigidbody>();
        if (handRb != null)
        {
            handRb.AddForce(slapDirection * slapForce, ForceMode.Impulse);
        }

        // Wait for the slap to finish (you can adjust the duration as needed)
        yield return new WaitForSeconds(0.5f);

        slapping = false;
    }
}
