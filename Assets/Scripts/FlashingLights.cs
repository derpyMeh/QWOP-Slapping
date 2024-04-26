using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Serialization;

public class FlashingLights : MonoBehaviour
{
    [SerializeField] private LightManager _lightManager;
    [SerializeField] private Light thisPointLight;

    public bool FlashEffect_RedWhite = true;
    [FormerlySerializedAs("whiteColourActive")] private bool firstColourActive = true;
    private bool secondColourActive;
    

    private float timeCounter;
    private float timeTillTransition;

    [SerializeField] private float MinTime = 0.01f;
    [SerializeField] private float MaxTime = 0.05f;
    private float Timer;

    [SerializeField] private GameObject LEDlens;
    private Renderer LEDlensRenderer;

    public AudioSource AS;
    public AudioClip LightAudio;
    
    //private float timeToLerp;
    //[SerializeField] private float redCol_R, redCol_G, redCol_B;
    
    //[SerializeField] private int redCol_RGB();
    
    
    // Start is called before the first frame update
    void Start()
    {
        timeTillTransition = Random.Range(0.01f, 0.15f);
        //Timer = Random.Range(MinTime, MaxTime);
        _lightManager = GameObject.FindGameObjectWithTag("LightManager").GetComponent<LightManager>();

        LEDlensRenderer = LEDlens.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        
        if (Timer >= timeTillTransition && _lightManager.FastSceneLights_RedWhite)
        {
            FlashEffect_(0.015f,0.06f);
        }
        else if (Timer >= timeTillTransition && _lightManager.MediumSceneLights_RedWhite)
        {
            FlashEffect_(0.3f,0.12f);
        }
        else if (Timer >= timeTillTransition && _lightManager.SlowSceneLights_RedWhite)
        {
            FlashEffect_(0.6f,0.24f);
        }

        if (Input.GetKeyDown(KeyCode.Space))        //On event, use Lerp functions to change lights
        {
            //StartCoroutine(IntensityLightTransition(8f));
            StopAllCoroutines();
            StartCoroutine(LerpToIntensity(8f, 5f));
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            //StartCoroutine(IntensityLightTransition(8f));
            StopAllCoroutines();
            StartCoroutine(LerpToColour(Color.green, 5f));
        }
        
        /*if (FlashEffect_RedWhite)
        {
            FlashEffect_();
            /*timeCounter += Time.deltaTime;

            if (timeCounter >= timeTillTransition && firstColourActive == false)
            {
                FlashEffect(255,0,0,Random.Range(1f,3f));
                secondColourActive = false;
                firstColourActive = true;
            }
            if (timeCounter >= timeTillTransition && secondColourActive == false)
            {
                FlashEffect(255,255,255,Random.Range(1f,3f));
                firstColourActive = false;
                secondColourActive = true;
            }#1#

            /*if (timeCounter >= timeTillTransition && secondColourActive == true)
            {
                FlashEffect(255, 0, 0,Random.Range(2f,4f));
                /*firstColourActive = true;
                secondColourActive = false;#2#
            }
            if (timeCounter >= timeTillTransition && firstColourActive == true)
            {
                FlashEffect(255, 255, 255,Random.Range(2f,4f));
                /*secondColourActive = true;
                firstColourActive = false;#2#
            }#1#
            
           // MainLight.color = new Color32(System.Convert.ToByte(255, 0, 0));
        }*/
        
        /*float target = 1.0f;

        float delta = target - current;
        delta *= Time.deltaTime;

        current += delta;*/
        
    }

    void FlashEffect_(float minTime, float maxTime)
    {
        int colorPicker = Random.Range(1, 3);
        switch (colorPicker)
        {
            case 1:
                thisPointLight.color = Color.white;
                LEDlensRenderer.material.SetColor("_EmissionColor", Color.white);
                thisPointLight.intensity = Random.Range(3, 4);
                timeTillTransition = Random.Range(minTime, maxTime);
                Timer = 0;
                break;
            case 2:
                thisPointLight.color = Color.red;
                LEDlensRenderer.material.SetColor("_EmissionColor",Color.red);
                thisPointLight.intensity = Random.Range(3, 4);
                timeTillTransition = Random.Range(minTime, maxTime);
                Timer = 0;
                break;
            case 3:
                thisPointLight.color = Color.blue;
                LEDlensRenderer.material.SetColor("_EmissionColor", Color.blue);
                thisPointLight.intensity = Random.Range(3, 4);
                timeTillTransition = Random.Range(minTime, maxTime);
                Timer = 0;
                break;
        }
    }
    
