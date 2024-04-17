using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRagdoll : MonoBehaviour
{
    AudioSource audioSource;
    Rigidbody headRB;
    
    float forceAmount = 1000f;
    float scoreMultiplier = 1.0f;

    public Healthbar healthbar;
    public float maxHealth;
    float currentHealth;

    public ConfigurableJoint[] joints;
    public Rigidbody[] rigidbodies;

    private Vector3 ResetPosition;
    private Quaternion ResetRotation;
    public GameObject slappedParent;

    [SerializeField]
    private GameObject SpawnPoint;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        headRB = GetComponent<Rigidbody>();

        currentHealth = maxHealth;

        ResetPosition = SpawnPoint.transform.position;
        ResetRotation = SpawnPoint.transform.rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "SlapHand" && collision.relativeVelocity.magnitude > 2)
        {
            audioSource.Play();

            foreach (var joint in joints)
            {
                JointDrive jointXDrive = joint.angularXDrive;
                jointXDrive.positionSpring = 0f;
                joint.angularXDrive = jointXDrive;

                JointDrive jointYZDrive = joint.angularYZDrive;
                jointYZDrive.positionSpring = 0f;
                joint.angularYZDrive = jointYZDrive;
            }

            Debug.Log(collision.GetContact(0).normal);

            float score = collision.relativeVelocity.magnitude * scoreMultiplier;
            Debug.Log($"Score: {score}");
            TakeDamage(score);

            headRB.AddForce(collision.GetContact(0).normal * collision.relativeVelocity.magnitude * forceAmount);
            Debug.Log($"Score: {collision.relativeVelocity.magnitude * forceAmount}");

            StartCoroutine(Respawn(3));
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthbar.UpdateHealthbar(currentHealth / maxHealth);
    }

    private IEnumerator Respawn(float waitTime)
    {
        Debug.Log("Start");
        yield return new WaitForSeconds(waitTime);
        Debug.Log("End");

        foreach (var joint in joints)
        {
            JointDrive jointXDrive = joint.angularXDrive;
            jointXDrive.positionSpring = 500f;
            joint.angularXDrive = jointXDrive;

            JointDrive jointYZDrive = joint.angularYZDrive;
            jointYZDrive.positionSpring = 500f;
            joint.angularYZDrive = jointYZDrive;
        }

        slappedParent.transform.position = ResetPosition;
        slappedParent.transform.rotation = ResetRotation;
        // rigidBody.velocity = Vector3.zero;
        // rigidBody.angularVelocity = Vector3.zero;
    }
}
