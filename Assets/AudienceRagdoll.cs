using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceRagdoll : MonoBehaviour
{
    [SerializeField] private Animator ragdollAnimator;
    
    private Collider[] _ragdollColliders;
    private Rigidbody[] _ragdollRigidbodies;

    [SerializeField] private GameObject rootGameObject;
    [SerializeField] private GameObject activateRagdollCollideGameObject;
    
    // Start is called before the first frame update
    
    /*private enum AudienceState
    {
        Idle,
        Celebrating,
        Ragdoll
    }

    private AudienceState _currentState = AudienceState.Idle;*/
    
    void Awake()
    {
        _ragdollColliders = rootGameObject.GetComponentsInChildren<Collider>();
        
        _ragdollRigidbodies = rootGameObject.GetComponentsInChildren<Rigidbody>();
        
        DisableRagdoll();
        //ragdollAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        /*switch (_currentState)
        {
            case AudienceState.Idle:
                IdleBehaviour();
                break;
            case AudienceState.Ragdoll:
                RagdollBehaviour();
                break;
            default:
                IdleBehaviour();
                break;
        }*/
    }

    private void DisableRagdoll()
    {
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
            gameObject.GetComponent<Animator>().enabled = true;
            activateRagdollCollideGameObject.SetActive(true);
        }
        
        foreach (var collider in _ragdollColliders)
        {
            collider.enabled = false;
        }
    }
    
    private void EnableRagdoll()
    {
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
            gameObject.GetComponent<Animator>().enabled = false;
            //activateRagdollGameobject.SetActive(false);
        }
        
        foreach (var collider in _ragdollColliders)
        {
            collider.enabled = true;
        }
    }

    private void IdleBehaviour()
    {
        
    }
    
    private void RagdollBehaviour()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EnableRagdoll();
            //_currentState = AudienceState.Ragdoll;
        }
        
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DisableRagdoll();
        }
        
    }

    private void OnBecameInvisible()
    {
        DisableRagdoll();
    }
}
