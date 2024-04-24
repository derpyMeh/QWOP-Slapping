using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform camTransform;

    public GameObject lookTargetP1;
    public GameObject lookTargetP2;
    public GameObject lookTargetDefaultP1;
    public GameObject lookTargetDefaultP2;

    [SerializeField] ToggleRagdoll toggleRagdollScriptP1;
    [SerializeField] ToggleRagdoll toggleRagdollScriptP2;

    public float shakeAmount = 0.3f;
    public float shakeSpeed = 0.3f;

    Vector3 originalPos;
    Vector3 currentPos;
    Vector3 targetPos;

    Vector3 followOffSet;

    // Start is called before the first frame update
    void Start()
    {
        camTransform = GetComponent<Transform>();

        Cursor.lockState = CursorLockMode.Locked;

        originalPos = camTransform.localPosition;
        currentPos = originalPos;
        targetPos = camTransform.localPosition;

        // followOffSet = lookTargetDefaultP1.transform.position - originalPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(currentPos, targetPos) < 0.1)
        {
            targetPos = originalPos + Random.insideUnitSphere * shakeAmount;
        }

        currentPos = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * shakeSpeed);
        camTransform.localPosition = currentPos;
        
        if (toggleRagdollScriptP1.isSlapped) 
        {
            if (lookTargetP1.activeInHierarchy)
            {
                camTransform.LookAt(lookTargetP1.transform);
                // camTransform.position = lookTargetDefaultP1.transform.position - followOffSet;
            }
        } 
        else if (toggleRagdollScriptP2.isSlapped)
        {
            if (lookTargetP2.activeInHierarchy)
            {
                camTransform.LookAt(lookTargetP2.transform);
                // camTransform.position = lookTargetDefaultP2.transform.position - followOffSet;
            }
        }
        else
        {
            if (lookTargetDefaultP1.activeInHierarchy)
            {
                camTransform.LookAt(lookTargetDefaultP1.transform);
            }
            else
            {
                camTransform.LookAt(lookTargetDefaultP2.transform);
            }
        }
    }
}
