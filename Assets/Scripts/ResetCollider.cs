using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCollider : MonoBehaviour
{
    [SerializeField] private ToggleRagdoll p1ToggleRagdoll;
    [SerializeField] private ToggleRagdoll p2ToggleRagdoll;
    public Transform camTransform;
    public Vector3 currentPos;

    void Start()
    {
      Vector3 currentPos = camTransform.position;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player1") && p1ToggleRagdoll.Walking)
        {
            p1ToggleRagdoll.Walking = false;
            p1ToggleRagdoll.Respawn();
            camTransform.position = currentPos;

        }
        if (other.gameObject.CompareTag("Player2") && p2ToggleRagdoll.Walking)
        {
            p2ToggleRagdoll.Walking = false;
            p2ToggleRagdoll.Respawn();
            camTransform.position = currentPos;
        }
    }
}
