using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FlashingLights : MonoBehaviour
{
    [SerializeField] private DirectionalLight MainLight;

    public bool FlashEffect_RedWhite;

    private float timeCounter;

    [SerializeField] private float redCol_R, redCol_G, redCol_B;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FlashEffect_RedWhite)
        {
            timeCounter += Time.deltaTime;
            
            
        }
    }
}
