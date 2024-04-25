using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ToggleRagdoll : MonoBehaviour
{
    AudioSource audioSource;
    Rigidbody headRB;

    [SerializeField] AudioClip weakSlap;
    [SerializeField] AudioClip mediumSlap;
    [SerializeField] AudioClip hardSlap;

    float forceAmount = 500f;
    float springStart = 500f;
    float initialTime = 30f;
    float currentTime = 30f;

    [SerializeField] ArmController armController;
    [SerializeField] Animator animator;
    [SerializeField] LightManager lightManager;
    
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

    public GameObject slappedParent;
    public GameObject slapperParent;

    public bool isSlapped = false;
    public bool Walking = false;
    bool isDefeated = false;

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

        // Initializes all reset positions based on the placed position in the world when starting the game
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
            // To avoid multiple slaps triggering on same slap
            if (!isSlapped)
            {
                // Is set true until respawn function is called
                isSlapped = true;

                // Calculates Score
                scoreMultiplier = armController.scoreMultiplier;
                float score = collision.relativeVelocity.magnitude * scoreMultiplier;
                score = Mathf.Round(score);
                Debug.Log($"Score: {score}");
                TakeDamage(score);
                
                // Changes Variables Based on Score
                armController.scoreText.text = score.ToString();
                audioSource.volume = score / 100f * 2.5f;

                // Thresholds for scores that triggers certain effects
                if (score < 10)
                {
                    Debug.Log("Weak Slap");
                    armController.scoreText.color = Color.red;
                    audioSource.clip = weakSlap;
                    audioSource.Play();

                    JointDrive jointXDrive = joints[6].angularXDrive;
                    jointXDrive.positionSpring = 0;
                    joints[6].angularXDrive = jointXDrive;

                    JointDrive jointYZDrive = joints[6].angularYZDrive;
                    jointYZDrive.positionSpring = 0;
                    joints[6].angularYZDrive = jointYZDrive;

                    StartCoroutine(lightManager.SlowFlashing(3));
                    StartCoroutine(WaitThenRespawn(3));
                }
                else if (score < 20)
                {
                    Debug.Log("Good Slap");
                    armController.scoreText.color = Color.yellow;
                    audioSource.clip = mediumSlap;
                    audioSource.Play();

                    for (int i = 1; i < 6; i++)
                    {
                        JointDrive jointXDrive = joints[i].angularXDrive;
                        jointXDrive.positionSpring = 0;
                        joints[i].angularXDrive = jointXDrive;

                        JointDrive jointYZDrive = joints[i].angularYZDrive;
                        jointYZDrive.positionSpring = 0;
                        joints[i].angularYZDrive = jointYZDrive;
                    }

                    StartCoroutine(lightManager.MediumFlashing(3));
                    StartCoroutine(WaitThenRespawn(3));
                }
                else
                {
                    Debug.Log("Amazing Slap");
                    armController.scoreText.color = Color.green;
                    audioSource.clip = hardSlap;
                    audioSource.Play();

                    // Activates ragdoll based on all joint's springs get set to 0
                    foreach (var joint in joints)
                    {
                        JointDrive jointXDrive = joint.angularXDrive;
                        jointXDrive.positionSpring = 0f;
                        joint.angularXDrive = jointXDrive;

                        JointDrive jointYZDrive = joint.angularYZDrive;
                        jointYZDrive.positionSpring = 0f;
                        joint.angularYZDrive = jointYZDrive;
                    }

                    StartCoroutine(lightManager.FastFlashing(3));
                    // Starts the walking controls after a certain amounts of seconds with ragdoll
                    StartCoroutine(StartWalking(3));
                }

                // Adds force to the head based on the contact points normal vector and the relative velocity of the collision
                headRB.AddForce(collision.GetContact(0).normal * collision.relativeVelocity.magnitude * forceAmount);
                Debug.Log($"Force: {collision.relativeVelocity.magnitude * forceAmount}");
            }
        }
    }

    // Is called when a character has taken damage
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthbar.UpdateHealthbar(currentHealth / maxHealth);
    }

    // Is called when the given player the script is on has lost
    public void Defeated()
    {
        Debug.Log("Game Ended");
        isDefeated = true;
        defeatScreenUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        // Time.timeScale = 0f;
    }

    // for the restart button on the end screen
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Update()
    {
        // Walking Controls
        //Left
        if (Walking && Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool("WalkingLeft", true);
        } 
        else if (animator.GetBool("WalkingLeft"))
        {
            animator.SetBool("WalkingLeft", false);
        }

        //Right
        if (Walking && Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("WalkingRight", true);
        }
        else if (animator.GetBool("WalkingRight"))
        {
            animator.SetBool("WalkingRight", false);
        }

        if (Walking)
        {
            // Timer that counts down which triggers defeat if it reach 0
            if (!isDefeated)
            {
                currentTime -= Time.deltaTime;
                armController.scoreText.text = Mathf.Round(currentTime).ToString();
            }
            
            if (currentTime <= 0 && !isDefeated)
            {
                Defeated();
                armController.scoreText.text = "";
            }
            else if (currentTime < 10 && !isDefeated)
            {
                armController.scoreText.color = Color.red;
            }
            else if (currentTime < 20 && !isDefeated)
            {
                armController.scoreText.color = Color.yellow;
            }
            else if (currentTime < 10 && !isDefeated)
            {
                armController.scoreText.color = Color.green;
            }

            // Points the walking character towards WalkTarget gameobject's transform.position
            var lookPos = (WalkTarget.transform.position - pelvis.transform.position).normalized;
            var rotation = Quaternion.LookRotation(lookPos);
            rotation *= Quaternion.Euler(0, -90, -90);
            rotation *= Quaternion.Euler(180, 0, 0);
            ConfigurableJointExtensions.SetTargetRotationLocal(pelvisJoint, rotation, pelvisStartRotation);
        }
    }

    // Is called when the walking controls should activate
    private IEnumerator StartWalking(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (currentHealth <= 0)
        {
            Defeated();
        }
        else
        {
            Walking = true;
            currentTime = initialTime;

            foreach (var joint in joints)
            {
                JointDrive jointXDrive = joint.angularXDrive;
                jointXDrive.positionSpring = 500 - ((1 / currentHealth) * 100);
                joint.angularXDrive = jointXDrive;

                JointDrive jointYZDrive = joint.angularYZDrive;
                jointYZDrive.positionSpring = 500 - ((1 / currentHealth) * 100);
                joint.angularYZDrive = jointYZDrive;
            }

            for (int i = 0; i < 6; i++)
            {
                JointDrive jointXDrive = joints[i].angularXDrive;
                jointXDrive.positionSpring = 50 - ((1 / currentHealth) * 100);
                joints[i].angularXDrive = jointXDrive;

                JointDrive jointYZDrive = joints[i].angularYZDrive;
                jointYZDrive.positionSpring = 50 - ((1 / currentHealth) * 100);
                joints[i].angularYZDrive = jointYZDrive;
            }

            JointDrive pelvisJointYZDrive = pelvisJoint.angularYZDrive;
            pelvisJointYZDrive.positionSpring = 400f - ((1 / currentHealth) * 100);
            pelvisJoint.angularYZDrive = pelvisJointYZDrive;
        }
    }

    // Is called to reset all positions and start a new round where the players switch sides.
    public void Respawn()
    {
        armController.ResetTargetRotation();
        armController.scoreMultiplier = 0;
        armController.scoreText.text = "";
        armController.sliderText.text = armController.scoreMultiplier.ToString();

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

        ConfigurableJointExtensions.SetTargetRotationLocal(pelvisJoint, pelvisStartRotation, pelvisStartRotation);

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

    private IEnumerator WaitThenRespawn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Respawn();
    }
}
