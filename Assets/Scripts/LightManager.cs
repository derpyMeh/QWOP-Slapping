using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] Light sceneLight;
    
    //Change these bools in other scripts to enable/disable lights effects on the stage
    public bool FastSceneLights_RedWhite;
    public bool MediumSceneLights_RedWhite;
    public bool SlowSceneLights_RedWhite;

    public IEnumerator FastFlashing(float waitTime)
    {
        FastSceneLights_RedWhite = true;
        yield return new WaitForSeconds(waitTime);
        FastSceneLights_RedWhite = false;
        sceneLight.color = Color.white;
    }

    public IEnumerator MediumFlashing(float waitTime)
    {
        MediumSceneLights_RedWhite = true;
        yield return new WaitForSeconds(waitTime);
        MediumSceneLights_RedWhite = false;
        sceneLight.color = Color.white;
    }

    public IEnumerator SlowFlashing(float waitTime)
    {
        SlowSceneLights_RedWhite = true;
        yield return new WaitForSeconds(waitTime);
        SlowSceneLights_RedWhite = false;
        sceneLight.color = Color.white;
    }
}
