using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform camTransform;
    public GameObject lookTargetP1;
    public GameObject lookTargetP2;

    public float shakeAmount = 0.3f;
    public float shakeSpeed = 0.3f;
    Vector3 originalPos;
    Vector3 currentPos;
    Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        camTransform = GetComponent<Transform>();
        originalPos = camTransform.localPosition;
        currentPos = originalPos;
        targetPos = camTransform.localPosition;
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
        
        if (lookTargetP1.activeInHierarchy)
        {
            camTransform.LookAt(lookTargetP1.transform);
        } 
        else
        {
            camTransform.LookAt(lookTargetP2.transform);
        }
    }
}
