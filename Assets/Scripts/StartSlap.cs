using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSlap : MonoBehaviour
{
    public Transform rightHandTarget; // The target transform for the right hand
    public float slapForce = 10f; // The force of the slap
    public float slapDistance = 1f; // The distance of the slap
    
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
    
        Vector3 originalPosition = rightHandTarget.localPosition;
        Quaternion originalRotation = rightHandTarget.localRotation;
    
        // Move hand forward
        for (float t = 0; t < 1f; t += Time.deltaTime / slapDistance)
        {
            rightHandTarget.localPosition = Vector3.Lerp(originalPosition, originalPosition + Vector3.forward * slapDistance, t);
            yield return null;
        }
    
        // Apply slap force
        RaycastHit hit;
        if (Physics.Raycast(rightHandTarget.position, rightHandTarget.forward, out hit, slapDistance))
        {
            if (hit.transform.CompareTag("Opponent"))
            {
                Rigidbody opponentRb = hit.transform.GetComponent<Rigidbody>();
                if (opponentRb != null)
                {
                    opponentRb.AddForceAtPosition(rightHandTarget.forward * slapForce, hit.point, ForceMode.Impulse);
                }
            }
        }
    
        // Return hand to original position
        for (float t = 0; t < 1f; t += Time.deltaTime / slapDistance)
        {
            rightHandTarget.localPosition = Vector3.Lerp(originalPosition + Vector3.forward * slapDistance, originalPosition, t);
            yield return null;
        }
    
        slapping = false;
        rightHandTarget.localPosition = originalPosition;
        rightHandTarget.localRotation = originalRotation;
    }
}
