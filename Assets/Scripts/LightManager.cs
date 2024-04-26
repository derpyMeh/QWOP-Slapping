using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private GameObject sceneLightLED_Gameobject;
    
    [SerializeField] Light sceneLight;

    private Renderer sceneLightLED_Renderer;
    
    //Change these bools in other scripts to enable/disable lights effects on the stage
    public bool FastSceneLights_RedWhite;
    public bool MediumSceneLights_RedWhite;
    public bool SlowSceneLights_RedWhite;

    private void Start()
    {
        sceneLightLED_Renderer = sceneLightLED_Gameobject.GetComponent<Renderer>();
    }

    public IEnumerator FastFlashing(float waitTime)
    {
        FastSceneLights_RedWhite = true;
        yield return new WaitForSeconds(waitTime);
        FastSceneLights_RedWhite = false;
        StartCoroutine(LerpToColour(Color.white, 2));
        StartCoroutine(LerpToIntensity(3.2f, 2));
    }

    public IEnumerator MediumFlashing(float waitTime)
    {
        MediumSceneLights_RedWhite = true;
        yield return new WaitForSeconds(waitTime);
        MediumSceneLights_RedWhite = false;
        StartCoroutine(LerpToColour(Color.white, 2));
        StartCoroutine(LerpToIntensity(3.2f, 2));
    }

    public IEnumerator SlowFlashing(float waitTime)
    {
        SlowSceneLights_RedWhite = true;
        yield return new WaitForSeconds(waitTime);
        SlowSceneLights_RedWhite = false;
        StartCoroutine(LerpToColour(Color.white, 2));
        StartCoroutine(LerpToIntensity(3.2f, 2));
    }
    
    public IEnumerator LerpToIntensity(float endValue, float duration)
    {
        float time = 0;
        float startValue = sceneLight.intensity;

        while (time < duration)
        {
            sceneLight.intensity = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        sceneLight.intensity = endValue;
    }
    
    public IEnumerator LerpToColour(Color endValue, float duration)
    {
        float time = 0;
        Color startColor = sceneLightLED_Renderer.material.color;
        Color startLightColor= sceneLight.color;

        while (time < duration)
        {
            sceneLightLED_Renderer.material.SetColor("_EmissionColor",Color.Lerp(startColor, endValue, time / duration));
            sceneLight.color = Color.Lerp(startLightColor, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        sceneLightLED_Renderer.material.color = endValue;
    }
    
}
