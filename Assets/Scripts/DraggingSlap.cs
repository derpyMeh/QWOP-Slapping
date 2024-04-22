using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggingSlap : MonoBehaviour, IPointerEnterHandler
{
    public float timerGoing;
    public GameObject centreZone;
    public GameObject[] innerLayer;
    public GameObject[] outerLayer;
    public GameObject currentObj;

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        timerGoing += Time.deltaTime;
        if (currentObj != centreZone)
        {
            for (int i = 0; i < 2; i++)
            {
                if (currentObj == innerLayer[i])
                {
                    timerGoing += Time.deltaTime*1.5f;
                    Debug.Log("InnerLayerHit");
                }
            }

            for (int i = 0; i < 2; i++)
            {
                if (currentObj == outerLayer[i])
                {
                    timerGoing += Time.deltaTime * 2f;
                    Debug.Log("OuterLayerHit");
                }
            }
        }

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Log the name of the GameObject the pointer is over
        currentObj = eventData.pointerCurrentRaycast.gameObject;
    }

}

