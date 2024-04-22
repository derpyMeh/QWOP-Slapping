using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleRagdoll : MonoBehaviour
{
    AudioSource audioSource;
    Rigidbody headRB;
    
    float forceAmount = 500f;
    [SerializeField] ArmController armController;
    float scoreMultiplier = 0;

    public Healthbar healthbar;
    public float maxHealth;
    float currentHealth;
    public GameObject defeatScreenUI;

    public ConfigurableJoint[] joints;
    public Rigidbody[] rigidbodies;
    public GameObject[] limbs;
    private Vector3[] resetPositions;
    private Quaternion[] resetRotations;

    public GameObject pelvis;
    ConfigurableJoint pelvisJoint;

    public GameObject slappedParent;
    public GameObject slapperParent;

    public bool isSlapped = false;
    
    [SerializeField] private GameObject OpponentSlapper;
    [SerializeField] private GameObject OpponentSlapped;
    public GameObject[] opponentLimbs;
    private Vector3[] opponentResetPositions;
    private Quaternion[] opponentResetRotations;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        headRB = GetComponent<Rigidbody>();
        pelvisJoint = pelvis.GetComponent<ConfigurableJoint>();

        currentHealth = maxHealth;

        resetPositions = new Vector3[limbs.Length];
        resetRotations = new Quaternion[limbs.Length];

        for (int i = 0; i < limbs.Length; i++)
        {
            resetPositions[i] = limbs[i].transform.position;
            resetRotations[i] = limbs[i].transform.rotation;
        }

        opponentResetPositions = new Vector3[opponentLimbs.Length];
        opponentResetRotations = new Quaternion[opponentLimbs.Length];

        for (int i = 0; i < opponentLimbs.Length; i++)
        {
            opponentResetPositions[i] = opponentLimbs[i].transform.position;
            opponentResetRotations[i] = opponentLimbs[i].transform.rotation;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "SlapHand" && collision.relativeVelocity.magnitude > 2)
        {
            isSlapped = true;
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
            scoreMultiplier = armController.scoreMultiplier;
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

    public void Defeated()
    {
        Debug.Log("Game Ended");
        defeatScreenUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        // Time.timeScale = 0f;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator Respawn(float waitTime)
    {
        Debug.Log("Start");
        yield return new WaitForSeconds(waitTime);
        Debug.Log("End");

        armController.ResetTargetRotation();

        if (currentHealth <= 0)
        {
            Defeated();
        }
        else
        {
            isSlapped = false;

            foreach (var joint in joints)
            {
                JointDrive jointXDrive = joint.angularXDrive;
                jointXDrive.positionSpring = 500f;
                joint.angularXDrive = jointXDrive;

                JointDrive jointYZDrive = joint.angularYZDrive;
                jointYZDrive.positionSpring = 500f;
                joint.angularYZDrive = jointYZDrive;
            }
            JointDrive pelvisJointYZDrive = pelvisJoint.angularYZDrive;
            pelvisJointYZDrive.positionSpring = 1500f;
            pelvisJoint.angularYZDrive = pelvisJointYZDrive;

            for (int i = 0; i < limbs.Length; i++)
            {
                limbs[i].transform.position = resetPositions[i];
                limbs[i].transform.rotation = resetRotations[i];
            }

            for (int i = 0; i < opponentLimbs.Length; i++)
            {
                opponentLimbs[i].transform.position = opponentResetPositions[i];
                opponentLimbs[i].transform.rotation = opponentResetRotations[i];
            }

            foreach (var rigidbody in rigidbodies)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }

            OpponentSlapper.SetActive(false);
            OpponentSlapped.SetActive(true);

            slapperParent.SetActive(true);
            slappedParent.SetActive(false);
        }
    }
}
