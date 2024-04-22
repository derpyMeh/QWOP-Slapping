using UnityEngine;
using UnityEngine.UI;

public class ArmController : MonoBehaviour
{
    public Transform target;
    ConfigurableJoint cjoint;
    Quaternion startRotation;

    public Slider QTESlide;
    public Image fillColor;
    public float scoreMultiplier;

    float currentXRotation;
    float currentYRotation;
    float currentZRotation;

    Transform localTransform;

    float speed = 350.0f;

    public float maxY = 90f;
    public float minY = -90f;
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
        float mouseXvalue = Input.GetAxis("Mouse X") * Time.deltaTime * speed;
        float mouseYvalue = Input.GetAxis("Mouse Y") * Time.deltaTime * speed;

        if (mouseXvalue != 0)
        {
            //print("Mouse X movement: " + mouseXvalue);
            currentZRotation += mouseXvalue;
            // target.transform.Rotate(0, 0, mouseXvalue, Space.Self);
        }
        if (mouseYvalue != 0)
        {
            //print("Mouse Y movement: " + mouseYvalue);
            currentYRotation += mouseYvalue;
            // target.transform.Rotate(0, mouseYvalue, 0, Space.Self);
        }


        QTESlide.value += mouseXvalue;

        ColorChange();


        if (QTESlide.value >= 180)
        {
            switch (scoreMultiplier)
            {
                case 0:
                    scoreMultiplier = 1;
                    break;
                case 1:
                    scoreMultiplier = 2;
                    break;

                case 2:
                    scoreMultiplier = 3;
                    break;
                default:
                    break;

            }

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

    void ColorChange()
    {
        if (QTESlide.value <= 100)
        {
            fillColor.color = Color.red;
        }
        if (QTESlide.value >= 100)
        {
            fillColor.color = Color.yellow;
        }
        if (QTESlide.value >= 180)
        {
            fillColor.color = Color.green;
        }
    }
}
