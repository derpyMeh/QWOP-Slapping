using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ArmController : MonoBehaviour
{
    public Transform target;
    ConfigurableJoint cjoint;
    Quaternion startRotation;

    public Slider QTESlide;
    public Image fillColor;
    public TMP_Text sliderText;
    public TMP_Text scoreText;
    public float scoreMultiplier = 0;
    private bool hasExecuted = false;

    float currentXRotation;
    float currentYRotation;
    float currentZRotation;
    
    public float mouseXvalue;

    float speed = 350.0f;

    public float maxY = 10f;
    public float minY = -10f;
    public float maxZ = 120f;
    public float minZ = -90f;

    // Start is called before the first frame update
    void Start()
    {
        cjoint = GetComponent<ConfigurableJoint>();
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        mouseXvalue = Input.GetAxis("Mouse X") * Time.deltaTime * speed;
        float mouseYvalue = Input.GetAxis("Mouse Y") * Time.deltaTime * speed;

        if (mouseXvalue != 0)
        {
            //print("Mouse X movement: " + mouseXvalue);
            currentZRotation += mouseXvalue;
        }
        if (mouseYvalue != 0)
        {
            //print("Mouse Y movement: " + mouseYvalue);
            currentYRotation += mouseYvalue;
        }


        QTESlide.value = currentZRotation;
        // QTESlide.value += mouseXvalue;

        ColorChange();


        if (QTESlide.value >= 100 && !hasExecuted)
        {
            switch (scoreMultiplier)
            {
                case 0:
                    scoreMultiplier = 1;
                    Debug.Log($"Score Multiplier: {scoreMultiplier}");
                    sliderText.text = scoreMultiplier.ToString();
                    hasExecuted = true;
                    break;
                case 1:
                    scoreMultiplier = 2;
                    Debug.Log($"Score Multiplier: {scoreMultiplier}");
                    sliderText.text = scoreMultiplier.ToString();
                    hasExecuted = true;
                    break;

                case 2:
                    scoreMultiplier = 3;
                    Debug.Log($"Score Multiplier: {scoreMultiplier}");
                    sliderText.text = scoreMultiplier.ToString();
                    hasExecuted = true;
                    break;
                default:
                    break;

            }


        }
        if ( QTESlide.value < 10 && hasExecuted)
        {
            hasExecuted = false;
            Debug.Log("Score Slider Reset");
        }

        SetCurrentRotation(currentXRotation, currentYRotation, currentZRotation);

        // target.transform.Rotate(0, mouseYvalue, mouseXvalue, Space.Self);

        ConfigurableJointExtensions.SetTargetRotationLocal(cjoint, target.rotation, startRotation);
    }

    void SetCurrentRotation(float xRot, float yRot, float zRot)
    {
        currentXRotation = Mathf.Clamp(xRot, -90, 90);
        currentYRotation = Mathf.Clamp(yRot, minY, maxY);
        currentZRotation = Mathf.Clamp(zRot, minZ, maxZ);
        target.transform.localRotation = Quaternion.Euler(xRot, yRot, zRot);
    }

    public void ResetTargetRotation()
    {
        currentXRotation = startRotation.x;
        currentYRotation = startRotation.y;
        currentZRotation = startRotation.z;
    }

    void ColorChange()
    {
        if (QTESlide.value <= 10)
        {
            fillColor.color = Color.red;
        }
        if (QTESlide.value >= 10)
        {
            fillColor.color = Color.yellow;
        }
        if (QTESlide.value >= 100)
        {
            fillColor.color = Color.green;
        }
    }
}
