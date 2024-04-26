using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    private DialogueSetup _dialogueSetup;
    [SerializeField] private GameObject audioManagerGameobject;
    private AudioSource audioManagerAudioSource;

    private float clipLength;

    private float waitTimeToTalkNext;
    
    // Start is called before the first frame update
    void Start()
    {
        _dialogueSetup = FindObjectOfType<DialogueSetup>();
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
        clipLength = _dialogueSetup.Play(AudioClipName);     //We extract the return value of the Play function, which is the length of the chosen audio clip.
        float waitTimeToTalkNext = (clipLength + Random.Range(0.15f, 0.5f));   //WaitTime before playing next dialogue clip, so the speakers don't interrupt each other
        
        return waitTimeToTalkNext;
    }

    public void playRandomArnold_Insult()
    {
        int randomInt = Random.Range(1, 6);
        switch (randomInt)
        {
            case 1:
                _dialogueSetup.Play("Insult1");
                break;
            case 2:
                _dialogueSetup.Play("Insult2");
                break;
            case 3:
                _dialogueSetup.Play("Insult3");
                break;
            case 4:
                _dialogueSetup.Play("Insult4");
                break;
            case 5:
                _dialogueSetup.Play("Insult5");
                break;
        }
    }
    
    public void playRandomArnold_Neutral()
    {
        int randomInt = Random.Range(1, 6);
        switch (randomInt)
        {
            case 1:
                _dialogueSetup.Play("Neutral1");
                break;
            case 2:
                _dialogueSetup.Play("Neutral2");
                break;
            case 3:
                _dialogueSetup.Play("Neutral3");
                break;
        }
        
    }
    
    public void playRandomArnold_Good()
    {
        int randomInt = Random.Range(1, 4);
        switch (randomInt)
        {
            case 1:
                _dialogueSetup.Play("Commend1");
                break;
            case 2:
                _dialogueSetup.Play("Commend2");
                break;
            case 3:
                _dialogueSetup.Play("Commend3");
                break;
        }
        
    }
    
}
