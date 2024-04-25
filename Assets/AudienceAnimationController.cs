using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceAnimationController : MonoBehaviour
{
    [SerializeField] private Animator thisAnimator;

    private float counter;
    private float pauseDuration;
    
    // Start is called before the first frame update
    void Start()
    {
        thisAnimator = gameObject.GetComponent<Animator>();

        pauseDuration = Random.Range(5f, 12f);
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;

        if (pauseDuration >= counter)
        {
            
            thisAnimator.SetTrigger("isCelebrating");

            pauseDuration = Random.Range(5f, 12f);
            counter = 0;
        }
        
    }


}
