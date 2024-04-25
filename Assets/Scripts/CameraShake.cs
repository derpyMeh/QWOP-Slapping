using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform camTransform;

    public GameObject lookTargetP1;
    public GameObject lookTargetP2;
    public GameObject lookTargetDefaultP1;
    public GameObject lookTargetDefaultP2;

    public GameObject player1;
    public GameObject player2;

    [SerializeField] ToggleRagdoll toggleRagdollScriptP1;
    [SerializeField] ToggleRagdoll toggleRagdollScriptP2;

    public float shakeAmount = 0.3f;
    public float shakeSpeed = 0.1f;

    Vector3 originalPos;
    Vector3 currentPos;
    Vector3 targetPos;


    // Start is called before the first frame update
    void Start()
    {
        camTransform = GetComponent<Transform>();

        Cursor.lockState = CursorLockMode.Locked;

        originalPos = camTransform.localPosition;
        //currentPos = originalPos;
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

        //currentPos = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * shakeSpeed);
        // camTransform.localPosition = currentPos;

        if (toggleRagdollScriptP1.isSlapped)
        {
            if (lookTargetP1.activeInHierarchy)
            {

                camTransform.position = new Vector3(Mathf.Lerp(currentPos.x, lookTargetP1.transform.position.x, Time.deltaTime*shakeAmount), lookTargetP1.transform.position.y, lookTargetP1.transform.position.z);
                camTransform.LookAt(lookTargetP1.transform);
                camTransform.position = new Vector3(lookTargetP1.transform.position.x + 2, lookTargetP1.transform.position.y, lookTargetP1.transform.position.z);
            }
        }
        else if (toggleRagdollScriptP2.isSlapped)
        {
            if (lookTargetP2.activeInHierarchy)
            {

                camTransform.position = new Vector3(Mathf.Lerp(currentPos.x, lookTargetP2.transform.position.x, Time.deltaTime*shakeAmount), lookTargetP2.transform.position.y, lookTargetP2.transform.position.z);
                camTransform.LookAt(lookTargetP2.transform);
                camTransform.position = new Vector3(lookTargetP2.transform.position.x+2 , lookTargetP2.transform.position.y, lookTargetP2.transform.position.z);
                // camTransform.transform.SetParent(lookTargetP2.transform);    //lookTargetDefaultP2.transform.position - followOffSet;
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
