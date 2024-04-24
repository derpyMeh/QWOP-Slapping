using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class ToggleRagdoll : MonoBehaviour
{
    AudioSource audioSource;
    Rigidbody headRB;
    
    float forceAmount = 500f;
    float springStart = 500f;

    [SerializeField] ArmController armController;
    [SerializeField] LegsController legsController;
    [SerializeField] Animator animator;
    
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
    Quaternion pelvisStartRotation;

    public GameObject thigh_l;
    CopyAnimation thigh_l_animator;
    public GameObject thigh_r;
    CopyAnimation thigh_r_animator;
    public GameObject calf_l;
    CopyAnimation calf_l_animator;
    public GameObject calf_r;
    CopyAnimation calf_r_animator;

    public GameObject slappedParent;
    public GameObject slapperParent;

    public bool isSlapped = false;
    bool Walking = false;

    // Walk Testing
    

    [SerializeField] private GameObject WalkTarget;
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
        pelvisStartRotation = pelvis.transform.rotation;

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

        thigh_l_animator = thigh_l.GetComponent<CopyAnimation>();
        thigh_r_animator = thigh_r.GetComponent<CopyAnimation>();
        calf_l_animator = calf_l.GetComponent<CopyAnimation>();
        calf_r_animator = calf_r.GetComponent<CopyAnimation>();
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
            score = Mathf.Round(score);
            Debug.Log($"Score: {score}");
            TakeDamage(score);
            armController.scoreText.text = score.ToString();

            headRB.AddForce(collision.GetContact(0).normal * collision.relativeVelocity.magnitude * forceAmount);
            Debug.Log($"Force: {collision.relativeVelocity.magnitude * forceAmount}");

            StartCoroutine(StartWalking(3));
            //StartCoroutine(Respawn(3));
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

    public void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("Walking", true);
        } 
        else if (animator.GetBool("Walking"))
        {
            animator.SetBool("Walking", false);
        }

        if (Walking)
        {
            var lookPos = (WalkTarget.transform.position - pelvis.transform.position).normalized;
            var rotation = Quaternion.LookRotation(lookPos);
            rotation *= Quaternion.Euler(0, -90, -90);
            rotation *= Quaternion.Euler(180, 0, 0);
            //pelvis.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
            ConfigurableJointExtensions.SetTargetRotationLocal(pelvisJoint, rotation, pelvisStartRotation);
        }
    }

    private IEnumerator StartWalking(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        // legsController.isWalking = true;
        Walking = true;
        Debug.Log($"Walking: {legsController.isWalking}");

        foreach (var joint in joints)
        {
            JointDrive jointXDrive = joint.angularXDrive;
            jointXDrive.positionSpring = springStart - (1 / currentHealth * 1000);
            joint.angularXDrive = jointXDrive;

            JointDrive jointYZDrive = joint.angularYZDrive;
            jointYZDrive.positionSpring = springStart - (1 / currentHealth * 1000);
            joint.angularYZDrive = jointYZDrive;
        }
        JointDrive pelvisJointYZDrive = pelvisJoint.angularYZDrive;
        pelvisJointYZDrive.positionSpring = 750f - (1 / currentHealth * 1000);
        pelvisJoint.angularYZDrive = pelvisJointYZDrive;

        //thigh_l_animator.enabled = false;
        //thigh_r_animator.enabled = false;
        //calf_l_animator.enabled = false;
        //calf_r_animator.enabled = false;
    }

    private IEnumerator Respawn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        armController.ResetTargetRotation();
        armController.scoreMultiplier = 0;
        armController.scoreText.text = "";
        armController.sliderText.text = armController.scoreMultiplier.ToString();

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
                jointXDrive.positionSpring = springStart - (1 / currentHealth * 1000);
                joint.angularXDrive = jointXDrive;

                JointDrive jointYZDrive = joint.angularYZDrive;
                jointYZDrive.positionSpring = springStart - (1/currentHealth * 1000);
                joint.angularYZDrive = jointYZDrive;
            }
            JointDrive pelvisJointYZDrive = pelvisJoint.angularYZDrive;
            pelvisJointYZDrive.positionSpring = 1500f - (1 / currentHealth * 1000);
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
