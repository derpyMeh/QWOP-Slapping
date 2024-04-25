using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCollider : MonoBehaviour
{
    [SerializeField] private ToggleRagdoll p1ToggleRagdoll;
    [SerializeField] private ToggleRagdoll p2ToggleRagdoll;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player1" && p1ToggleRagdoll.Walking)
        {
            p1ToggleRagdoll.Walking = false;
            p1ToggleRagdoll.Respawn();
        }
        if (collision.gameObject.tag == "Player2" && p2ToggleRagdoll.Walking)
        {
            p2ToggleRagdoll.Walking = false;
            p2ToggleRagdoll.Respawn();
        }
    }
}