    IEnumerator LerpToIntensity(float endValue, float duration)
    {
        float time = 0;
        float startValue = thisPointLight.intensity;

        while (time < duration)
        {
            thisPointLight.intensity = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        thisPointLight.intensity = endValue;
    }
    
    IEnumerator LerpToColour(Color endValue, float duration)
    {
        float time = 0;
        Color startColor = LEDlensRenderer.material.color;
        Color startLightColor= thisPointLight.color;

        while (time < duration)
        {
            LEDlensRenderer.material.SetColor("_EmissionColor",Color.Lerp(startColor, endValue, time / duration));
            thisPointLight.color = Color.Lerp(startLightColor, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        LEDlensRenderer.material.color = endValue;
    }

    void ChangeLightsToWhite()      //DOESNT LERP
    {
        thisPointLight.color = Color.white;
        LEDlensRenderer.material.SetColor("_EmissionColor",Color.white);
    }
    void ChangeLightsToRed()        //DOESNT LERP
    {
        thisPointLight.color = Color.red;
        LEDlensRenderer.material.SetColor("_EmissionColor", Color.red);
    }
    
    /*IEnumerator IntensityLightTransition(float targetIntensity)    //Max intensity is 8. Ranges from 0 to 8.
    {
        float currentIntensity = thisPointLight.intensity;
        
        if (currentIntensity > targetIntensity)
        {
            for (float i = currentIntensity; i > targetIntensity; i--)
            {
                Debug.Log("Decremented Intensity");
                thisPointLight.intensity --;
                yield return new WaitForSeconds(0.05f);
            }
        }

        if (currentIntensity < targetIntensity)
        {
            for (float i = currentIntensity; i < targetIntensity; i++)
            {
                Debug.Log("Incrememted Intensity");
                thisPointLight.intensity ++;
                        
                //i = thisPointLight.intensity;
                yield return new WaitForSeconds(0.05f);
            }
        }
        
    }*/
    
    
    
    
    /*void FlashEffect(float R_target, float G_target, float B_target, float timeToLerp)
    {

        /*float r = 180.0f / 255.0f;
        float g = 63.0f / 255.0f;
        float b = 255.0f / 255.0f;
        Color prettyColor = new Color (r, g, b);
        thisPointLight.color = prettyColor;#1#
        
        StartCoroutine(LerpToColour(R_target, G_target, B_target, timeToLerp));    
            
        //LerpToColour(255, 255, 255, timeToLerp);  //Lerp to white
            
         
        //firstColourActive = true;
        timeTillTransition = timeToLerp;
    }

    /*private Color GenerateRandomRGBColourFloat(int min, int max, float transitionTime)
    {
        int randomRColour = Random.Range(min, max);
        int GreenColour = Random.Range(min, max);
        int randomRColour = Random.Range(min, max);
    }#1#

    IEnumerator LerpToColour(float R_valueTarget, float G_valueTarget, float B_valueTarget, float transitionTime)
    {
        timeCounter = 0;
        R_valueTarget /= 100;       //Convert to 0-1 color scale
        G_valueTarget /= 100;
        B_valueTarget /= 100;
        
        var color = thisPointLight.color;
        //Convert the current colour to 255 color space

        if(timeCounter <= transitionTime)
        {
            color.r = Mathf.Lerp(color.r, R_valueTarget, timeCounter / transitionTime);
            color.g = Mathf.Lerp(color.g, B_valueTarget, timeCounter / transitionTime);
            color.b = Mathf.Lerp(color.b, G_valueTarget, timeCounter / transitionTime);
            thisPointLight.color = color;
        }

        yield return null;*/




        /*float deltaR = R_valueTarget - currentR_value;
        deltaR *= Time.deltaTime;
        currentR_value += deltaR;

        float deltaG = G_valueTarget - currentG_value;
        deltaG *= Time.deltaTime;
        currentG_value += deltaG;

        float deltaB = B_valueTarget - currentB_value;
        deltaB *= Time.deltaTime;
        currentB_value += deltaB;

        thisPointLight.color.*/


    
    /*Color endColor = Color.black;
    private float fadeTime = 2.5;
    private Color selectedColor;
    var lightcolour: Color[];

    private var tColor: float;

    function SetColor(){
        selection = lightcolour [Random.Range(0, lightcolour.length)];
        tColor = 0; // reset timer
    }

    function Update(){
        if (tColor <= 1){ // if end color not reached yet...
            tColor += Time.deltaTime / fadetime; // advance timer at the right speed
            light.color = Color.Lerp(selection, endColor, tColor);
        }
    }*/
}
