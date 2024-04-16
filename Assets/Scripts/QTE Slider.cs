using UnityEngine;
using UnityEngine.UI;

public class QTESlider : MonoBehaviour
{

    public int goalTargetsHit = 3;
    public int targetsHit;
    bool QTEup;
    private float powerSpeed = 0.2f;
    public float powersMove = 0.0f;

    public Slider QTESlide;
    public Image fillColor;

    void Update()
    {

        if (QTESlide.value <= 0.1f)
        {
            powersMove = 0.0f;
            QTEup = true;
            powersMove += Time.deltaTime + powerSpeed;
        }
        else if (QTESlide.value >= 199.9999f)
        {
            powersMove = 0.0f;
            QTEup = false;
            powersMove -= Time.deltaTime + powerSpeed;
        }

        if (QTEup == true)
        {
            QTESlide.value += powersMove;

            ColorChange();
        }
        else if (!QTEup)
        {
            QTESlide.value += powersMove;
            ColorChange();

        }

        if (Input.GetKeyDown(KeyCode.Space) && QTESlide.value >= 180.0f)
        {
            targetsHit++;
            Debug.Log("Spacekey hit");
            if (targetsHit >= 3)
            {
                //YouWin!!
            }

        }
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
