using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    private AudioManager _audioManager;
    [SerializeField] private GameObject audioManagerGameobject;
    private AudioSource audioManagerAudioSource;

    private float clipLength;

    private float waitTimeToTalkNext;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        audioManagerAudioSource = audioManagerGameobject.GetComponent<AudioSource>();
        
        print(audioManagerAudioSource);

        //StartCoroutine(Play_RidiculeConversation_1());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator Play_RidiculeConversation_1()
    {
        //First dialogue line
        //PlayVoiceLine("1");
        waitTimeToTalkNext = PlayVoiceLine("1");
        Debug.Log("FirstVoiceLine");
        yield return new WaitForSeconds(waitTimeToTalkNext);   //WaitTime before playing next dialogue clip, so the speakers don't interrupt each other
        //Second dialogue line
        //PlayVoiceLine("2");
        waitTimeToTalkNext = PlayVoiceLine("2");
        yield return new WaitForSeconds(waitTimeToTalkNext);
        Debug.Log("SecondVoiceLine");
        
    }

    private float PlayVoiceLine(string AudioClipName)
    {
        clipLength = _audioManager.Play(AudioClipName);     //We extract the return value of the Play function, which is the length of the chosen audio clip.
        float waitTimeToTalkNext = (clipLength + Random.Range(0.15f, 0.5f));   //WaitTime before playing next dialogue clip, so the speakers don't interrupt each other
        
        return waitTimeToTalkNext;
    }
}
